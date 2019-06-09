using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Common;
using System.Xml.Serialization;
using System.IO;
using Photon.SocketServer;


namespace MyGameServer.Threads
{
    public class SyncPositionThread
    {
        private Thread t;

        public void Run()
        {
            t = new Thread(UpdatePosition);
            t.IsBackground = true;
            t.Start();
        }

        public void Stop()
        {
            t.Abort();
        }

        //同步位置，向客户端发送Event
        private void UpdatePosition()
        {
            Thread.Sleep(4000);
            MyGameServer.log.Info("进入同步线程！");
            while(true)
            {
                //每秒发送20次
                Thread.Sleep(50);
                SendPosition();

            }
        }

        private void SendPosition()
        {

            //取得所有已登录用户信息
            List<string> usernameList = new List<string>();
            List<List<Actions>> ActionList = new List<List<Actions>>();
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (string.IsNullOrEmpty(tempPeer.username) == false)
                {
                    usernameList.Add(tempPeer.username);
                    List<Actions> AL = new List<Actions>();
                    if(tempPeer.ActionQueue.Count == 0)
                    {
                        AL.Add(Actions.stay);
                    }
                    else
                    {
                        while (tempPeer.ActionQueue.Count != 0)
                        {
                            AL.Add(tempPeer.ActionQueue.Dequeue());
                        }                        
                    }
                    ActionList.Add(AL);
                }
            }

            //利用XML将已登录用户信息转化为字符串，便于发送         
            StringWriter stringWriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            serializer.Serialize(stringWriter, usernameList);
            stringWriter.Close();
            string usernameListString = stringWriter.ToString();

            //将字符串装载到响应中
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.UsernameList, usernameListString);

            StringWriter stringWriter2 = new StringWriter();
            XmlSerializer serializer2 = new XmlSerializer(typeof(List<List<Actions>>));
            serializer2.Serialize(stringWriter2, ActionList);
            stringWriter2.Close();
            string actionListString = stringWriter2.ToString();

            //if (usernameList.Count > 1 && (ActionList[0].Count > 1 || ActionList[1].Count > 1))
            //{
            //    MyGameServer.log.Info("用户名序列为：" + usernameListString);
            //    MyGameServer.log.Info("动作序列为：" + actionListString);
            //}


            //将字符串装载到响应中
            data.Add((byte)ParameterCode.ActionData, actionListString);

            //告诉所有客户端
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (string.IsNullOrEmpty(tempPeer.username) == false)
                {
                    EventData ed = new EventData((byte)EventCode.SyncAction);
                    ed.Parameters = data;
                    tempPeer.SendEvent(ed, new SendParameters());
                }
            }

        }


    }
}
