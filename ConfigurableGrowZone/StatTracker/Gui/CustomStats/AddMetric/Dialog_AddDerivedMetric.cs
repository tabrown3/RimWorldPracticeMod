using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class Dialog_AddDerivedMetric : Window
    {
        private string name = "";
        private string key = "";

        public Dialog_AddDerivedMetric()
        {
            this.closeOnClickedOutside = true;
            this.doCloseX = true;
            this.focusWhenOpened = true;
            this.absorbInputAroundWindow = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            float textInputHeight = 30f;

            new RectStacker(inRect).Then(u =>
            {
                Rect nameEntryRect = new Rect(u);
                nameEntryRect.height = textInputHeight;
                nameEntryRect.width = 400f;
                name = Widgets.TextEntryLabeled(nameEntryRect, "Name", name);

                return nameEntryRect;
            })
            .ThenGap(15f)
            .Then(u =>
            {
                Rect keyEntryRect = new Rect(u);
                keyEntryRect.height = textInputHeight;
                keyEntryRect.width = 400f;
                key = Widgets.TextEntryLabeled(keyEntryRect, "Key", key);

                return keyEntryRect;
            });
        }
    }
}
