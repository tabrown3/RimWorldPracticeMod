using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class PollStatMetric : StatMetric
    {
        public PollStatMetric(string key, string name, IPullable<float> source, string unit, TimeDomain domain) : base(key, name, source, unit, domain)
        {
        }

        public override void Tick(int gameTick)
        {
            if(ShouldPushValue(gameTick))
            {
                PushValue(gameTick, source.PullValue());
            }
        }
    }
}
