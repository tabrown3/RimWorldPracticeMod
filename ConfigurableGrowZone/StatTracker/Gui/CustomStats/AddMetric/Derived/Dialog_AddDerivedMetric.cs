using RimWorld;
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
    public class Dialog_AddDerivedMetric : Dialog_AddMetric
    {
        private readonly AddDerivedMetricForm form = new AddDerivedMetricForm();
        private Subject<Tuple<CompStatTracker, AddDerivedMetricForm>> onSubmit { get; } = new Subject<Tuple<CompStatTracker, AddDerivedMetricForm>>();

        private readonly AddOperatorListComponent addOperatorListComponent;

        public IObservable<Tuple<CompStatTracker, AddDerivedMetricForm>> OnSubmit => onSubmit;

        public Dialog_AddDerivedMetric(CompStatTracker tracker, List<SourceMetric> allSourceMetrics, List<Type> allOperatorTypes) : base(tracker)
        {
            addOperatorListComponent = new AddOperatorListComponent(allSourceMetrics, allOperatorTypes, form.OperatorList);
        }

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            new RectStacker(inRect).Then(u => DrawTextEntry(u, "Name", form.Name, v => form.Name = v))
                .ThenGap(15f)
                .Then(u => DrawTextEntry(u, "Key", form.Key, v => form.Key = v))
                .ThenGap(15f)
                .Then(u => DrawTextButton(u, "Source", tracker.Data.SourceMetrics, form.AnchorMetric, v => form.AnchorMetric = v))
                .Then(u => addOperatorListComponent.Draw(u))
                .Then(u => DrawSubmitButton(u, form, onSubmit));
        }

        private RectConnector DrawTextButton(Rect inRect, string label, List<SourceMetric> metricList, SourceMetric selectedMetric, Action<SourceMetric> metricCb)
        {
            return StatWidgets.DrawTextButtonSideLabel(inRect, label, metricList, u => u.Name, selectedMetric, metricCb);
        }
    }
}
