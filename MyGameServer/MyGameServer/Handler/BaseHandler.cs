using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    public abstract class BaseHandler  //所有Handler的父类，包含用于对应请求的OperationCode，和响应的抽象方法
    {
        public OperationCode opCode;
        public abstract void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters,ClientPeer peer);
    }
}
