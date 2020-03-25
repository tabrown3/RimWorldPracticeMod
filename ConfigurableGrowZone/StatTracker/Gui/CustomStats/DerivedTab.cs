using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class DerivedTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Derived;

        private List<DerivedMetric> metrics = new List<DerivedMetric>();

        public void DrawTab(Rect pane)
        {
            Widgets.DrawBoxSolid(pane, Color.blue);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            float trackerRectHeight = 20f;
            for (int i = 0; i < metrics.Count; i++)
            {
                var derivedMetric = metrics[i];

                Rect metricRect = new Rect(pane);
                metricRect.y = i * trackerRectHeight;
                metricRect.height = trackerRectHeight;

                Widgets.Label(metricRect, derivedMetric.Name);
            }
        }

        public void SetSource(List<DerivedMetric> derivedMetrics, StatHistory history)
        {
            metrics = derivedMetrics;
        }
    }
}
