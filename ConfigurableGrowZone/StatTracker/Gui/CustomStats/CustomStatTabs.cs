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

        private List<IDisposable> unsubscribes;

        public void PreOpen()
        {
            SetUpSubscriptions();

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

        private void SetUpSubscriptions()
        {
            var disp1 = Observable.Merge(TrackersTab.OnListItemClick, SignalsTab.OnListItemClick).Subscribe(compStatTracker =>
            {
                Log.Message("Clicked TrackersTab item or SignalsTab item");
                MetricsTab.SetSource(compStatTracker.Data.SourceMetrics, compStatTracker.Data.History);
                DerivedTab.SetSource(compStatTracker.Data.DerivedMetrics, compStatTracker.Data.History);
            });

            var disp2 = MetricsTab.OnAddMetricClicked.Subscribe(u =>
            {
                var dialog = new Dialog_AddSourceMetric(
                    StatTypesHelper.DomainTypes,
                    StatTypesHelper.SourceTypes,
                    StatTypesHelper.AggregatorTypes);

                Find.WindowStack.Add(dialog);

                var disp4 = dialog.OnSubmit.Subscribe(v =>
                {
                    Log.Message($"Name: {v.Name}");
                    Log.Message($"Key: {v.Key}");
                    Log.Message($"Domain: {v.DomainType.Name}");
                    Log.Message($"Source: {v.SourceType.Name}");
                    Log.Message($"Aggregator: {v.AggregatorType?.Name}");
                });

                unsubscribes.Add(disp4);
            });

            var disp3 = DerivedTab.OnAddMetricClicked.Subscribe(u =>
            {
                Find.WindowStack.Add(new Dialog_AddDerivedMetric());
            });

            unsubscribes = new List<IDisposable>()
            {
                disp1,
                disp2,
                disp3
            };
        }
    }
}
