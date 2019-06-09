using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Tools;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    class ExitHandler : BaseHandler
    {
        public ExitHandler ()
        {
            opCode = OperationCode.Exit;
        }


        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {

            MyGameServer.log.Info("进入了ExitHandler");
            //将退出的用户从服务器端的客户列表中删除  
            MyGameServer.Instance.peerList.Remove(peer);
            MyGameServer.log.Info(peer.username+"用户退出了！");
        }
    }
}
