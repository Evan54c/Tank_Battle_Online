using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
using Common.Tools;
using MyGameServer.Manager;


namespace MyGameServer.Handler
{
    class LoginHandler : BaseHandler  //登录请求对应的Handler
    {
        public LoginHandler()
        {
            opCode = OperationCode.Login;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string username = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Username) as string;
            string password = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Password) as string;

            //连数据库查询
            UserManager manager = new UserManager();
            bool isSuccess = manager.VerifyUser(username, password);

            //响应，直接用请求时的OperationCode作为响应的OperationCode
            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            if(isSuccess)
            {
                //登录成功并用ReturnCode作为数据返回
                response.ReturnCode = (short)ReturnCode.Success;
                peer.username = username;
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Failed;
            }
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
