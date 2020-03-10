using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace ConfigurableGrowZone
{
    public class PlaceWorker_CustomSwitch : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
			IntVec3 intVec = center + IntVec3.East.RotatedBy(rot);

			GenDraw.DrawFieldEdges(new List<IntVec3> { intVec }, Color.white);
		}

        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            IntVec3 c = center + IntVec3.East.RotatedBy(rot);
            var bob = c.GetFirstThing<Building_PowerSwitch>(map);
            if(bob == null)
            {
                return "Gray area must be over a Power switch";
            }

            return true;
        }
    }
}
