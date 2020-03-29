using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class AddDerivedMetricForm : IValidatable
    {
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public SourceMetric AnchorMetric { get; set; }
        public List<SourceMetric> Inputs { get; } = new List<SourceMetric>();
        public IEnumerable<SourceMetric> SourceMetrics
        {
            get
            {
                yield return AnchorMetric;
                foreach (var metric in Inputs)
                    yield return metric;
            }
        }
        public List<IOperator<float>> Operators { get; set; }

        public bool IsValid()
        {
            return true;
        }
    }
}
