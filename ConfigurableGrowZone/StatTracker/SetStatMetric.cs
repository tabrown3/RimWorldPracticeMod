using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public abstract class SetStatMetric : StatMetric
    {
        protected readonly IAggregator<float> aggregator;
        public SetStatMetric(string key, string name, IPullable<float> source, string unit, TimeDomain domain, IAggregator<float> aggregator = null) : base(key, name, source, unit, domain)
        {
            if (aggregator == null)
            {
                this.aggregator = new AverageAggregator();
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
