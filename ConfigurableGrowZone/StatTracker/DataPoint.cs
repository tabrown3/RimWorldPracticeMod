using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ConfigurableGrowZone
{
    public class DataPoint : IExposable
    {
        public DataPoint()
        {

        }

        public DataPoint(int timeStampGameTicks, float value)
        {
            TimeStampGameTicks = timeStampGameTicks;
            Value = value;
        }
        private int timeStampGameTicks;
        public int TimeStampGameTicks { get { return timeStampGameTicks; } set { timeStampGameTicks = value; } }
        private float value;
        public float Value { get { return value; } set { this.value = value; } }

        public void ExposeData()
        {
            Scribe_Values.Look(ref timeStampGameTicks, "DataPoint.timeStampGameTicks");
            Scribe_Values.Look(ref value, "DataPoint.value");

        }
    }
}
