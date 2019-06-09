using System;
using System.Collections.Generic;
using System.Text;


namespace Common
{
    public enum ParameterCode:byte //区分传送数据时的参数类型
    {
        Username,           //用户名
        Password,           //密码
        X,Y,Z,Direction,   //方位信息
        UsernameList,
        PlayerDataList,   //登入时同步其他玩家
        ActQueue,        //动作
        ActionData,
        RoomID
    }
}
