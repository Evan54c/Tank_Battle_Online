using Common;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour {

    //事件的父类，抽象类，定义了EventCode和响应回应的方法
    public EventCode eventCode;
    public abstract void OnEvent(EventData eventData);  //响应回应的方法

    public virtual void Start()
    {
        PhotonEngine.Instance.AddEvent(this);
    }
    public void OnDestroy()
    {
        PhotonEngine.Instance.RemoveEvent(this);
    }
}
