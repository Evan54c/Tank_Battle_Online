using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Common;
using Common.Tools;


public class NewPlayerEvent : BaseEvent
{

    private PlayerManager playerManager;

    public override void Start()
    {
        base.Start();
        playerManager = GetComponent<PlayerManager>();
    }

    public override void OnEvent(EventData eventData)
    {
        float x = (float)DicTool.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.X);
        float y = (float)DicTool.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.Y);
        float z = (float)DicTool.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.Z);
        DirectiontCode direction = (DirectiontCode)DicTool.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.Direction);
        string username = (string)DicTool.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.Username);
        Vector3Data vector3Data = new Vector3Data() { x = x, y = y, z = z, direction = direction };
        
        //处理事件：
        playerManager.OnNewPlayerEvent(username, vector3Data);
    }
}
