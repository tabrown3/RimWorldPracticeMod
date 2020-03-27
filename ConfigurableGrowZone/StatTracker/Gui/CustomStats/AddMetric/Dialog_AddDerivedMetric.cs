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
            Rect nameEntryRect = new Rect(inRect);
            nameEntryRect.height = textInputHeight;
            nameEntryRect.width /= 2;
            name = Widgets.TextEntryLabeled(nameEntryRect, "Name", name);
            var curY = nameEntryRect.height;

            Rect keyEntryRect = new Rect(inRect);
            keyEntryRect.y = curY;
            keyEntryRect.height = textInputHeight;
            keyEntryRect.width /= 2;
            key = Widgets.TextEntryLabeled(keyEntryRect, "Key", key);
            curY += textInputHeight;

        }
    }
}
