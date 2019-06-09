using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public class SyncActionRequest : Request
{
    //当前客户端物体位置变化的请求类
    [HideInInspector]
    public Queue<Actions> act;


    public override void Start() 
    {
        base.Start();
        act=new Queue<Actions>();
    }
    public override void DefaultRequest()
    {
        Debug.Log(act.Count);
        //act=new Queue<Actions>();
        List<Actions> actList = new List<Actions>();
        int num = act.Count;
        for (int i = 0; i < num; i++)
        {
            actList.Add(act.Dequeue());
        }

        //利用XML将已登录用户信息转化为字符串，便于发送         
        StringWriter stringWriter = new StringWriter();
        XmlSerializer serializer = new XmlSerializer(typeof(List<Actions>));
        serializer.Serialize(stringWriter, actList);
        stringWriter.Close();
        string actString = stringWriter.ToString();
        //Debug.Log(actString);

        //将字符串装载到响应中
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.ActQueue, actString);
        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }

    

}
