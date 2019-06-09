using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using Common;
using Common.Tools;
using MyGameServer.Handler;


namespace MyGameServer
{
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        //每个客户端物体的三维坐标,及朝向
        public float x, y, z;
        public DirectiontCode direction;
        public Queue<Actions> ActionQueue;
        public Queue<Actions> ActionQueueCopy;
        public string roomID;

        //每个客户端的用户名
        public string username;

        //继承自父类的构造方法
        public ClientPeer(InitRequest initRequest):base(initRequest)
        {
           
            ActionQueue = new Queue<Actions>();
            ActionQueueCopy = new Queue<Actions>();
        }
        
        //客户端断开连接的后续操作
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            //从服务器端的peerList移除当前客户端的peer
            MyGameServer.Instance.peerList.Remove(this);
        }

        //处理客户端的请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            //MyGameServer.log.Info("收到了一个客户端的请求!!!");
            //根据OperationCode在Handler字典中查找响应的Handler
            BaseHandler handler = DicTool.GetValue<OperationCode, BaseHandler>(MyGameServer.Instance.HandlerDict, (OperationCode)operationRequest.OperationCode);
            if(handler != null)
            {
                //调用Handler处理请求
                handler.OnOperationRequest(operationRequest, sendParameters, this);

            }
            else
            {
                //未在Handler字典中查找到相应响应的Handler，默认调用defaultHandler
                BaseHandler defaultHandler = DicTool.GetValue<OperationCode, BaseHandler>(MyGameServer.Instance.HandlerDict, (OperationCode)operationRequest.OperationCode);
                defaultHandler.OnOperationRequest(operationRequest, sendParameters, this);
            }
        }
    }
}
