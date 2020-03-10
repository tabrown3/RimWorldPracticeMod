using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class DataPoint
    {
        public string Key { get; set; }
        public GameTime.InTicks Resolution { get; set; }
        public int TimeStamp { get; set; }
        public float DigestValue { get; set; }
    }
}
