using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ConfigurableGrowZone
{
    public class WorkGiver_GrowerHarvestConfigurable : WorkGiver_GrowerHarvest
    {
        public WorkGiver_GrowerHarvestConfigurable()
        {
            //Log.Message("I'm Alive");
        }

        public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
        {
            var bob = base.HasJobOnCell(pawn, c, forced);

            if(bob)
            {
                //Log.Message($"{pawn.Name.ToStringFull} has job at cell {c.ToString()}");
            }
            return bob;
        }

        public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
        {
            //Log.Message("JobOnCell called");
            return base.JobOnCell(pawn, c, forced);
        }

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            var bob = base.ShouldSkip(pawn, forced);
            if(!bob)
            {
                //Log.Message($"Should not skip for {pawn.Name.ToStringFull}");
            }
            
            return bob;
        }
    }
}
