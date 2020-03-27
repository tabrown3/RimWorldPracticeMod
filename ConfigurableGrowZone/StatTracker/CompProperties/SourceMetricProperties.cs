using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class SourceMetricProperties
    {
        //private string metricType;
        //public Type MetricType {
        //    get
        //    {
        //        return Type.GetType(metricType);
        //    }
        //    set
        //    {
        //        metricType = value.FullName;
        //    }
        //}
        public string MetricType { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string DomainType { get; set; }
        public string SourceType { get; set; }
        public string AggregatorType { get; set; }
    }
}
