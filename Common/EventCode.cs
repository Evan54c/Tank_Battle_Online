using System;
using System.Collections.Generic;
using System.Text;


namespace Common
{
    public enum EventCode:byte //区分服务器端向客户端发送的事件的类型
    {
        NewPlayer,     //新加入玩家
        SyncAction,     //同步动作
        Clear,          //清缓存，要位置
        Room,         //新玩家加入房间
        Recover       //玩家复活
    } 
}
