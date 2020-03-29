using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class PollSourceMetric : SourceMetric
    {
        public PollSourceMetric(string parentName, string key, string name, IPullable<float> source, string unit, TimeDomain domain) : base(parentName, key, name, source, unit, domain)
        {
        }

        public override void Tick(int gameTick)
        {
            if(Domain.IsResolutionBoundary(gameTick))
            {
                PushValue(gameTick, source.PullValue());
            }
        }
    }
}
