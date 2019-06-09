using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Common;
using Common.Tools;

public class PhotonEngine : MonoBehaviour ,IPhotonPeerListener{
    public static PhotonPeer Peer
    {
        get
        {
            return peer;
        }
    }
    public static PhotonEngine Instance;
    private static PhotonPeer peer;

    private Dictionary<OperationCode, Request> RequestDict = new Dictionary<OperationCode, Request>();
    private Dictionary<EventCode, BaseEvent> EventDict = new Dictionary<EventCode, BaseEvent>();

    public static string username;

    //设置单例模式，保证只有一个PhotonEngine
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    // Use this for initialization
    void Start () {
        //通过Listender接受服务器端的响应
        Debug.Log("Wait to Connect");
        peer = new PhotonPeer(this,ConnectionProtocol.Udp);
        peer.Connect("47.101.223.4:5055", "MyGame1");   //47.101.223.4   
    }
	
	// Update is called once per frame
	void Update () {
        //更新的时候维持服务
        peer.Service();  
	}

    void OnDestory()
    {
        if(peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    //服务器端直接向客户端发送请求
    public void OnEvent(EventData eventData)
    {
        EventCode code = (EventCode)eventData.Code;
        BaseEvent e = DicTool.GetValue<EventCode, BaseEvent>(EventDict, code);
        e.OnEvent(eventData);
    }

    //服务器端对客户端的响应
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Debug.Log("接收到服务器端的响应！");
        OperationCode opCode =(OperationCode) operationResponse.OperationCode;
        Request request = null;
        bool temp = RequestDict.TryGetValue(opCode, out request);
        if(temp)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.Log("未找到对应的响应处理对象！");
        }
    }
    //服务器和客户端之间连接状态发生改变
    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(statusCode);
    }

    //向请求字典里添加请求
    public void AddRequest(Request request)
    {
        RequestDict.Add(request.OpCode, request);
    }
    //向请求字典里移除请求
    public void RemoveRequest(Request request)
    {
        RequestDict.Remove(request.OpCode);
    }
    //向事件字典里添加事件
    public void AddEvent(BaseEvent e)
    {
        EventDict.Add(e.eventCode, e);
    }
    //向事件字典里移除事件
    public void RemoveEvent(BaseEvent e)
    {
        EventDict.Remove(e.eventCode);
    }


}