using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ConfigurableGrowZone
{
    public class Dialog_ConfigureEnergyThreshold : Window
    {
        private readonly Action<FloatRange> onAccept;
        private FloatRange floatRange;
        private bool hasEnergyCapacity;
        private float maxEnergyCapacity;

        public Dialog_ConfigureEnergyThreshold(FloatRange energyPercentageRange, bool hasEnergyCapacity, float? maxEnergyCapacity, Action<FloatRange> onAccept = null)
        {
            this.floatRange = new FloatRange(energyPercentageRange.min, energyPercentageRange.max);
            this.onAccept = onAccept;
            this.hasEnergyCapacity = hasEnergyCapacity;
            this.maxEnergyCapacity = maxEnergyCapacity.HasValue ? maxEnergyCapacity.Value : 0f;

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
            outRect.yMin += 20f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;

            float curY = 0f;

            float thresholdRangeControlHeight = 32f;
            Rect thresholdRangeControlRect = new Rect(0f, curY, outRect.width - 16f, thresholdRangeControlHeight);
            Widgets.FloatRange(thresholdRangeControlRect, 1, ref floatRange, 0, 1, valueStyle: ToStringStyle.PercentOne);
            curY += thresholdRangeControlHeight;
            float lowerEnergyPercentage = floatRange.min;
            float upperEnergyPercentage = floatRange.max;

            if (hasEnergyCapacity)
            {
                float lowerEnergyThreshold = Mathf.Lerp(0f, maxEnergyCapacity, lowerEnergyPercentage);
                float upperEnergyThreshold = Mathf.Lerp(0f, maxEnergyCapacity, upperEnergyPercentage);

                float lowerValueLabelHeight = 32f;
                Rect lowerValueLabelRect = new Rect(0f, curY, outRect.width - 16f, lowerValueLabelHeight);
                Widgets.Label(lowerValueLabelRect, $"Lower Threshold: {lowerEnergyThreshold.ToString("F")}Wd");
                curY += lowerValueLabelHeight;

                float upperValueLabelHeight = 32f;
                Rect upperValueLabelRect = new Rect(0f, curY, outRect.width - 16f, upperValueLabelHeight);
                Widgets.Label(upperValueLabelRect, $"Upper Threshold: {upperEnergyThreshold.ToString("F")}Wd");
                curY += upperValueLabelHeight;
            }

            float acceptButtonHeight = 32f;
            Rect acceptButtonRect = new Rect(0f, curY, outRect.width * 0.7f, acceptButtonHeight);
            curY += acceptButtonHeight;

            if (Widgets.ButtonText(acceptButtonRect, "Accept"))
            {
                SoundDefOf.Click.PlayOneShotOnCamera();

                if (onAccept != null)
                {
                    onAccept(new FloatRange(lowerEnergyPercentage, upperEnergyPercentage));
                }

                Close();
            }
        }
    }
}
