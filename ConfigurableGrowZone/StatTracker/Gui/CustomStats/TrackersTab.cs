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

        public void DrawTab(Rect pane)
        {
            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            //float trackerRectHeight = 45f;
            //float trackerRectWidth = pane.width;

            new RectStacker(pane)
                .ThenForEach(StatTrackers, (u, v, w) =>
                {
                    Rect trackerRect = new Rect(u);
                    //trackerRect.height = trackerRectHeight;
                    //trackerRect.width = trackerRectWidth;

                    return StatWidgets.DrawListItem(trackerRect, selectedTracker, v, w,
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
