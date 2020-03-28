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

        protected Rect DrawTextButton(Rect inRect, string label, List<Type> typeList, Type selectedType, Action<Type> typeCb)
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

            if (selectedType != null)
            {
                Rect typeNameRect = new Rect(outerRect);
                typeNameRect.x = textButtonRect.x + textButtonRect.width;
                typeNameRect.width = Text.CalcSize(selectedType.Name).x;
                Widgets.Label(typeNameRect, selectedType.Name);
            }

            return outerRect;
        }
    }
}
