using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
using Common.Tools;
using System.IO;
using System.Xml.Serialization;

namespace MyGameServer.Handler
{
    class SyncActionHandler : BaseHandler  //SyncPositionHandler类处理客户端的移动同步请求
    {
        public SyncActionHandler()
        {
            opCode = OperationCode.Action;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //将客户端的动作传到服务器端
            List<Actions> act = new List<Actions>();
            Actions a ;
            string actString = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.ActQueue) as string;

            //if (act.Count != 0)
            //{
            //    MyGameServer.log.Info(peer.username + "用户的动作为：" + actString);
            //}


            using (StringReader reader = new StringReader(actString))
            {
                //利用XML得到响应字符串链表
                XmlSerializer serializer = new XmlSerializer(typeof(List<Actions>));
                act = (List<Actions>)serializer.Deserialize(reader);
            }

            
            for (int i = 0; i < act.Count; i++)
            {
                a = act[i];
                peer.ActionQueue.Enqueue(a);
                peer.ActionQueueCopy.Enqueue(a);
            }
        }
    }
}
