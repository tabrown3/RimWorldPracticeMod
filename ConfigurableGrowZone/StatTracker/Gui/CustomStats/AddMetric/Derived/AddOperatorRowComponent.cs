using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class AddOperatorRowComponent : IValidatable
    {
        private readonly List<Type> allOperatorTypes;
        private readonly List<string> allTrackerNames;
        private readonly List<SourceMetric> allSourceMetrics;
        private List<SourceMetric> availableMetrics = new List<SourceMetric>();

        private Type chosenOperator = null;
        private string chosenTrackerName = "";
        private SourceMetric chosenSourceMetric = null;

        private bool chosenOperatorIsBinary = false;

        private readonly Subject<Type> operatorChosen = new Subject<Type>();
        private readonly Subject<string> trackerNameChosen = new Subject<string>();
        private readonly Subject<SourceMetric> sourceMetricChosen = new Subject<SourceMetric>();

        public readonly IObservable<bool> RowBecameValid;

        public AddOperatorRowComponent(List<Type> allOperatorTypes, List<string> allTrackerNames, List<SourceMetric> allSourceMetrics)
        {
            this.allOperatorTypes = allOperatorTypes;
            this.allTrackerNames = allTrackerNames;
            this.allSourceMetrics = allSourceMetrics;

            RowBecameValid = RowBecameValidFactory();
        }

        public Rect Draw(Rect inRect)
        {
            return new RectSpanner(inRect)
                .Then(
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Operator", allOperatorTypes, v => v.Name, chosenOperator, OperatorChosen)
                )
                .ThenGap(50f)
                .IfThen(
                    () => chosenOperator != null && chosenOperatorIsBinary,
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Tracker", allTrackerNames, v => v, chosenTrackerName, TrackerNameChosen)
                )
                .ThenGap(50f)
                .IfThen(
                    () => !string.IsNullOrEmpty(chosenTrackerName),
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Metric", availableMetrics, v => v.Name, chosenSourceMetric, SourceMetricChosen)
                ).GetRect();
        }

        public bool IsValid()
        {
            bool hasOperator = chosenOperator != null;

            bool isValid;
            if (hasOperator && chosenOperatorIsBinary)
            {
                isValid = !string.IsNullOrEmpty(chosenTrackerName) && chosenSourceMetric != null;
            }
            else
            {
                isValid = hasOperator;
            }

            return isValid;
        }

        private void OperatorChosen(Type chosenOperator)
        {
            this.chosenOperator = chosenOperator;
            chosenTrackerName = "";
            chosenSourceMetric = null;
            chosenOperatorIsBinary = !StatTypesHelper.IsUnaryOperator(chosenOperator);
            operatorChosen.OnNext(chosenOperator);
        }

        private void TrackerNameChosen(string chosenTrackerName)
        {
            this.chosenTrackerName = chosenTrackerName;
            chosenSourceMetric = null;
            availableMetrics = allSourceMetrics.Where(w => w.ParentName == chosenTrackerName).ToList();
            trackerNameChosen.OnNext(chosenTrackerName);
        }

        private void SourceMetricChosen(SourceMetric chosenSourceMetric)
        {
            this.chosenSourceMetric = chosenSourceMetric;
            sourceMetricChosen.OnNext(chosenSourceMetric);
        }

        private IObservable<bool> RowBecameValidFactory()
        {
            bool lastValidityState = false;

            // observable that emits when rows's validity changes from invalid to valid
            return Observable.Merge(
                    operatorChosen.Select(u => IsValid()),
                    trackerNameChosen.Select(u => IsValid()),
                    sourceMetricChosen.Select(u => IsValid())
                )
                .Where(curValidityState =>
                    {
                        bool validityStateChanged = curValidityState != lastValidityState;
                        lastValidityState = curValidityState;
                        return validityStateChanged && curValidityState;
                    }
                );
        }
    }
}
