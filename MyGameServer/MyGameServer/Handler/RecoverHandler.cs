using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Tools;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    class RecoverHandler : BaseHandler
    {
        public RecoverHandler()
        {
            opCode = OperationCode.Recover;
        }
        
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            MyGameServer.log.Info("进入了RecoverHandler");
            //记录登入信息
            float x = (float)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.X);
            float y = (float)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Y);
            float z = (float)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Z);
            DirectiontCode direction = (DirectiontCode)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Direction);
            string username = (string)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Username);

            while (peer.ActionQueue.Count != 0)
                peer.ActionQueue.Dequeue();

            while (peer.ActionQueueCopy.Count != 0)
                peer.ActionQueueCopy.Dequeue();

            Thread.Sleep(1000);


            //告诉其他客户端有客户端复活，并传输其方位和用户名
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (string.IsNullOrEmpty(tempPeer.username) == false && tempPeer.username != peer.username)
                {
                    EventData ed = new EventData((byte)EventCode.Recover);
                    Dictionary<byte, object> data = new Dictionary<byte, object>();
                    data.Add((byte)ParameterCode.X, x);
                    data.Add((byte)ParameterCode.Y, y);
                    data.Add((byte)ParameterCode.Z, z);
                    data.Add((byte)ParameterCode.Direction, direction);
                    data.Add((byte)ParameterCode.Username, peer.username);
                    ed.Parameters = data;
                    tempPeer.SendEvent(ed, sendParameters);
                }
            }



        }
    }
}
