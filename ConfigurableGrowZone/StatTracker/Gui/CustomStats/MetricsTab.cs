using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class MetricsTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Metrics;
        public IObservable<CompStatTracker> OnAddMetricClicked => onAddMetricClicked;

        private readonly Subject<CompStatTracker> onAddMetricClicked = new Subject<CompStatTracker>();
        private CompStatTracker tracker;
        private List<SourceMetric> metrics = new List<SourceMetric>();

        public void DrawTab(Rect pane)
        {
            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            float metricRectHeight = 25f;

            new RectStacker()
                .ThenForEach(metrics, (u, v, w) =>
                {
                    Rect metricRect = new Rect(u);
                    metricRect.height = metricRectHeight;
                    metricRect.width = Text.CalcSize(v.Name).x;

                    Widgets.Label(metricRect, v.Name);

                    return metricRect;
                })
                .IfThen(() => metrics.Count > 0, u =>
                {
                    Rect addMetricButtonRect = new Rect(u);
                    addMetricButtonRect.height = 35f;
                    addMetricButtonRect.width = 80f;

                    if (Widgets.ButtonText(addMetricButtonRect, "Add metric"))
                    {
                        onAddMetricClicked.OnNext(tracker);
                    }

                    return addMetricButtonRect;
                });
        }

        public void SetSource(CompStatTracker tracker)
        {
            this.tracker = tracker;
            metrics = tracker.Data.SourceMetrics;
        }
    }
}
