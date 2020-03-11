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
        private string metricKey;

        float barWidth = 10f;

        public Dialog_PowerStatTracker(string metricKey, Dictionary<int, DataPoint> history)
        {
            this.metricKey = metricKey;
            this.history = history;

            draggable = true;
            preventCameraMotion = false;
            doCloseX = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            float curY = 0f;

            Rect titleRect = new Rect(inRect);
            titleRect.y = curY;
            titleRect.height = 32f;
            Widgets.Label(titleRect, metricKey);
            curY += titleRect.height;


            Rect graphRect = new Rect(inRect);
            graphRect.y = curY;
            graphRect.height -= 64f;
            //graphRect.width -= 16f;
            //GUI.BeginGroup(graphRect)
            curY += graphRect.height;

            List<DataPoint> historyList = history.Select(u => u.Value).OrderByDescending(u => u.TimeStamp).ToList();

            int barCount = Mathf.Min(Mathf.FloorToInt(graphRect.width / barWidth), historyList.Count);
            float maxValue = historyList.Max(u => u.DigestValue);
            float minValue = historyList.Min(u => u.DigestValue);

            float roofValue = Mathf.Max(maxValue, 0);
            float basementValue = Mathf.Min(minValue, 0);

            float scaleDivisor = Mathf.Max(roofValue, Mathf.Abs(basementValue));

            for (int i = 0; i < barCount; i++)
            {
                DataPoint point = historyList[i];

                float xPos = graphRect.width - (i + 1) * barWidth;

                float barHeight;
                if (scaleDivisor != 0) // wouldn't want to divide by zero, would we...
                {
                    barHeight = (point.DigestValue / scaleDivisor) * graphRect.height/2;
                }
                else
                {
                    barHeight = 0; // scaleDivisor will only be zero if all values are zero
                }

                float yPos = (graphRect.y + graphRect.height/2) - barHeight;
                Rect barRect = new Rect(xPos, yPos, barWidth, barHeight);
                Widgets.DrawBoxSolid(barRect, Color.yellow);
            }

            Widgets.DrawLine(new Vector2(0, graphRect.y), new Vector2(graphRect.width, graphRect.y), Color.white, 1f); // chart top
            Widgets.DrawLine(new Vector2(0, graphRect.y + graphRect.height / 2), new Vector2(graphRect.width, graphRect.y + graphRect.height / 2), Color.white, 1f); // chart zero
            Widgets.DrawLine(new Vector2(0, graphRect.y + graphRect.height), new Vector2(graphRect.width, graphRect.y + graphRect.height), Color.white, 1f); // chart bottom
        }
    }
}
