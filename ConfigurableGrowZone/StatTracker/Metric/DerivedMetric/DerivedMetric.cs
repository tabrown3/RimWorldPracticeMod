using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class DerivedMetric<T> : IMetric
    {
        public string Key { get; }
        public string Name { get; }
        public string Unit { get; }
        public TimeDomain Domain { get; }

        public List<SourceMetric> Sources = new List<SourceMetric>();
        public List<IOperator<T>> Operators = new List<IOperator<T>>();
    }
}
