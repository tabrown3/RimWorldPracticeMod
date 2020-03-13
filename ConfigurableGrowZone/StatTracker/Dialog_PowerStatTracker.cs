using RimWorld;
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
        private DataVolume dataVolume;

        private float barWidth = 10f;
        private float barElementWidth => barWidth + spaceBetweenBars;

        private float spaceBetweenBars = 1f;
        private float graphVerticalPadding = 15f;
        private float yAxisLabelOffset = -8f;
        private float yAxisLabelPaneWidth = 55f;
        private float lastScaleDivisor = 1f;
        private float adjustedScaleDivisor = 1f;

        public Dialog_PowerStatTracker(DataVolume dataVolume)
        {
            this.dataVolume = dataVolume;

            draggable = true;
            preventCameraMotion = false;
            doCloseX = true;
            onlyOneOfTypeAllowed = false;
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
            Widgets.Label(titleRect, dataVolume.Name);
            
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

            int curTimeInTicks = Find.TickManager.TicksGame;
            int curTimeInChosenUnit = Mathf.FloorToInt(curTimeInTicks / (int)dataVolume.Resolution); // hours for right now

            // The presentation layer should determine how data is presented, so I think it's ok to leave this logic here
            List<DataPoint> historyList = dataVolume.DataPoints.OrderByDescending(u => u.TimeStampGameTicks).ToList();

            float maxValue = historyList.Max(u => u.Value);
            float minValue = historyList.Min(u => u.Value);

            float roofValue = Mathf.Max(maxValue, 0f);
            float basementValue = Mathf.Min(minValue, 0f);

            float scaleDivisor = Mathf.Max(roofValue, Mathf.Abs(basementValue));
            if (lastScaleDivisor != scaleDivisor && scaleDivisor != 0)
            {
                adjustedScaleDivisor = AdjustScaleDivisor(scaleDivisor);

                lastScaleDivisor = scaleDivisor;
            }
            else if(scaleDivisor == 0)
            {
                adjustedScaleDivisor = 0;
            }

            Rect innerGraphRect = new Rect(outerGraphRect);
            GUI.BeginGroup(innerGraphRect);
            innerGraphRect = innerGraphRect.AtZero();

            // draw y-axis and labels
            int numLines = 10;
            for (int i = -numLines; i <= numLines; i++)
            {
                float valToPrint = adjustedScaleDivisor * (-i / (float)numLines);

                string labelText = Mathf.RoundToInt(valToPrint).ToString();

                float valYPos = (((10 + i) * (innerGraphRect.height - graphVerticalPadding * 2)) / 20) + graphVerticalPadding;

                Widgets.Label(new Rect(yAxisLabelPaneWidth - 30f, valYPos + yAxisLabelOffset, Text.CalcSize(labelText).x, Text.CalcSize(labelText).y), labelText);

                if (i != 0) // zero already has a line: the x-axis
                {
                    Widgets.DrawLine(new Vector2(yAxisLabelPaneWidth, valYPos), new Vector2(innerGraphRect.width, valYPos), Color.gray, 1f); // chart top
                }
            }

            string yAxisUnitLabel = dataVolume.Unit;
            Widgets.Label(new Rect(0f, (innerGraphRect.height / 2) + yAxisLabelOffset, Text.CalcSize(yAxisUnitLabel).x, Text.CalcSize(yAxisUnitLabel).y), yAxisUnitLabel);
            GUI.EndGroup(); // end innerGraphLeftRect

            Rect innerGraphRightRect = new Rect(innerGraphRect);
            innerGraphRightRect.x += yAxisLabelPaneWidth;
            innerGraphRightRect.width -= yAxisLabelPaneWidth;
            GUI.BeginGroup(innerGraphRightRect);
            innerGraphRightRect = innerGraphRightRect.AtZero();

            int numBarsVisibleMax = Mathf.FloorToInt(innerGraphRightRect.width / barElementWidth);
            int numBarsVisible = Mathf.Min(numBarsVisibleMax, historyList.Count);

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
                    Vector2 labelSize = Text.CalcSize(labelText);

                    float yOffset = 4f;
                    if(i < dataVolume.DataPoints.Count)
                    {
                        DataPoint point = historyList[i];
                        if (point != null && point.Value < 0f)
                        {
                            yOffset -= labelSize.y + 4f;
                        }
                    }
                    
                    Widgets.Label(new Rect(xPos, zeroY + yOffset, labelSize.x, labelSize.y), labelText);
                }
            }

            // Draw bars
            for (int i = 0; i < numBarsVisible; i++)
            {
                DataPoint point = historyList[i];

                float xPos = (innerGraphRightRect.width - (i + 1) * barElementWidth) + 1f;

                float barHeight;
                if (adjustedScaleDivisor != 0) // wouldn't want to divide by zero, would we...
                {
                    float normalizedValue = (point.Value / adjustedScaleDivisor);
                    float maxHeight = (innerGraphRightRect.height / 2) - graphVerticalPadding;
                    barHeight = normalizedValue * maxHeight;
                }
                else
                {
                    barHeight = 0; // scaleDivisor will only be zero if all values are zero
                }

                Rect barRect = DrawBar(xPos, innerGraphRightRect.height / 2, barWidth, barHeight); // y is BOTTOM left corner, for my sanity

                if(Mouse.IsOver(barRect))
                {
                    var builder = new StringBuilder();
                    builder.AppendLine($"Value: {point.Value}{dataVolume.Unit}");
                    builder.AppendLine($"Date: {GetDateTimeString(point, dataVolume.LatLong)}");

                    TooltipHandler.TipRegion(barRect, builder.ToString());
                }
            }

            Widgets.DrawLine(new Vector2(0f, zeroY), new Vector2(innerGraphRightRect.width, zeroY), Color.white, 1f); // chart zero

            GUI.EndGroup(); // end innerGraphRightRect
            GUI.EndGroup(); // end innerGraphRect
        }

        private Rect DrawBar(float x, float y, float width, float height)
        {
            Rect barRect = new Rect(x, y - height, width, height);
            Widgets.DrawBoxSolid(barRect, Color.yellow);

            return barRect;
        }

        // This function will cause the y-axis labels to be nice round numbers
        //  for instance, if for scaleDivisor -> output:
        //  20 -> 25, 40 -> 50, 60 -> 75, 80 -> 100, 110 -> 250, 300 -> 500, 700 -> 750, 850 -> 1000, etc...
        private float AdjustScaleDivisor(float scaleDivisor)
        {
            float adjustedScaleDivisor = Mathf.Pow(10f, Mathf.Ceil(Mathf.Log10(scaleDivisor)));

            float quarterDivisor = adjustedScaleDivisor / 4;
            float halfDivisor = adjustedScaleDivisor / 2;

            if (scaleDivisor < quarterDivisor)
            {
                adjustedScaleDivisor = quarterDivisor;  // one-quarter its order of magnitude
            }
            else if(scaleDivisor < halfDivisor)
            {
                adjustedScaleDivisor = halfDivisor; // half its order of magnitude
            }
            else if(scaleDivisor < quarterDivisor + halfDivisor)
            {
                adjustedScaleDivisor = quarterDivisor + halfDivisor; // three-quarters its order of magnitude
            }

            return adjustedScaleDivisor;
        }

        private string GetDateTimeString(DataPoint point, Vector2 latLong)
        {
            return GenDate.DateFullStringWithHourAt(GenDate.TickGameToAbs(point.TimeStampGameTicks), latLong);
        }
    }
}
