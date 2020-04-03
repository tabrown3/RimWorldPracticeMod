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

        public void DrawTab(Rect pane)
        {
            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            float trackerRectHeight = 25f;

            new RectStacker()
                .ThenForEach(StatTrackers, (u, v, w) =>
                {
                    Rect trackerRect = new Rect(u);
                    trackerRect.height = trackerRectHeight;
                    trackerRect.width = Text.CalcSize(v.Name).x;
                    Widgets.Label(trackerRect, v.Name);

                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(trackerRect))
                    {
                        onListItemClick.OnNext(v);
                    }

                    return trackerRect;
                });
        }
    }
}
