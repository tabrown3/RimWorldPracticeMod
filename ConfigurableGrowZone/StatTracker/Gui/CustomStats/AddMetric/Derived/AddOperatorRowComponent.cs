using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Verse;

namespace ConfigurableGrowZone
{
    public class AddOperatorRowComponent
    {
        private readonly AddOperatorOptionsManager optionsManager;

        public readonly AddOperatorRowModel Model;

        private readonly Subject<Type> operatorChosen = new Subject<Type>();
        private readonly Subject<string> trackerNameChosen = new Subject<string>();
        private readonly Subject<SourceMetric> sourceMetricChosen = new Subject<SourceMetric>();

        public readonly IObservable<bool> RowBecameValid;

        public AddOperatorRowComponent(AddOperatorOptionsManager optionsManager, AddOperatorRowModel model)
        {
            RowBecameValid = RowBecameValidFactory();
            Model = model;
            this.optionsManager = optionsManager;

            optionsManager.DomainChanged.Subscribe(OnDomainChanged);
        }

        public RectConnector Draw(Rect inRect)
        {
            return new RectSpanner(inRect)
                .Then(
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Operator", optionsManager.GetAvailableOperatorTypes(), v => v.Name, Model.ChosenOperator, OperatorChosen)
                )
                .ThenGap(50f)
                .IfThen(
                    () => Model.ChosenOperator != null && Model.ChosenOperatorIsBinary,
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Tracker", optionsManager.GetAvailableTrackerNames(), v => v, Model.ChosenTrackerName, TrackerNameChosen)
                )
                .ThenGap(50f)
                .IfThen(
                    () => !string.IsNullOrEmpty(Model.ChosenTrackerName),
                    u => StatWidgets.DrawTextButtonBottomLabel(u, "Metric", optionsManager.GetAvailableSourceMetrics(Model.ChosenTrackerName), v => v.Name, Model.ChosenSourceMetric, SourceMetricChosen)
                );
        }

        public bool IsEmpty()
        {
            return Model.IsEmpty();
        }

        private void OperatorChosen(Type chosenOperator)
        {
            Model.ChosenOperator = chosenOperator;
            Model.ChosenTrackerName = "";
            Model.ChosenSourceMetric = null;
            Model.ChosenOperatorIsBinary = !StatTypesHelper.IsUnaryOperator(chosenOperator);
            operatorChosen.OnNext(chosenOperator);
        }

        private void TrackerNameChosen(string chosenTrackerName)
        {
            Model.ChosenTrackerName = chosenTrackerName;
            Model.ChosenSourceMetric = null;
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

        private void OnDomainChanged(TimeDomain newDomain)
        {
            if (!optionsManager.TrackerHasChildOfCurrentDomain(Model.ChosenTrackerName))
            {
                Model.ChosenTrackerName = "";
            }

            if (!optionsManager.SourceMetricIsOfCurrentDomain(Model.ChosenSourceMetric))
            {
                Model.ChosenSourceMetric = null;
            }
        }
    }
}
