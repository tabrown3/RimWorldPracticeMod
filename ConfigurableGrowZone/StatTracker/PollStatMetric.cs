using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class PollStatMetric : StatMetric
    {
        public PollStatMetric(string key, string name, Func<float> metricValueFunc, string unit, TimeDomain domain) : base(key, name, metricValueFunc, unit, domain)
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
