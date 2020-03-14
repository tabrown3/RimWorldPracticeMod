using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public abstract class Domain
    {
        public Func<int, float> DomainFunc { get; protected set; }
        public abstract bool IsResolutionBoundary(int domainElement);
    }
}
