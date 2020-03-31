using ConfigurableGrowZone.StatTracker.Gui.CustomStats.AddMetric.Derived;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ConfigurableGrowZone
{
    public class AddDerivedMetricForm : IValidatable
    {
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public SourceMetric AnchorMetric { get; set; }
        public IEnumerable<SourceMetric> SourceMetrics
        {
            get
            {
                yield return AnchorMetric;
                foreach (var metric in Inputs)
                    yield return metric;
            }
        }

        public List<Type> Operators
        {
            get
            {
                var operators = OperatorList.Rows.Select(u => u.ChosenOperator).Where(u => u != null).ToList();

                return operators;
            }
        }

        public List<SourceMetric> Inputs
        {
            get
            {
                var sourceMetrics = OperatorList.Rows.Select(u => u.ChosenSourceMetric).Where(u => u != null).ToList();

                return sourceMetrics;
            }
        }

        public AddOperatorListModel OperatorList { get; } = new AddOperatorListModel();

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Key) && AnchorMetric != null && OperatorList.IsValid();
        }
    }
}
