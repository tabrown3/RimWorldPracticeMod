using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class AddOperatorRowComponent
    {
        private readonly List<Type> allOperatorTypes;
        private readonly List<string> allTrackerNames;
        private readonly List<SourceMetric> allSourceMetrics;
        private List<SourceMetric> availableMetrics = new List<SourceMetric>();

        public readonly AddOperatorRowModel Model;// = new AddOperatorRowModel();

        private readonly Subject<Type> operatorChosen = new Subject<Type>();
        private readonly Subject<string> trackerNameChosen = new Subject<string>();
        private readonly Subject<SourceMetric> sourceMetricChosen = new Subject<SourceMetric>();

        public readonly IObservable<bool> RowBecameValid;

        public AddOperatorRowComponent(List<Type> allOperatorTypes, List<string> allTrackerNames, List<SourceMetric> allSourceMetrics, AddOperatorRowModel model)
        {
            this.allOperatorTypes = allOperatorTypes;
            this.allTrackerNames = allTrackerNames;
            this.allSourceMetrics = allSourceMetrics;

            RowBecameValid = RowBecameValidFactory();
            Model = model;
        }

        public Rect Draw(Rect inRect)
        {
            return new RectSpanner(inRect)
                .Then(
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Operator", allOperatorTypes, v => v.Name, Model.ChosenOperator, OperatorChosen)
                )
                .ThenGap(50f)
                .IfThen(
                    () => Model.ChosenOperator != null && Model.ChosenOperatorIsBinary,
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Tracker", allTrackerNames, v => v, Model.ChosenTrackerName, TrackerNameChosen)
                )
                .ThenGap(50f)
                .IfThen(
                    () => !string.IsNullOrEmpty(Model.ChosenTrackerName),
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Metric", availableMetrics, v => v.Name, Model.ChosenSourceMetric, SourceMetricChosen)
                ).GetRect();
        }

        public bool IsEmpty()
        {
            return Model.IsEmpty();
        }

        private void OperatorChosen(Type chosenOperator)
        {
            Model.ChosenOperator = chosenOperator;
            Log.Message($"Chose: {chosenOperator.Name}");
            Model.ChosenTrackerName = "";
            Model.ChosenSourceMetric = null;
            Model.ChosenOperatorIsBinary = !StatTypesHelper.IsUnaryOperator(chosenOperator);
            operatorChosen.OnNext(chosenOperator);
        }

        private void TrackerNameChosen(string chosenTrackerName)
        {
            Model.ChosenTrackerName = chosenTrackerName;
            Model.ChosenSourceMetric = null;
            availableMetrics = allSourceMetrics.Where(w => w.ParentName == chosenTrackerName).ToList();
            trackerNameChosen.OnNext(chosenTrackerName);
        }

        private void SourceMetricChosen(SourceMetric chosenSourceMetric)
        {
            Model.ChosenSourceMetric = chosenSourceMetric;
            sourceMetricChosen.OnNext(chosenSourceMetric);
        }

        private IObservable<bool> RowBecameValidFactory()
        {
            bool lastValidityState = false;

            // observable that emits when rows's validity changes from invalid to valid
            return Observable.Merge(
                    operatorChosen.Select(u => Model.IsValid()),
                    trackerNameChosen.Select(u => Model.IsValid()),
                    sourceMetricChosen.Select(u => Model.IsValid())
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
