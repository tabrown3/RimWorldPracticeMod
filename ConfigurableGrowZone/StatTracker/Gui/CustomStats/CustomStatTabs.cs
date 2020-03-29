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

        private MapStatTracker mapStatTracker => Find.CurrentMap.GetComponent<MapStatTracker>();

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
                MetricsTab.SetSource(compStatTracker);
                DerivedTab.SetSource(compStatTracker);
            });

            var disp2 = MetricsTab.OnAddMetricClicked.SelectMany(tracker => // SelectMany just merges Observables into a single Observable
            {
                var dialog = new Dialog_AddSourceMetric(
                    tracker,
                    StatTypesHelper.DomainTypes,
                    StatTypesHelper.SourceTypes,
                    StatTypesHelper.AggregatorTypes);

                Find.WindowStack.Add(dialog);

                return dialog.OnSubmit;
            }).Subscribe(u =>
            {
                var tracker = u.Item1;
                var form = u.Item2;

                tracker.AddSourceMetric(form.MetricType, form.Key, form.Name, form.SourceType, form.Unit, form.DomainType, form.AggregatorType);
            });

            var disp3 = DerivedTab.OnAddMetricClicked.SelectMany(tracker =>
            {
                var dialog = new Dialog_AddDerivedMetric(tracker, mapStatTracker.GetMetrics(), StatTypesHelper.OperatorTypes);

                Find.WindowStack.Add(dialog);

                return dialog.OnSubmit;
            }).Subscribe(u =>
            {
                var tracker = u.Item1;
                var form = u.Item2;

                tracker.AddDerivedMetric(form.Key, form.Name, form.SourceMetrics.ToList(), form.Operators);
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
