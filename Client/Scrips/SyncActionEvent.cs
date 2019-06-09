using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Common.Tools;
using Common;
using System.Xml.Serialization;
using System.IO;

public class SyncActionEvent : BaseEvent
{

    private PlayerManager playerManager;

    public override void Start()
    {
        base.Start();
        playerManager = GetComponent<PlayerManager>();
    }

    public override void OnEvent(EventData eventData)
    {
        List<string> usernameList = new List<string>();
        List<List<Actions>> ActionList = new List<List<Actions>>();
        List<Queue<Actions>> ActionQueueList = new List<Queue<Actions>>();
        string usernameListString = (string) DicTool.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.UsernameList);
      
      Debug.Log("GetName:"+usernameListString);
      
       using (StringReader reader1 = new StringReader(usernameListString))
       {
           XmlSerializer serializer1 = new XmlSerializer(typeof(List<string>));
           usernameList = (List<string>)serializer1.Deserialize(reader1);
       }

       string actionListString = (string) DicTool.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.ActionData);
       using (StringReader reader2 = new StringReader(actionListString))
       {
           XmlSerializer serializer2 = new XmlSerializer(typeof(List<List<Actions>>));
           ActionList = (List<List<Actions>>)serializer2.Deserialize(reader2);
       }

        Debug.Log("GetAction:"+actionListString);


        List<Actions> al = new List<Actions>();
        //Queue<Actions> aq = new Queue<Actions>();
        Actions a;
        for (int i = 0; i < ActionList.Count; i++)
        {
            al = ActionList[i];
             Queue<Actions> aq = new Queue<Actions>();
            for (int j = 0; j < al.Count; j++)
            {
               
                a = al[j];
                aq.Enqueue(a);
            }
            ActionQueueList.Add(aq);
           // aq.Clear();
        }

        Dictionary<string, Queue<Actions>> actionDictionary = new Dictionary<string, Queue<Actions>>();
        for (int i = 0; i < ActionQueueList.Count; i++)
        {
            actionDictionary.Add(usernameList[i], ActionQueueList[i]);
        }

        //处理事件
        playerManager.OnSyncActionEvent(actionDictionary);

    }


}
