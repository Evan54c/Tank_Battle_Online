using System.Collections;
using System.Collections.Generic;
using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public class ExitRequest : Request {

    public override void DefaultRequest()
    {
        PhotonEngine.Peer.OpCustom((byte)OpCode, new Dictionary<byte, object>(), true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }

}
