using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public enum Actions : byte
    {
        Upward ,
        Downward,
        Toleft,
        Toright,
        Fire,
        stay
    }
}
