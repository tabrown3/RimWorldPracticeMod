using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class Building_CustomSwitch : Building
    {
        public CompEnergyThreshold CompEnergyThreshold => GetComp<CompEnergyThreshold>();

        public override Graphic Graphic => DefaultGraphic;

        public override void TickRare()
        {
            base.TickRare();
            var grayCell = Position + IntVec3.East.RotatedBy(Rotation);
            var dependentSwitch = grayCell.GetFirstThing<Building_PowerSwitch>(Map);
            var dependentFlickableComp = dependentSwitch.GetComp<CompFlickable>();

            if(PowerComp.PowerNet != null && PowerComp.PowerNet.batteryComps.Count > 0)
            {
                if (dependentFlickableComp.SwitchIsOn && Math.Round(PowerComp.PowerNet.CurrentStoredEnergy()) > Math.Round(CompEnergyThreshold.UpperEnergyThreshold))
                {
                    dependentFlickableComp.SwitchIsOn = false;
                }
                else if (!dependentFlickableComp.SwitchIsOn && Math.Round(PowerComp.PowerNet.CurrentStoredEnergy()) < Math.Round(CompEnergyThreshold.LowerEnergyThreshold))
                {
                    dependentFlickableComp.SwitchIsOn = true;
                }
            }
        }

        public override bool TransmitsPowerNow => true;

        public override string GetInspectString()
        {
            var builder = new StringBuilder();

            string baseInspectString = base.GetInspectString();
            if (!string.IsNullOrEmpty(baseInspectString))
            {
                builder.AppendLine(baseInspectString);
            }

            if (CompEnergyThreshold.PowerNetStoredEnergyMax > 0f)
            {
                builder.AppendLine("Lower Threshold: " + CompEnergyThreshold.LowerEnergyThreshold.ToString("F") + "Wd");
                builder.Append("Upper Threshold: " + CompEnergyThreshold.UpperEnergyThreshold.ToString("F") + "Wd");
            }

            builder.AppendLine("Lower Energy Percentage: " + CompEnergyThreshold.EnergyPercentageRange.min.ToStringByStyle(ToStringStyle.PercentOne));
            builder.AppendLine("Upper Energy Percentage: " + CompEnergyThreshold.EnergyPercentageRange.max.ToStringByStyle(ToStringStyle.PercentOne));

            return builder.ToString();
        }
    }
}
