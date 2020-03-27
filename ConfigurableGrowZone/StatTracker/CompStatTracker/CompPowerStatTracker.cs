using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class CompPowerStatTracker : CompStatTracker
    {
        public CompProperties_PowerStatTracker Props => (CompProperties_PowerStatTracker)props;
        public PowerNet PowerNet => parent.GetComp<CompPower>()?.PowerNet;

        public CompPowerStatTracker()
        {
        }

        public override void Initialize(CompProperties baseProps)
        {
            base.Initialize(baseProps);
            CompProperties_PowerStatTracker props = (CompProperties_PowerStatTracker)baseProps;

            foreach (var el in props.SourceMetrics)
            {
                var metric = CreateMetric(el.MetricType, el.Key, el.Name, el.SourceType, el.Unit, el.DomainType, el.AggregatorType);
                this.AddSourceMetric(metric);
            }

            var firstDerivedMetric = DeriveMetric(
                "NegativeStoredEnergyEachHourPoll",
                "Negative Stored Energy at Hour",
                new List<Tuple<string, string>>()
                {
                    Tuple.Create(this.Name, "StoredEnergyEachHourPoll")
                },
                new List<string>()
                {
                    "ConfigurableGrowZone.NegateOperator"
                }
            );
            this.AddDerivedMetric(firstDerivedMetric);

            var secondDerivedMetric = DeriveMetric(
                "DoubleStoredEnergyNegatedEachHourPoll",
                "Summed Negative Stored Something at Hour",
                new List<Tuple<string, string>>()
                {
                    Tuple.Create(this.Name, "StoredEnergyEachHourPoll"),
                    Tuple.Create(this.Name, "EnergyGainByHourDigest")
                },
                new List<string>()
                {
                    "ConfigurableGrowZone.PlusOperator",
                    "ConfigurableGrowZone.NegateOperator"
                }
            );
            this.AddDerivedMetric(secondDerivedMetric);
        }

        public void AddSourceMetric(SourceMetric metric)
        {
            Data.AddSourceMetric(metric);
        }

        public void AddDerivedMetric(DerivedMetric derivedMetric)
        {
            Data.AddDerivedMetric(derivedMetric);
        }

        public override void CompTick()
        {
            base.CompTick();

            int ticksGame = Find.TickManager.TicksGame;

            foreach (SourceMetric metric in Data.SourceMetrics)
            {
                metric.Tick(ticksGame);
            }

            foreach (DerivedMetric derivedMetric in Data.DerivedMetrics)
            {
                derivedMetric.Tick(ticksGame, Data.History.History);
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            Command_Action command_Action = new Command_Action();
            command_Action.action = delegate
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var metric in Data.SourceMetrics)
                {
                    list.Add(new FloatMenuOption(metric.Name, delegate
                    {
                        Find.WindowStack.Add(new Dialog_BarChart(Data.History.Get(metric.Key))); // TODO: make this not break if key DNE in Dict
                    }));
                }
                foreach (var derivedMetric in Data.DerivedMetrics)
                {
                    list.Add(new FloatMenuOption(derivedMetric.Name, delegate
                    {
                        Find.WindowStack.Add(new Dialog_BarChart(Data.History.Get(derivedMetric.Key))); // TODO: make this not break if key DNE in Dict
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            };
            command_Action.defaultLabel = "View Stats";
            //command_Action.defaultDesc = "View readouts of stats currently being tracked.";
            command_Action.hotKey = KeyBindingDefOf.Misc5;
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower");
            yield return command_Action;
        }

        private SourceMetric CreateMetric(string metricTypeName, string key, string name, string sourceTypeName, string unit, string domainTypeName, string aggregatorTypeName = null)
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

        private SourceMetric CreateMetric(Type metricType, string key, string name, Type sourceType, string unit, Type domainType, Type aggregatorType = null)
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

        private DerivedMetric DeriveMetric(string key, string name, List<Tuple<string, string>> trackerMetricKv, List<string> operatorTypeNames)
        {
            var sourceMetrics = trackerMetricKv.Select(u => MapStatTracker.GetMetric(u.Item1, u.Item2)).ToList();
            var operators = operatorTypeNames.Select(u => (IOperator<float>)Activator.CreateInstance(Type.GetType(u))).ToList();

            return DeriveMetric(key, name, sourceMetrics, operators);
        }

        private DerivedMetric DeriveMetric(string key, string name, List<SourceMetric> sourceMetrics, List<IOperator<float>> operators)
        {
            var derivedMetric = new DerivedMetric(
                key,
                name,
                sourceMetrics,
                operators
            );

            return derivedMetric;
        }
    }
}
