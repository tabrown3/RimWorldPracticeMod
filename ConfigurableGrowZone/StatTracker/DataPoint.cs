using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class DataPoint
    {
        public DataPoint(int timeStampGameTicks, float value)
        {
            TimeStampGameTicks = timeStampGameTicks;
            Value = value;
        }
        public int TimeStampGameTicks { get; }
        public float Value { get; }
    }
}
