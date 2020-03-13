using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class PollStatMetric : StatMetric
    {
        public PollStatMetric(string key, string name, Func<float> metricValueFunc, string unit, GameTime.InTicks resolution = GameTime.InTicks.Hour) : base(key, name, metricValueFunc, unit, resolution)
        {
        }

        public override void Tick(int gameTick)
        {
            if(ShouldPushValue(gameTick))
            {
                PushValue(gameTick, metricValueFunc());
            }
        }
    }
}
