using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public abstract class SetSourceMetric : SourceMetric
    {
        protected readonly IAggregator<float> aggregator;
        public SetSourceMetric(string parentName, string key, string name, IPullable<float> source, string unit, TimeDomain domain, IAggregator<float> aggregator = null) : base(parentName, key, name, source, unit, domain)
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
