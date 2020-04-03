using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class TrackerDomain
    {
        public string TrackerName { get; }
        public TimeDomain Domain { get; }

        public TrackerDomain(string trackerName, TimeDomain domain)
        {
            TrackerName = trackerName;
            Domain = domain;
        }
    }

    public class TrackerDomainEqualityComparer : IEqualityComparer<TrackerDomain>
    {
        public bool Equals(TrackerDomain x, TrackerDomain y)
        {
            return x?.TrackerName == y?.TrackerName && x?.Domain?.Resolution == y?.Domain?.Resolution;
        }

        public int GetHashCode(TrackerDomain obj)
        {
            return obj.GetHashCode();
        }
    }
}
