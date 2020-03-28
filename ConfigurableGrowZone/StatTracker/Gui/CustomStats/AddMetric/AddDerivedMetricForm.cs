using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class AddDerivedMetricForm
    {
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public List<SourceMetric> SourceMetrics { get; set; }
        public List<IOperator<float>> Operators { get; set; }

        public bool IsValid()
        {
            return true;
        }
    }
}
