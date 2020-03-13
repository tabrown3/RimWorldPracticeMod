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
        public PowerStatData Data { get; private set; } // TODO: gonna need to save and load this with ExposeData eventually
        public CompProperties_PowerStatTracker Props => (CompProperties_PowerStatTracker)props;
        public PowerNet PowerNet => parent.GetComp<CompPower>()?.PowerNet;

        public CompPowerStatTracker()
        {
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            // Using Find.CurrentMap since parent.Map is still null at this point
            Find.CurrentMap.GetComponent<PowerStatTracker>().RegisterPowerStatTracker(this);
            Data = new PowerStatData(GetLatLong(parent));
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            previousMap.GetComponent<PowerStatTracker>().DeregisterPowerStatTracker(this);
        }

        public void AddMetric(DigestStatMetric metric)
        {
            metric.OnDigest += (o, ev) => { Log.Message($"Average of {metric.Name} in last {metric.Resolution} is {ev.DataPoint.Value}"); };
            Data.AddMetric(metric);
        }

        public override void CompTick()
        {
            base.CompTick();

            int ticksGame = Find.TickManager.TicksGame;

            foreach (DigestStatMetric metric in Data.Metrics)
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
                        Find.WindowStack.Add(new Dialog_PowerStatTracker(Data.History.Get(metric.Key))); // TODO: make this not break if key DNE in Dict
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            };
            command_Action.defaultLabel = "View Stats";
            //command_Action.defaultDesc = "View reaouts of stats currently being tracked.";
            command_Action.hotKey = KeyBindingDefOf.Misc5;
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower");
            yield return command_Action;
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
