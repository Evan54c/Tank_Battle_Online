using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Common.Tools;
using Common;
using System.Xml.Serialization;
using System.IO;

public class GetinRequest : Request {

    //请求同步其他客户端的移动类
    private PlayerManager playerManager;

    [HideInInspector]
    public Vector3 pos;
    [HideInInspector]
    public DirectiontCode direction;
    [HideInInspector]
    public string username;

    public override void Start()
    {
        base.Start();
        playerManager = GetComponent<PlayerManager>();
        pos=new Vector3();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.X, pos.x);
        data.Add((byte)ParameterCode.Y, pos.y);
        data.Add((byte)ParameterCode.Z, pos.z);
        data.Add((byte)ParameterCode.Direction, direction);
        data.Add((byte)ParameterCode.Username, username);
        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        List<List<Actions>> ActionList = new List<List<Actions>>();
        List<PlayerData> PlayerDataList = new List<PlayerData>();
        List<Queue<Actions>> ActionQueueList = new List<Queue<Actions>>();

        //提取响应的数据
        string playerDataListString = DicTool.GetValue<byte, object>(operationResponse.Parameters,(byte)ParameterCode.PlayerDataList) as string;
        using (StringReader reader = new StringReader(playerDataListString))
        {
            //利用XML得到响应字符串链表
            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerData>));
            PlayerDataList = (List<PlayerData>)serializer.Deserialize(reader);
        }

        string actionListString = (string)DicTool.GetValue<byte, object>(operationResponse.Parameters, (byte)ParameterCode.ActionData);
        using (StringReader reader2 = new StringReader(actionListString))
        {
            XmlSerializer serializer2 = new XmlSerializer(typeof(List<List<Actions>>));
            ActionList = (List<List<Actions>>)serializer2.Deserialize(reader2);
        }

        List<Actions> al = new List<Actions>();
        Queue<Actions> aq = new Queue<Actions>();
        Actions a;
        for (int i = 0; i < ActionList.Count; i++)
        {
            al = ActionList[i];
            for(int j = 0; j < al.Count; j++)
            {
                a = al[j];
                aq.Enqueue(a);
            }
            ActionQueueList.Add(aq);
        }

        //处理响应
        playerManager.OnGetinResponse(PlayerDataList, ActionQueueList);
    }

}
