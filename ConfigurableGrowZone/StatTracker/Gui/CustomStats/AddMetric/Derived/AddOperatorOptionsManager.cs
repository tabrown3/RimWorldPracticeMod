using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace ConfigurableGrowZone
{
    public class AddOperatorOptionsManager
    {
        private TimeDomain domain;
        private readonly List<Type> allOperatorTypes;
        private readonly List<SourceMetric> allSourceMetrics;
        private readonly Dictionary<GameTime.InTicks, List<string>> trackerNamesCache;
        private readonly Dictionary<TrackerDomain, List<SourceMetric>> metricsCache;
        private readonly Subject<TimeDomain> domainChanged = new Subject<TimeDomain>();

        public IObservable<TimeDomain> DomainChanged => domainChanged;

        public AddOperatorOptionsManager(List<SourceMetric> allSourceMetrics, List<Type> allOperatorTypes, TimeDomain initDomain)
        {
            this.allSourceMetrics = allSourceMetrics;
            this.allOperatorTypes = allOperatorTypes;
            domain = initDomain;

            trackerNamesCache = new Dictionary<GameTime.InTicks, List<string>>();
            metricsCache = new Dictionary<TrackerDomain, List<SourceMetric>>(new TrackerDomainEqualityComparer());
        }

        public List<Type> GetAvailableOperatorTypes()
        {
            return allOperatorTypes;
        }

        public List<string> GetAvailableTrackerNames()
        {
            if(trackerNamesCache.ContainsKey(domain.Resolution))
            {
                return trackerNamesCache[domain.Resolution];
            }
            else
            {
                var availableTrackerNames = allSourceMetrics.Where(u => u.Domain.Resolution == domain.Resolution).Select(u => u.ParentName).Distinct().ToList();

                trackerNamesCache.Add(domain.Resolution, availableTrackerNames);

                return availableTrackerNames;
            }
        }

        public bool TrackerHasChildOfCurrentDomain(string trackerName)
        {
            if (!string.IsNullOrEmpty(trackerName))
            {
                return allSourceMetrics.Where(u => u.Domain.Resolution == domain.Resolution && u.ParentName == trackerName).Count() > 0;

            }

            return false;
        }

        public List<SourceMetric> GetAvailableSourceMetrics(string trackerName)
        {
            TrackerDomain trackerDomain = new TrackerDomain(trackerName, domain);

            if(metricsCache.ContainsKey(trackerDomain))
            {
                return metricsCache[trackerDomain];
            }
            else
            {
                var availableSourceMetrics = allSourceMetrics
                    .Where(u => u.ParentName == trackerName && u.Domain.Resolution == domain.Resolution)
                    .ToList();

                metricsCache.Add(trackerDomain, availableSourceMetrics);

                return availableSourceMetrics;
            }
        }

        public bool SourceMetricIsOfCurrentDomain(SourceMetric sourceMetric)
        {
            if (sourceMetric != null)
            {
                return sourceMetric.Domain.Resolution == domain.Resolution;
            }

            return false;
        }

        public void ChangeDomain(TimeDomain inDomain)
        {
            domain = inDomain;
            domainChanged.OnNext(inDomain);
        }
    }
}
