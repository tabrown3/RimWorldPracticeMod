using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public abstract class SetStatMetric : StatMetric
    {
        protected readonly Func<IEnumerable<float>, float> aggregator;
        public SetStatMetric(string key, string name, Func<float> metricValueFunc, string unit, TimeDomain domain, Func<IEnumerable<float>, float> aggregator = null) : base(key, name, metricValueFunc, unit, domain)
        {
            if (aggregator == null)
            {
                this.aggregator = u => u.Average();
            }
            else
            {
                this.aggregator = aggregator;
            }
        }

        public abstract List<float> GetInternalState();
        public abstract void SetInternalState(List<float> state);
    }
}
