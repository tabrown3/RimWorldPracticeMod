using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using Verse;

namespace ConfigurableGrowZone
{
    public class CustomStatTabs
    {
        public readonly List<TabRecord> LeftTabs = new List<TabRecord>();
        public readonly List<TabRecord> RightTabs = new List<TabRecord>();

        public ITabDrawable<CustomStatsTab> LeftActiveTab { get; private set; }
        public ITabDrawable<CustomStatsTab> RightActiveTab { get; private set; }

        private readonly TrackersTab TrackersTab = new TrackersTab();
        private readonly SignalsTab SignalsTab = new SignalsTab();
        private readonly MetricsTab MetricsTab = new MetricsTab();
        private readonly DerivedTab DerivedTab = new DerivedTab();

        public CustomStatTabs()
        {
            Observable.Merge(TrackersTab.OnListItemClick, SignalsTab.OnListItemClick).Subscribe(compStatTracker =>
            {
                MetricsTab.SetSource(compStatTracker.Data.SourceMetrics, compStatTracker.Data.History);
                DerivedTab.SetSource(compStatTracker.Data.DerivedMetrics, compStatTracker.Data.History);

                Log.Message($"Line item with name '{compStatTracker.Name}' clicked!");
            });
        }

        public void PreOpen()
        {
            LeftActiveTab = TrackersTab;
            RightActiveTab = MetricsTab;

            LeftTabs.Clear();
            RightTabs.Clear();

            LeftTabs.Add(new TabRecord("Trackers", () => LeftActiveTab = TrackersTab, () => LeftActiveTab == TrackersTab));
            LeftTabs.Add(new TabRecord("Signals", () => LeftActiveTab = SignalsTab, () => LeftActiveTab == SignalsTab));

            RightTabs.Add(new TabRecord("Metrics", () => RightActiveTab = MetricsTab, () => RightActiveTab == MetricsTab));
            RightTabs.Add(new TabRecord("Derived", () => RightActiveTab = DerivedTab, () => RightActiveTab == DerivedTab));
        }
    }
}
