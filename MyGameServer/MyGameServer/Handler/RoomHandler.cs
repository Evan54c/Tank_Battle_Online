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
    class RoomHandler : BaseHandler
    {
        public RoomHandler()
        {
            opCode = OperationCode.Room;
        }


        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string roomID = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.RoomID) as string;
            peer.roomID = roomID;

            //取得所有已登录用户信息
            List<string> usernameList = new List<string>();
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (string.IsNullOrEmpty(tempPeer.username) == false && tempPeer.roomID == roomID)
                {
                    usernameList.Add(tempPeer.username);
                }
            }

            if(usernameList.Count == 0)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode);
                response.ReturnCode = (short)ReturnCode.Failed;
                peer.SendOperationResponse(response, sendParameters);
            }
            else
            {
                //利用XML将已登录用户信息转化为字符串，便于发送         
                StringWriter stringWriter = new StringWriter();
                XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                serializer.Serialize(stringWriter, usernameList);
                stringWriter.Close();
                string usernameListString = stringWriter.ToString();

                //将字符串装载到响应中
                Dictionary<byte, object> data = new Dictionary<byte, object>();
                data.Add((byte)ParameterCode.UsernameList, usernameListString);
                OperationResponse response = new OperationResponse(operationRequest.OperationCode);
                response.Parameters = data;
                peer.SendOperationResponse(response, sendParameters);

                //告诉房间内的所有客户端
                foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
                {
                    if (string.IsNullOrEmpty(tempPeer.username) == false && tempPeer.roomID == roomID && tempPeer.username != peer.username)
                    {
                        EventData ed = new EventData((byte)EventCode.Room);
                        Dictionary<byte, object> data2 = new Dictionary<byte, object>();
                        data2.Add((byte)ParameterCode.UsernameList, peer.username);
                        ed.Parameters = data2;
                        tempPeer.SendEvent(ed, new SendParameters());
                    }
                }
            }
        }
    }
}
