using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
using System.Xml.Serialization;
using System.IO;
using Common.Tools;

namespace MyGameServer.Handler
{
    class GetinHandler : BaseHandler    //处理登入客户端的其他玩家移动同步请求
    {
        public GetinHandler()
        {
            opCode = OperationCode.Getin;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            MyGameServer.log.Info("进入了GetinHandler");
            //记录登入信息
            float x = (float)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.X);
            float y = (float)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Y);
            float z = (float)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Z);
            DirectiontCode direction = (DirectiontCode)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Direction);
            string username = (string)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Username);
            peer.x = x; peer.y = y; peer.z = z; peer.direction = direction; peer.username = username;

            MyGameServer.log.Info("用户名为："+ username);

            //取得所有已登录用户信息
            List<PlayerData> PlayerDataList = new List<PlayerData>();
            List<List<Actions>> ActionList = new List<List<Actions>>();
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if(string.IsNullOrEmpty(tempPeer.username) == false && tempPeer.username != peer.username)
                {
                    Vector3Data Pos = new Vector3Data() { x = tempPeer.x, y = tempPeer.y, z = tempPeer.z, direction = tempPeer.direction };
                    PlayerData playerData = new PlayerData() { Pos = Pos, Username = tempPeer.username };
                    PlayerDataList.Add(playerData);

                    List<Actions> AL = new List<Actions>();
                    Queue<Actions> AQ = new Queue<Actions>(tempPeer.ActionQueueCopy);
                    if (AQ.Count == 0)
                    {
                        AL.Add(Actions.stay);
                    }
                    else
                    {
                        while (AQ.Count != 0)
                        {
                            AL.Add(AQ.Dequeue());
                        }
                    }
                    ActionList.Add(AL);
                }
            }

            //利用XML将已登录用户信息转化为字符串，便于发送         
            StringWriter stringWriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerData>));
            serializer.Serialize(stringWriter, PlayerDataList);
            stringWriter.Close();
            string playerDataListString = stringWriter.ToString();


            //将字符串装载到响应中，响应新加入客户端，告诉新加入的客户端其他客户端的用户名
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.PlayerDataList, playerDataListString);

            StringWriter stringWriter2 = new StringWriter();
            XmlSerializer serializer2 = new XmlSerializer(typeof(List<List<Actions>>));
            serializer2.Serialize(stringWriter2, ActionList);
            stringWriter2.Close();
            string actionListString = stringWriter2.ToString();


            //将字符串装载到响应中
            data.Add((byte)ParameterCode.ActionData, actionListString);

            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            response.Parameters = data;
            peer.SendOperationResponse(response, sendParameters);


            //告诉其他客户端有新的客户端加入，并传输其方位和用户名
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (string.IsNullOrEmpty(tempPeer.username) == false && tempPeer.username != peer.username)
                {
                    EventData ed = new EventData((byte)EventCode.NewPlayer);
                    Dictionary<byte, object> data2 = new Dictionary<byte, object>();
                    data2.Add((byte)ParameterCode.X, x);
                    data2.Add((byte)ParameterCode.Y, y);
                    data2.Add((byte)ParameterCode.Z, z);
                    data2.Add((byte)ParameterCode.Direction, direction);
                    data2.Add((byte)ParameterCode.Username, username);
                    ed.Parameters = data2;
                    tempPeer.SendEvent(ed, sendParameters);                    
                }
            }
        }
    }
}
