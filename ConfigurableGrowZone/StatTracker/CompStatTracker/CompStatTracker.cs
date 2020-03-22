using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class CompStatTracker : ThingComp
    {
        public string Name { get; set; }
        public StatData Data { get; private set; }
        public Vector2 LatLong => GetLatLong(parent);

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            Data = new StatData(LatLong);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Data.PersistData();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            // Using Find.CurrentMap since parent.Map is still null at this point
            Find.CurrentMap.GetComponent<MapStatTracker>().RegisterPowerStatTracker(this);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            previousMap.GetComponent<MapStatTracker>().DeregisterPowerStatTracker(this);
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
