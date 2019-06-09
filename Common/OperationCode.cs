using System;
using System.Collections.Generic;
using System.Text;


namespace Common
{
    public enum  OperationCode:byte //区分请求和响应的类型
    {
        Login,    //登录
        Registe,  //注册
        Default,  //默认
        Action,   //每帧操作
        Getin,    //进入游戏或重连
        Room,      //进入房间
        Recover,       //复活请求
        Exit       //客户端退出
    }
}
