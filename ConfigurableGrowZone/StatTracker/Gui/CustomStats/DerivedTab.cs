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
    public class DerivedTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Derived;
        public IObservable<CompStatTracker> OnAddMetricClicked => onAddMetricClicked;

        private readonly Subject<CompStatTracker> onAddMetricClicked = new Subject<CompStatTracker>();
        private CompStatTracker tracker;
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
            float curY = trackerRectHeight * metrics.Count;

            if (metrics.Count > 0)
            {
                float addMetricButtonHeight = 35f;
                Rect addMetricButtonRect = new Rect(0f, curY, 80f, addMetricButtonHeight);
                curY += addMetricButtonHeight;

                if (Widgets.ButtonText(addMetricButtonRect, "Add metric"))
                {
                    onAddMetricClicked.OnNext(tracker); // TODO: Hmm, just sending true? That's weird...
                }
            }
        }

        public void SetSource(CompStatTracker tracker)
        {
            this.tracker = tracker;
            metrics = tracker.Data.DerivedMetrics;
        }
    }
}
