using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class CompPowerStatTracker : ThingComp
    {
        public PowerStatData Data { get; } // TODO: gonna need to save and load this with ExposeData eventually
        public CompProperties_PowerStatTracker Props => (CompProperties_PowerStatTracker)props;
        public PowerNet PowerNet => parent.GetComp<CompPower>()?.PowerNet;

        public CompPowerStatTracker()
        {
            Data = new PowerStatData();
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            // Using Find.CurrentMap since parent.Map is still null at this point
            Find.CurrentMap.GetComponent<PowerStatTracker>().RegisterPowerStatTracker(this);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            previousMap.GetComponent<PowerStatTracker>().DeregisterPowerStatTracker(this);
        }

        public void AddMetric(PowerStatMetric metric)
        {
            metric.OnDigest += (o, ev) => { Log.Message($"Average of {ev.DataPoint.Key} in last {ev.DataPoint.Resolution} is {ev.DataPoint.DigestValue}"); };
            Data.AddMetric(metric);
        }

        public override void CompTick()
        {
            base.CompTick();

            int ticksGame = Find.TickManager.TicksGame;

            foreach (PowerStatMetric metric in Data.Metrics)
            {
                metric.Tick(ticksGame);
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
                foreach (var metric in Data.Metrics)
                {
                    list.Add(new FloatMenuOption(metric.Key, delegate
                    {
                        Log.Message(metric.Key);
                        Find.WindowStack.Add(new Dialog_PowerStatTracker(Data.History[metric.Key]));
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            };
            command_Action.defaultLabel = "View Stats";
            command_Action.defaultDesc = "View reaouts of stats currently being tracked.";
            command_Action.hotKey = KeyBindingDefOf.Misc5;
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower");
            yield return command_Action;
        }
    }
}
