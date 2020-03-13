using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public abstract class SetStatMetric : StatMetric
    {
        protected readonly Func<IEnumerable<float>, float> aggregator;
        public SetStatMetric(string key, string name, Func<float> metricValueFunc, string unit, GameTime.InTicks resolution = GameTime.InTicks.Hour, Func<IEnumerable<float>, float> aggregator = null) : base(key, name, metricValueFunc, unit, resolution)
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
    }
}
