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
    public class TrackersTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Trackers;
        public List<CompStatTracker> StatTrackers => Find.CurrentMap?.GetComponent<MapStatTracker>()?.TrackerComps;
        public IObservable<CompStatTracker> OnListItemClick => onListItemClick;

        private readonly Subject<CompStatTracker> onListItemClick = new Subject<CompStatTracker>();
        private CompStatTracker selectedTracker = null;
        private StatTabList statTabList = StatWidgets.StatTabListFactory();

        public void DrawTab(Rect pane)
        {
            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            new RectStacker(pane)
                .ThenForEach(StatTrackers, (u, v, w) =>
                {
                    return statTabList.DrawItem(u, selectedTracker, v, w,
                        (drawRect, curItem, ind) =>
                        {
                            Widgets.Label(drawRect, curItem.Name);
                        },
                        clickedObj =>
                        {
                            selectedTracker = clickedObj;
                            onListItemClick.OnNext(clickedObj);
                        }
                    );
                });
        }
    }
}
