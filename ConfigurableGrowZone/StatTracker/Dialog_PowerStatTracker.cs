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
        private string unit;

        private float barWidth = 10f;
        private float barElementWidth => barWidth + spaceBetweenBars;

        private float spaceBetweenBars = 1f;
        private float graphVerticalPadding = 15f;
        private float yAxisLabelOffset = -8f;
        private float scaleOffsetFactor = 0.9f; // 1f would make the longest bars take up the full height; 0.9f makes them 90% the full height
        private float yAxisLabelPaneWidth = 55f;

        public Dialog_PowerStatTracker(string metricKey, GameTime.InTicks resolution, string unit, Dictionary<int, DataPoint> history)
        {
            this.metricKey = metricKey;
            this.resolution = resolution;
            this.history = history;
            this.unit = unit;

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

        private void DrawGraph(Rect outerGraphRect)
        {
            Text.Font = GameFont.Tiny;
            float zeroY = outerGraphRect.height / 2;

            // The presentation layer should determine how data is presented, so I think it's ok to leave this logic here
            List<DataPoint> historyList = history.Select(u => u.Value).OrderByDescending(u => u.TimeStamp).ToList();

            float maxValue = historyList.Max(u => u.DigestValue);
            float minValue = historyList.Min(u => u.DigestValue);

            float roofValue = Mathf.Max(maxValue, 0f);
            float basementValue = Mathf.Min(minValue, 0f);

            float scaleDivisor = Mathf.Max(roofValue, Mathf.Abs(basementValue)) / scaleOffsetFactor;

            Rect innerGraphRect = new Rect(outerGraphRect);
            GUI.BeginGroup(innerGraphRect);
            innerGraphRect = innerGraphRect.AtZero();

            // draw y-axis and labels
            float bob = scaleDivisor / 10;
            for (int i = -10; i <= 10; i++)
            {
                float valToPrint = scaleDivisor * -i;

                string labelText = Mathf.RoundToInt(valToPrint).ToString();

                float valYPos = (((10 + i) * (innerGraphRect.height - graphVerticalPadding * 2)) / 20) + graphVerticalPadding;

                Widgets.Label(new Rect(yAxisLabelPaneWidth - 30f, valYPos + yAxisLabelOffset, Text.CalcSize(labelText).x, Text.CalcSize(labelText).y), labelText);

                if (i != 0) // zero already has a line: the x-axis
                {
                    Widgets.DrawLine(new Vector2(yAxisLabelPaneWidth, valYPos), new Vector2(innerGraphRect.width, valYPos), Color.gray, 1f); // chart top
                }
            }

            string yAxisUnitLabel = unit;
            Widgets.Label(new Rect(0f, (innerGraphRect.height / 2) + yAxisLabelOffset, Text.CalcSize(yAxisUnitLabel).x, Text.CalcSize(yAxisUnitLabel).y), yAxisUnitLabel);
            GUI.EndGroup(); // end innerGraphLeftRect

            Rect innerGraphRightRect = new Rect(innerGraphRect);
            innerGraphRightRect.x += yAxisLabelPaneWidth;
            innerGraphRightRect.width -= yAxisLabelPaneWidth;
            GUI.BeginGroup(innerGraphRightRect);
            innerGraphRightRect = innerGraphRightRect.AtZero();

            int numBarsVisibleMax = Mathf.FloorToInt(innerGraphRightRect.width / barElementWidth);
            int numBarsVisible = Mathf.Min(numBarsVisibleMax, historyList.Count);
            int curTimeInTicks = Find.TickManager.TicksGame;
            int curTimeInChosenUnit = Mathf.FloorToInt(curTimeInTicks / (int)resolution); // hours for right now

            // Draw x-axis and labels
            for (int i = 0; i < numBarsVisibleMax; i++)
            {
                // TODO: duplicated from below
                float xPos = (innerGraphRightRect.width - (i + 1) * barElementWidth);

                Widgets.DrawLine(new Vector2(xPos, zeroY - 2f), new Vector2(xPos, zeroY + 4f), Color.white, 1f); // chart top

                int hourToDraw = (6 + curTimeInChosenUnit - i - 1) % 24;
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

            // Draw bars
            for (int i = 0; i < numBarsVisible; i++)
            {
                DataPoint point = historyList[i];

                float xPos = (innerGraphRightRect.width - (i + 1) * barElementWidth) + 1f;

                float barHeight;
                if (scaleDivisor != 0) // wouldn't want to divide by zero, would we...
                {
                    float normalizedValue = (point.DigestValue / scaleDivisor);
                    float maxHeight = (innerGraphRightRect.height / 2) - graphVerticalPadding;
                    barHeight = normalizedValue * maxHeight;
                }
                else
                {
                    barHeight = 0; // scaleDivisor will only be zero if all values are zero
                }

                DrawBar(xPos, innerGraphRightRect.height / 2, barWidth, barHeight); // y is BOTTOM left corner, for my sanity
            }

            Widgets.DrawLine(new Vector2(0f, zeroY), new Vector2(innerGraphRightRect.width, zeroY), Color.white, 1f); // chart zero

            GUI.EndGroup(); // end innerGraphRightRect
            GUI.EndGroup(); // end innerGraphRect
        }

        private void DrawBar(float x, float y, float width, float height)
        {
            Rect barRect = new Rect(x, y - height, width, height);
            Widgets.DrawBoxSolid(barRect, Color.yellow);
        }
    }
}
