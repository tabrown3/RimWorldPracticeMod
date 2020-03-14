using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class QuarterHourDomain : TimeDomain
    {
        public QuarterHourDomain() : base(GameTime.InTicks.QuarterHour)
        {
            DomainFunc = DomainGenerator;
        }

        private float DomainGenerator(int i)
        {
            int curTimeInTicks = Find.TickManager.TicksGame;
            int curTimeInChosenUnit = Mathf.FloorToInt(curTimeInTicks / ResInTicks); // quarter-hours for right now
            int quarterHourToDraw = (Mathf.FloorToInt(curTimeInChosenUnit) - i - 1);
            //if (hourToDraw < 0)
            //{
            //    hourToDraw += 24;
            //}

            return quarterHourToDraw;
        }
    }
}
