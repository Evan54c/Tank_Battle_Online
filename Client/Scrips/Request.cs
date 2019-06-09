using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ExitGames.Client.Photon;

public abstract class Request:MonoBehaviour{

    //请求的父类，抽象类，定义了OperationCode和发起请求和响应回应的虚方法
    public OperationCode OpCode;
    public abstract void OnOperationResponse(OperationResponse operationResponse);  //响应回应的方法
    public abstract void DefaultRequest(); //发起请求的方法

    public virtual void Start()
    {
        PhotonEngine.Instance.AddRequest(this);
    }
    public void OnDestroy()
    {
        PhotonEngine.Instance.RemoveRequest(this);
    }

}
