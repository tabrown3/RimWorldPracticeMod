using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class CompStatTracker : ThingComp
    {
        public string Name { get; set; }
        public StatData Data { get; private set; }
        public Vector2 LatLong => GetLatLong(parent);
        protected MapStatTracker MapStatTracker => Find.CurrentMap.GetComponent<MapStatTracker>();

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            Data = new StatData(LatLong);
            Name = this.GetType().Name + ":" + MapStatTracker.RegisterStatTracker(this);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Data.PersistData();
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            previousMap.GetComponent<MapStatTracker>().DeregisterStatTracker(this);
        }

        public void AddSourceMetric(Type metricType, string key, string name, Type sourceType, string unit, Type domainType, Type aggregatorType = null)
        {
            var soureMetric = CreateMetric(metricType, key, name, sourceType, unit, domainType, aggregatorType);
            AddSourceMetric(soureMetric);
        }

        public void AddSourceMetric(SourceMetric metric)
        {
            Data.AddSourceMetric(metric);
        }

        public void AddDerivedMetric(string key, string name, List<SourceMetric> sourceMetrics, List<IOperator<float>> operators)
        {
            var derivedMetric = DeriveMetric(key, name, sourceMetrics, operators);
            AddDerivedMetric(derivedMetric);
        }

        public void AddDerivedMetric(DerivedMetric derivedMetric)
        {
            Data.AddDerivedMetric(derivedMetric);
        }

        protected SourceMetric CreateMetric(string metricTypeName, string key, string name, string sourceTypeName, string unit, string domainTypeName, string aggregatorTypeName = null)
        {
            var metricType = Type.GetType(metricTypeName);
            var sourceType = Type.GetType(sourceTypeName);
            var domainType = Type.GetType(domainTypeName);

            Type aggregatorType = null;
            if (!string.IsNullOrEmpty(aggregatorTypeName))
            {
                aggregatorType = Type.GetType(aggregatorTypeName);
            }

            return CreateMetric(metricType, key, name, sourceType, unit, domainType, aggregatorType);
        }

        protected SourceMetric CreateMetric(Type metricType, string key, string name, Type sourceType, string unit, Type domainType, Type aggregatorType = null)
        {
            SourceMetric metric;
            if (metricType == typeof(PollSourceMetric))
            {
                metric = new PollSourceMetric(
                    key,
                    name,
                    (IPullable<float>)Activator.CreateInstance(sourceType, new object[] { parent }),
                    unit,
                    (TimeDomain)Activator.CreateInstance(domainType)
                );
            }
            else if (metricType == typeof(DigestSourceMetric))
            {
                metric = new DigestSourceMetric(
                    key,
                    name,
                    (IPullable<float>)Activator.CreateInstance(sourceType, new object[] { parent }),
                    unit,
                    (TimeDomain)Activator.CreateInstance(domainType),
                    (IAggregator<float>)Activator.CreateInstance(aggregatorType)
                );
            }
            else if (metricType == typeof(WindowSourceMetric))
            {
                metric = new WindowSourceMetric(
                    key,
                    name,
                    (IPullable<float>)Activator.CreateInstance(sourceType, new object[] { parent }),
                    unit,
                    (TimeDomain)Activator.CreateInstance(domainType),
                    (IAggregator<float>)Activator.CreateInstance(aggregatorType)
                );
            }
            else
            {
                throw new Exception($"Error creating type {nameof(metricType)}: Custom metric types not supported at this time");
            }

            return metric;
        }

        protected DerivedMetric DeriveMetric(string key, string name, List<Tuple<string, string>> trackerMetricKv, List<string> operatorTypeNames)
        {
            var sourceMetrics = trackerMetricKv.Select(u => MapStatTracker.GetMetric(u.Item1, u.Item2)).ToList();
            var operators = operatorTypeNames.Select(u => (IOperator<float>)Activator.CreateInstance(Type.GetType(u))).ToList();

            return DeriveMetric(key, name, sourceMetrics, operators);
        }

        protected DerivedMetric DeriveMetric(string key, string name, List<SourceMetric> sourceMetrics, List<IOperator<float>> operators)
        {
            var derivedMetric = new DerivedMetric(
                key,
                name,
                sourceMetrics,
                operators
            );

            return derivedMetric;
        }

        private Vector2 GetLatLong(Thing thing) // taken from game GenLocalDate.LocationForDate
        {
            int tile = thing.Tile;
            if (tile >= 0)
            {
                return Find.WorldGrid.LongLatOf(tile);
            }
            return Vector2.zero;
        }
    }
}
