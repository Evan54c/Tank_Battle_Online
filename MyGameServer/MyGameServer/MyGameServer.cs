using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using System.IO;
using log4net.Config;
using MyGameServer.Manager;
using Common;
using MyGameServer.Handler;
using MyGameServer.Threads;

namespace MyGameServer
{
    //所有的server端 主类都要继承自ApplicationBase
    public class MyGameServer : ApplicationBase
    {
        //日志输出
        public static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static MyGameServer Instance
        {
            get;
            private set;
        }

        //响应请求的Handler的字典，可以根据OperationCode查找对应的Handler
        public Dictionary<OperationCode, BaseHandler> HandlerDict = new Dictionary<OperationCode, BaseHandler>();

        //多客户端响应的同步线程
        private SyncPositionThread syncPositionThread = new SyncPositionThread();

        //用链表存储所有连接的客户端的peer，可用于向任意一个客户端发送数据
        public List<ClientPeer> peerList = new List<ClientPeer>();

        //当一个客户端请求连接时调用，使用peerbase表示和一个客户端的连接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("连接上了一个客户端！");
            //每个客户端创建一个ClientPeer对象，便于管理
            ClientPeer peer = new ClientPeer(initRequest);
            peerList.Add(peer);
            return peer;
        }

        //服务器端应用启动时初始化
        protected override void Setup()
        {
            Instance = this;
            //日志初始化
            log4net.GlobalContext.Properties["Photon:AppliactionLogPath"] = Path.Combine(
                                                               Path.Combine(this.ApplicationRootPath, "bin_Win64"), "log");
            FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance); //让photon知道使用的是log4net日志插件
                XmlConfigurator.ConfigureAndWatch(configFileInfo);  //让log4net读取配置文件
            }
            log.Info("Setup Completed! 初始化完成，服务器端启动！");
            //初始化Handler字典，将Handler和对应的opCode填入字典中，准备接受各种Handler
            InitHandler();
            //初始化同步
            syncPositionThread.Run();
        }

        //各种Handler初始化
        public void InitHandler()
        {
            //登录Handler
            LoginHandler loginHandler = new LoginHandler();
            HandlerDict.Add(loginHandler.opCode, loginHandler);

            //默认Handler
            DefaultHandler defaultHandler = new DefaultHandler();
            HandlerDict.Add(defaultHandler.opCode, defaultHandler);

            //注册Handler
            RegisterHandler registerHandler = new RegisterHandler();
            HandlerDict.Add(registerHandler.opCode, registerHandler);

            //同步位置上传Handler
            SyncActionHandler syncPositionHandler = new SyncActionHandler();
            HandlerDict.Add(syncPositionHandler.opCode, syncPositionHandler);

            //玩家加入房间Handler
            GetinHandler getinHandler = new GetinHandler();
            HandlerDict.Add(getinHandler.opCode, getinHandler);

            //房间请求Handler
            RoomHandler roomHandler = new RoomHandler();
            HandlerDict.Add(roomHandler.opCode, roomHandler);

            //复活请求Handler
            RecoverHandler recoverHandler = new RecoverHandler();
            HandlerDict.Add(recoverHandler.opCode, recoverHandler);
        }

        //server端关闭时的操作
        protected override void TearDown()
        {
            syncPositionThread.Stop();
            log.Info("服务器应用关闭！");
        }
    }
}
