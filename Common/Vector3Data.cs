using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public class Vector3Data
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public DirectiontCode direction { get; set; }
    }
}
