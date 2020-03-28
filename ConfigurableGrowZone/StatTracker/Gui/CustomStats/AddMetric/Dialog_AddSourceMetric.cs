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
    public class Dialog_AddSourceMetric : Window
    {
        private readonly AddSourceMetricForm form = new AddSourceMetricForm();

        private readonly CompStatTracker tracker;
        private readonly List<Type> domains;
        private readonly List<Type> sources;
        private readonly List<Type> aggregators;

        private readonly Subject<Tuple<CompStatTracker, AddSourceMetricForm>> onSubmit = new Subject<Tuple<CompStatTracker, AddSourceMetricForm>>();
        public IObservable<Tuple<CompStatTracker, AddSourceMetricForm>> OnSubmit => onSubmit;

        public Dialog_AddSourceMetric(CompStatTracker tracker, List<Type> domains, List<Type> sources, List<Type> aggregators)
        {
            this.tracker = tracker;
            this.domains = domains;
            this.sources = sources;
            this.aggregators = aggregators;

            this.closeOnClickedOutside = true;
            this.doCloseX = true;
            this.focusWhenOpened = true;
            this.absorbInputAroundWindow = true;
            this.forcePause = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            new RectStacker(inRect)
                .Then(u => DrawTextEntry(u, "Name", form.Name, v => form.Name = v))
                .ThenGap(15f)
                .Then(u => DrawTextEntry(u, "Key", form.Key, v => form.Key = v))
                .ThenGap(15f)
                .Then(u => DrawTextEntry(u, "Unit", form.Unit, v => form.Unit = v))
                .ThenGap(15f)
                .Then(u =>
                {
                    return new RectStacker(u)
                        .Then(v => DrawRadioButton(v, "Poll", typeof(PollSourceMetric)))
                        .ThenGap(10f)
                        .Then(v => DrawRadioButton(v, "Digest", typeof(DigestSourceMetric)))
                        .ThenGap(10f)
                        .Then(v => DrawRadioButton(v, "Window", typeof(WindowSourceMetric)));
                })
                .ThenGap(15f)
                .Then(u => DrawTextButton(u, "Domain", domains, form.DomainType, v => form.DomainType = v))
                .Then(u => DrawTextButton(u, "Source", sources, form.SourceType, v => form.SourceType = v))
                .IfThen(
                    () => StatTypesHelper.IsSetMetric(form.MetricType),
                    u => DrawTextButton(u, "Aggregator", aggregators, form.AggregatorType, v => form.AggregatorType = v)
                )
                .Then(u => DrawSubmitButton(u));
        }

        private Rect DrawTextEntry(Rect inRect, string label, string inVal, Action<string> outValCb)
        {
            Rect nameEntryRect = new Rect(inRect);
            nameEntryRect.height = 30f;
            nameEntryRect.width = 200f;
            outValCb(Widgets.TextEntryLabeled(nameEntryRect, label, inVal));

            return nameEntryRect;
        }

        private Rect DrawRadioButton(Rect inRect, string label, Type metricType)
        {
            Rect radioButtonRect = new Rect(inRect);
            radioButtonRect.width = 100f;
            radioButtonRect.height = 20f;
            if (Widgets.RadioButtonLabeled(radioButtonRect, label, form.MetricType == metricType))
            {
                form.MetricType = metricType;
            }

            return radioButtonRect;
        }

        private Rect DrawTextButton(Rect inRect, string label, List<Type> typeList, Type selectedType, Action<Type> typeCb)
        {
            Rect outerRect = new Rect(inRect);
            outerRect.height = 35f;

            Rect textButtonRect = new Rect(outerRect);
            textButtonRect.width = 100f;
            
            if (Widgets.ButtonText(textButtonRect, label))
            {
                List<FloatMenuOption> list = typeList.Select(v => new FloatMenuOption(v.Name, () => typeCb(v))).ToList();

                Find.WindowStack.Add(new FloatMenu(list));
            }

            if(selectedType != null)
            {
                Rect typeNameRect = new Rect(outerRect);
                typeNameRect.x = textButtonRect.x + textButtonRect.width;
                typeNameRect.width = Text.CalcSize(selectedType.Name).x;
                Widgets.Label(typeNameRect, selectedType.Name);
            }

            return outerRect;
        }

        private Rect DrawSubmitButton(Rect inRect)
        {
            Rect submitButtonRect = new Rect(inRect);
            submitButtonRect.width = 150f;
            submitButtonRect.height = 40f;
            
            if (Widgets.ButtonText(submitButtonRect, "Submit") && form.IsValid())
            {
                onSubmit.OnNext(Tuple.Create(tracker, form));
                onSubmit.OnCompleted();
                Close();
            }

            return submitButtonRect;
        }
    }
}
