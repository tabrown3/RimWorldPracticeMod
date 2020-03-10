using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class Dialog_PowerStatTracker : Window
    {
        private Dictionary<int, DataPoint> history;

        public Dialog_PowerStatTracker(Dictionary<int, DataPoint> history)
        {
            this.history = history;

            doCloseX = true;
            closeOnClickedOutside = true;
            absorbInputAroundWindow = true;
            draggable = true;
            forcePause = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            Rect outRect = new Rect(inRect);
            outRect.yMin += 20f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;

            float curY = 0f;

            //throw new NotImplementedException();
        }
    }
}
