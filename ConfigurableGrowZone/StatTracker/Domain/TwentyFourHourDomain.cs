using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class TwentyFourHourDomain : TimeDomain
    {
        public TwentyFourHourDomain() : base(GameTime.InTicks.Hour)
        {
            DomainFunc = DomainGenerator;
        }

        private float DomainGenerator(int i)
        {
            int curTimeInTicks = Find.TickManager.TicksGame;
            int curTimeInChosenUnit = Mathf.FloorToInt(curTimeInTicks / ResInTicks); // hours for right now
            int hourToDraw = (6 + curTimeInChosenUnit - i - 1) % 24;
            if (hourToDraw < 0)
            {
                hourToDraw += 24;
            }

            return hourToDraw;
        }
    }
}
