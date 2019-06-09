using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public class PlayerData
    {
        public Vector3Data Pos { get; set; }
        public string Username { get; set; }
    }
}
