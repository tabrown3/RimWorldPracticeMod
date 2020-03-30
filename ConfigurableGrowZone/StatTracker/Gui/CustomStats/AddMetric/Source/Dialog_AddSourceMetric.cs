﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class Dialog_AddSourceMetric : Dialog_AddMetric
    {
        private readonly AddSourceMetricForm form = new AddSourceMetricForm();

        private readonly List<Type> domains;
        private readonly List<Type> sources;
        private readonly List<Type> aggregators;

        private Subject<Tuple<CompStatTracker, AddSourceMetricForm>> onSubmit { get; } = new Subject<Tuple<CompStatTracker, AddSourceMetricForm>>();
        public IObservable<Tuple<CompStatTracker, AddSourceMetricForm>> OnSubmit => onSubmit;

        public Dialog_AddSourceMetric(CompStatTracker tracker, List<Type> domains, List<Type> sources, List<Type> aggregators) : base(tracker)
        {
            this.domains = domains;
            this.sources = sources;
            this.aggregators = aggregators;
        }

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            new RectStacker(inRect)
                .Then<RectStacker>(u => DrawTextEntry(u, "Name", form.Name, v => form.Name = v))
                .ThenGap<RectStacker>(15f)
                .Then<RectStacker>(u => DrawTextEntry(u, "Key", form.Key, v => form.Key = v))
                .ThenGap<RectStacker>(15f)
                .Then<RectStacker>(u => DrawTextEntry(u, "Unit", form.Unit, v => form.Unit = v))
                .ThenGap<RectStacker>(15f)
                .Then<RectStacker>(u =>
                {
                    return new RectStacker(u)
                        .Then<RectStacker>(v => DrawRadioButton(v, "Poll", typeof(PollSourceMetric)))
                        .ThenGap<RectStacker>(10f)
                        .Then<RectStacker>(v => DrawRadioButton(v, "Digest", typeof(DigestSourceMetric)))
                        .ThenGap<RectStacker>(10f)
                        .Then<RectStacker>(v => DrawRadioButton(v, "Window", typeof(WindowSourceMetric)));
                })
                .ThenGap<RectStacker>(15f)
                .Then<RectStacker>(u => DrawTextButton(u, "Domain", domains, form.DomainType, v => form.DomainType = v))
                .Then<RectStacker>(u => DrawTextButton(u, "Source", sources, form.SourceType, v => form.SourceType = v))
                .IfThen<RectStacker>(
                    () => StatTypesHelper.IsSetMetric(form.MetricType),
                    u => DrawTextButton(u, "Aggregator", aggregators, form.AggregatorType, v => form.AggregatorType = v)
                )
                .Then<RectStacker>(u => DrawSubmitButton(u, form, onSubmit));
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
            return StatWidgets.DrawTextButton(inRect, label, typeList, u => u.Name, selectedType, typeCb);
        }
    }
}
