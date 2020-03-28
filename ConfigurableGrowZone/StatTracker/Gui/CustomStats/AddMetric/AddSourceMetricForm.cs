using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class AddSourceMetricForm
    {
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public Type MetricType { get; set; }
        public Type DomainType { get; set; }
        public Type SourceType { get; set; }
        public Type AggregatorType { get; set; }

        public bool IsValid()
        {
            bool requiredFieldsValid = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Key) && MetricType != null && DomainType != null && SourceType != null;
            bool needsAggregatorType = StatTypesHelper.IsSetMetric(MetricType); // only children of SetSourceMetric need an aggregator
            bool hasAggregatorType = AggregatorType != null;

            if (needsAggregatorType)
            {
                return requiredFieldsValid && hasAggregatorType;
            }
            else
            {
                return requiredFieldsValid;
            }
        }
    }
}
