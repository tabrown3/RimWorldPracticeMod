using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class SumAggregator : IAggregator<float>
    {
        public Func<IEnumerable<float>, float> Aggregate { get; } = u => u.Sum();
    }
}
