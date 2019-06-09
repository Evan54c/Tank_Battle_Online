using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;

namespace MyGameServer.Handler
{
    class DefaultHandler : BaseHandler  //DefaultHandler类，对OperationCode不明的请求不做处理
    {
        public DefaultHandler()
        {
            opCode = OperationCode.Default;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            throw new NotImplementedException();
        }
    }
}
