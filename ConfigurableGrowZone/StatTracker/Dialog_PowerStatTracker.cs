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
        private string metricKey;
        private GameTime.InTicks resolution;
        private Dictionary<int, DataPoint> history;

        private float barWidth = 10f;
        private float spaceBetweenBars = 1f;
        private float barElementWidth => barWidth + spaceBetweenBars;

        public Dialog_PowerStatTracker(string metricKey, GameTime.InTicks resolution, Dictionary<int, DataPoint> history)
        {
            this.metricKey = metricKey;
            this.resolution = resolution;
            this.history = history;

            draggable = true;
            preventCameraMotion = false;
            doCloseX = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            GUI.BeginGroup(inRect);
            inRect = inRect.AtZero();

            float curY = 0f;

            Rect titleRect = new Rect(0f, curY, inRect.width, 32f);

            var origTextAnchor = Text.Anchor;
            Text.Anchor = TextAnchor.UpperCenter;
            Text.Font = GameFont.Medium;
            Widgets.Label(titleRect, metricKey);
            Text.Font = GameFont.Small;
            Text.Anchor = origTextAnchor;
            curY += titleRect.height;


            Rect graphRect = new Rect(inRect);
            graphRect.y = curY;
            graphRect.height -= 64f;
            GUI.BeginGroup(graphRect);
            graphRect = graphRect.AtZero();

            DrawGraph(graphRect);

            GUI.EndGroup(); // graphRect end
            curY += graphRect.height;

            GUI.EndGroup(); // inRect end
        }

        private void DrawGraph(Rect graphRect)
        {
            float zeroY = graphRect.height / 2;

            // The presentation layer should determine how data is presented, so I think it's ok to leave this logic here
            List<DataPoint> historyList = history.Select(u => u.Value).OrderByDescending(u => u.TimeStamp).ToList();

            int numBarsVisibleMax = Mathf.FloorToInt(graphRect.width / barElementWidth);
            int numBarsVisible = Mathf.Min(numBarsVisibleMax, historyList.Count);
            float maxValue = historyList.Max(u => u.DigestValue);
            float minValue = historyList.Min(u => u.DigestValue);

            float roofValue = Mathf.Max(maxValue, 0f);
            float basementValue = Mathf.Min(minValue, 0f);

            float scaleDivisor = Mathf.Max(roofValue, Mathf.Abs(basementValue));

            int curTick = Find.TickManager.TicksGame;
            int curHour = Mathf.FloorToInt(curTick / (int)GameTime.InTicks.Hour);

            // Draw horizontal ticks along zero line and label them
            Text.Font = GameFont.Tiny;
            for (int i = 0; i < numBarsVisibleMax; i++)
            {
                // TODO: duplicated from below
                float xPos = (graphRect.width - (i + 1) * barElementWidth);

                Widgets.DrawLine(new Vector2(xPos, zeroY - 2f), new Vector2(xPos, zeroY + 4f), Color.white, 1f); // chart top

                int hourToDraw = (6 + curHour - i - 1) % 24;
                if (hourToDraw < 0)
                {
                    hourToDraw += 24;
                }

                if (hourToDraw % 2 == 0)
                {
                    string labelText = hourToDraw.ToString();
                    Widgets.Label(new Rect(xPos, zeroY + 4f, Text.CalcSize(labelText).x, Text.CalcSize(labelText).y), labelText);
                }
            }

            float bob = scaleDivisor / 10;
            for (int i = -9; i <= 9; i++)
            {
                if(i == 0)
                {
                    continue;
                }

                float valToPrint = scaleDivisor * -i;

                string labelText = Mathf.RoundToInt(valToPrint).ToString();

                float valYPos = (10 + i) * graphRect.height / 20;

                if(valToPrint > 0)
                {
                    valYPos -= Text.CalcSize(labelText).y;
                }
                Widgets.Label(new Rect(0f, valYPos, Text.CalcSize(labelText).x, Text.CalcSize(labelText).y), labelText);
            }
            Text.Font = GameFont.Small;

            // Draw bars
            for (int i = 0; i < numBarsVisible; i++)
            {
                DataPoint point = historyList[i];

                float xPos = (graphRect.width - (i + 1) * barElementWidth) + 1f;

                float barHeight;
                if (scaleDivisor != 0) // wouldn't want to divide by zero, would we...
                {
                    barHeight = (point.DigestValue / scaleDivisor) * graphRect.height / 2;
                }
                else
                {
                    barHeight = 0; // scaleDivisor will only be zero if all values are zero
                }

                float yPos = graphRect.height / 2 - barHeight;
                Rect barRect = new Rect(xPos, yPos, barWidth, barHeight);
                Widgets.DrawBoxSolid(barRect, Color.yellow);
            }

            Widgets.DrawLine(new Vector2(0f, 0f), new Vector2(graphRect.width, 0f), Color.white, 1f); // chart top
            Widgets.DrawLine(new Vector2(0f, zeroY), new Vector2(graphRect.width, zeroY), Color.white, 1f); // chart zero
            Widgets.DrawLine(new Vector2(0f, graphRect.height - 1f), new Vector2(graphRect.width, graphRect.height - 1f), Color.white, 1f); // chart bottom
        }
    }
}
