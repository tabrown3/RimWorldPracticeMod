using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class MetricsTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Metrics;

        private List<SourceMetric> metrics = new List<SourceMetric>();

        public void DrawTab(Rect pane)
        {
            Widgets.DrawBoxSolid(pane, Color.red);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            float trackerRectHeight = 20f;
            for (int i = 0; i < metrics.Count; i++)
            {
                var sourceMetric = metrics[i];

                Rect metricRect = new Rect(pane);
                metricRect.y = i * trackerRectHeight;
                metricRect.height = trackerRectHeight;

                Widgets.Label(metricRect, sourceMetric.Name);
            }
        }

        public void SetSource(List<SourceMetric> sourceMetrics, StatHistory history)
        {
            metrics = sourceMetrics;
        }
    }
}
