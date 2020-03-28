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
        public IObservable<Tuple<CompStatTracker, AddDerivedMetricForm>> OnSubmit => onSubmit;

        public Dialog_AddDerivedMetric(CompStatTracker tracker) : base(tracker)
        {
            
        }

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            new RectStacker(inRect).Then(u => DrawTextEntry(u, "Name", form.Name, v => form.Name = v))
                .ThenGap(15f)
                .Then(u => DrawTextEntry(u, "Key", form.Key, v => form.Key = v))
                .ThenGap(15f);
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
