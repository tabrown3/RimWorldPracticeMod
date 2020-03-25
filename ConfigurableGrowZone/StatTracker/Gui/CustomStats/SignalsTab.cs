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
    public class SignalsTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Signals;
        public IObservable<CompStatTracker> OnListItemClick => onListItemClick;

        private readonly Subject<CompStatTracker> onListItemClick = new Subject<CompStatTracker>();

        public void DrawTab(Rect pane)
        {
            Widgets.DrawBoxSolid(pane, Color.blue);
        }
    }
}
