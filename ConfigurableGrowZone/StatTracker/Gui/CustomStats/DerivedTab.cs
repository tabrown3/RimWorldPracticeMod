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
        private DerivedMetric selectedMetric = null;
        private StatTabList statTabList = StatWidgets.StatTabListFactory();

        public void DrawTab(Rect pane)
        {
            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            new RectStacker(pane)
                .ThenForEach(metrics, (u, v, w) =>
                {
                    return statTabList.DrawItem(u, selectedMetric, v, w,
                        (drawRect, curItem, ind) =>
                        {
                            Widgets.Label(drawRect, curItem.Name);
                        },
                        clickedObj =>
                        {
                            selectedMetric = clickedObj;
                            //onListItemClick.OnNext(clickedObj);
                        }
                    );
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
            selectedMetric = metrics.FirstOrDefault();
        }
    }
}
