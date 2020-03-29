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
    public class Dialog_AddMetric : Window
    {
        protected readonly CompStatTracker tracker;

        public Dialog_AddMetric(CompStatTracker tracker)
        {
            this.tracker = tracker;

            this.closeOnClickedOutside = true;
            this.doCloseX = true;
            this.focusWhenOpened = true;
            this.absorbInputAroundWindow = true;
            this.forcePause = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            
        }

        protected Rect DrawTextEntry(Rect inRect, string label, string inVal, Action<string> outValCb)
        {
            Rect nameEntryRect = new Rect(inRect);
            nameEntryRect.height = 30f;
            nameEntryRect.width = 200f;
            outValCb(Widgets.TextEntryLabeled(nameEntryRect, label, inVal));

            return nameEntryRect;
        }

        protected Rect DrawTextButton<T>(Rect inRect, string label, List<T> objectList, Func<T,string> labelFunc, T selectedObject, Action<T> objectCb)
        {
            Rect outerRect = new Rect(inRect);
            outerRect.height = 35f;

            Rect textButtonRect = new Rect(outerRect);
            textButtonRect.width = 100f;

            if (Widgets.ButtonText(textButtonRect, label))
            {
                List<FloatMenuOption> list = objectList.Select(v => new FloatMenuOption(labelFunc(v), () => objectCb(v))).ToList();

                Find.WindowStack.Add(new FloatMenu(list));
            }

            if (selectedObject != null)
            {
                string selectedObjectLabel = labelFunc(selectedObject);

                Rect typeNameRect = new Rect(outerRect);
                typeNameRect.x = textButtonRect.x + textButtonRect.width;
                typeNameRect.width = Text.CalcSize(selectedObjectLabel).x;
                Widgets.Label(typeNameRect, selectedObjectLabel);
            }

            return outerRect;
        }

        protected Rect DrawSubmitButton<T>(Rect inRect, T form, Subject<Tuple<CompStatTracker, T>> onSubmit) where T : IValidatable
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
