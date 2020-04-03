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
            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            float derivedRectHeight = 25f;

            new RectStacker()
                .ThenForEach(metrics, (u, v, w) =>
                {
                    Rect derivedRect = new Rect(u);
                    derivedRect.height = derivedRectHeight;
                    derivedRect.width = Text.CalcSize(v.Name).x;

                    Widgets.Label(derivedRect, v.Name);

                    return derivedRect;
                })
                .IfThen(() => metrics.Count > 0, u =>
                {
                    Rect addDerivedButtonRect = new Rect(u);
                    addDerivedButtonRect.height = 35f;
                    addDerivedButtonRect.width = 80f;

                    if (Widgets.ButtonText(addDerivedButtonRect, "Add derived"))
                    {
                        onAddMetricClicked.OnNext(tracker);
                    }

                    return addDerivedButtonRect;
                });
        }

        public void SetSource(CompStatTracker tracker)
        {
            this.tracker = tracker;
            metrics = tracker.Data.DerivedMetrics;
        }
    }
}
