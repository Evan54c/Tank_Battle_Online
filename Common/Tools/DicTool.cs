using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Common.Tools
{
    public class DicTool
    {
        public static T2 GetValue<T1,T2>(Dictionary<T1,T2> dic, T1 key)
        {
            T2 value;
            bool isSuccess = dic.TryGetValue(key, out value);
            if(isSuccess)
            {
                return value;
            }
            else
            {
                return default(T2);
            }
        }
    }
}
