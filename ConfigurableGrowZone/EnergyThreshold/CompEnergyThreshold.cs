using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class CompEnergyThreshold : ThingComp
    {
        public CompProperties_EnergyThreshold Props => (CompProperties_EnergyThreshold)props;
        public float LowerEnergyThreshold => PowerNet != null ? Mathf.Lerp(0f, PowerNetStoredEnergyMax, EnergyPercentageRange.min) : 0f;
        public float UpperEnergyThreshold => PowerNet != null ? Mathf.Lerp(0f, PowerNetStoredEnergyMax, EnergyPercentageRange.max) : 0f;
        public FloatRange EnergyPercentageRange { get; set; }
        public PowerNet PowerNet => parent.GetComp<CompPower>()?.PowerNet;
        public float PowerNetStoredEnergyMax => PowerNet != null ? PowerNet.batteryComps.Sum(u => u.Props.storedEnergyMax) : 0f;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            EnergyPercentageRange = new FloatRange(Props.lowerEnergyPercentage, Props.upperEnergyPercentage);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach(var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            Command_Action command_Action = new Command_Action();
            command_Action.action = delegate
            {
                int? batteryCount = PowerNet?.batteryComps?.Count;
                bool hasEnergyCapacity = batteryCount.HasValue ? batteryCount.Value > 0 : false;

                Find.WindowStack.Add(new Dialog_ConfigureEnergyThreshold(EnergyPercentageRange, hasEnergyCapacity, PowerNetStoredEnergyMax, range => {
                    EnergyPercentageRange = range;
                }));
            };
            command_Action.defaultLabel = "Set Threshold";
            command_Action.defaultDesc = "Set the upper energy threshold where the switch should disengage, and the lower threshold where it should re-engage.";
            command_Action.hotKey = KeyBindingDefOf.Misc5;
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower");
            yield return command_Action;
        }

        public override void PostExposeData()
        {
            Persist(EnergyPercentageRange, threshold => EnergyPercentageRange = threshold);
        }

        private void Persist(FloatRange energyPercentageRange, Action<FloatRange> persistCb)
        {
            Scribe_Values.Look(ref energyPercentageRange, "HZ_energyPercentageRange"); 

            persistCb(energyPercentageRange);
        }
    }
}
