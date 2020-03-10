using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class GameTime
    {
        public enum InTicks
        {
            Tick = 1,
            QuarterHour = 625,
            HalfHour = 1250,
            Hour = 2500,
            Day = 60000
        }
    }
}
