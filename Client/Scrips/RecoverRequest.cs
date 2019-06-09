using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Common.Tools;
using Common;
using System.Xml.Serialization;
using System.IO;

public class RecoverRequest : Request
{
    [HideInInspector]
    public Vector3 pos;
    [HideInInspector]
    public DirectiontCode direction;
    [HideInInspector]
    public string username;

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
        throw new System.NotImplementedException();
    }

}
