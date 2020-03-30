using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class AddOperatorRowComponent : IValidatable
    {
        private readonly List<Type> allOperatorTypes;
        private readonly List<string> allTrackerNames;
        private readonly List<SourceMetric> allSourceMetrics;

        private Type chosenOperator = null;
        private string chosenTrackerName = "";
        private SourceMetric chosenSourceMetric = null;
        private bool chosenOperatorIsBinary = false;

        private List<SourceMetric> availableMetrics = new List<SourceMetric>();
        public AddOperatorRowComponent(List<Type> allOperatorTypes, List<string> allTrackerNames, List<SourceMetric> allSourceMetrics)
        {
            this.allOperatorTypes = allOperatorTypes;
            this.allTrackerNames = allTrackerNames;
            this.allSourceMetrics = allSourceMetrics;
        }

        public Rect Draw(Rect inRect)
        {
            return new RectSpanner(inRect)
                .Then(
                    u => StatWidgets.DrawTextButton(u, "Operator", allOperatorTypes, v => v.Name, chosenOperator,
                        v => {
                            chosenOperator = v;
                            chosenTrackerName = "";
                            chosenSourceMetric = null;
                            chosenOperatorIsBinary = typeof(BinaryOperator<float>).IsAssignableFrom(chosenOperator);
                        })
                )
                .IfThen(
                    () => chosenOperator != null && chosenOperatorIsBinary,
                    u => StatWidgets.DrawTextButton(u, "Tracker", allTrackerNames, v => v, chosenTrackerName,
                        v => {
                            chosenTrackerName = v;
                            chosenSourceMetric = null;
                            availableMetrics = allSourceMetrics.Where(w => w.ParentName == v).ToList();
                        })
                )
                .IfThen(
                    () => !string.IsNullOrEmpty(chosenTrackerName),
                    u => StatWidgets.DrawTextButton(u, "Metric", availableMetrics, v => v.Name, chosenSourceMetric, v => chosenSourceMetric = v)
                ).GetRect();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
