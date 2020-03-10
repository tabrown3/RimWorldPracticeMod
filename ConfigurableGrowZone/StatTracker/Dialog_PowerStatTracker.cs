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

        float barWidth = 10f;

        public Dialog_PowerStatTracker(Dictionary<int, DataPoint> history)
        {
            this.history = history;

            doCloseX = true;
            closeOnClickedOutside = true;
            absorbInputAroundWindow = true;
            draggable = true;
            forcePause = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            Rect outRect = new Rect(inRect);
            //outRect.yMin += 20f;
            //outRect.yMax -= 40f;
            outRect.width -= 16f;

            List<DataPoint> historyList = history.Select(u => u.Value).OrderByDescending(u => u.TimeStamp).ToList();

            int barCount = Mathf.Min(Mathf.FloorToInt(outRect.width / barWidth), historyList.Count);
            float maxValue = historyList.Max(u => u.DigestValue);
            float minValue = historyList.Min(u => u.DigestValue);

            float roofValue = Mathf.Max(maxValue, 0);
            float basementValue = Mathf.Min(minValue, 0);

            float scaleDivisor = Mathf.Max(roofValue, Mathf.Abs(basementValue));

            for (int i = 0; i < barCount; i++)
            {
                DataPoint point = historyList[i];

                float xPos = outRect.width - (i + 1) * barWidth;

                float barHeight;
                if (scaleDivisor != 0) // wouldn't want to divide by zero, would we...
                {
                    barHeight = (point.DigestValue / scaleDivisor) * outRect.height/2;
                }
                else
                {
                    barHeight = 0; // scaleDivisor will only be zero if all values are zero
                }

                float yPos = outRect.height/2 - barHeight;
                Rect barRect = new Rect(xPos, yPos, barWidth, barHeight);
                Widgets.DrawBoxSolid(barRect, Color.yellow);
            }

            Widgets.DrawLine(new Vector2(0, outRect.height / 2), new Vector2(outRect.width, outRect.height / 2), Color.white, 1f);
        }
    }
}
