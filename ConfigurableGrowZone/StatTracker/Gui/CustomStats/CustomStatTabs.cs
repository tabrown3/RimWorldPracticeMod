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

        private readonly List<IDisposable> unsubscribes;

        public CustomStatTabs()
        {
            var disp1 = Observable.Merge(TrackersTab.OnListItemClick, SignalsTab.OnListItemClick).Subscribe(compStatTracker =>
            {
                MetricsTab.SetSource(compStatTracker.Data.SourceMetrics, compStatTracker.Data.History);
                DerivedTab.SetSource(compStatTracker.Data.DerivedMetrics, compStatTracker.Data.History);
            });

            var disp2 = MetricsTab.OnAddMetricClicked.Subscribe(u =>
            {
                Log.Message("Clicked 'Add SourceMetric' button");
                Find.WindowStack.Add(new Dialog_AddSourceMetric());
                Log.Message("Executed 'Add SourceMetric' cb");
            });

            var disp3 = DerivedTab.OnAddMetricClicked.Subscribe(u =>
            {
                Log.Message("Clicked 'Add DerivedMetric' button");
                Find.WindowStack.Add(new Dialog_AddDerivedMetric());
                Log.Message("Executed 'Add DerivedMetric' cb");
            });

            unsubscribes = new List<IDisposable>()
            {
                disp1,
                disp2,
                disp3
            };
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

        public void PostClose()
        {
            unsubscribes.ForEach(u => u.Dispose());
        }
    }
}
