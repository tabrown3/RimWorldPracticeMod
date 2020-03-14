using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public abstract class TimeDomain : Domain
    {
        public GameTime.InTicks Resolution { get; }
        public int ResInTicks { get; }

        public TimeDomain(GameTime.InTicks resolution)
        {
            Resolution = resolution;
            ResInTicks = (int)Resolution;
        }

        public override bool IsResolutionBoundary(int domainElement)
        {
            return domainElement % ResInTicks == ResInTicks - 1;
        }
    }
}
