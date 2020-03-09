using RimWorld;
using Verse;

namespace BatteryTest
{
    public class BatteryDischargeTest : MapComponent
    {
        private float lastStoredEnergy = 0f;

        private decimal fakeDecimalStoredEnergy = 0m;
        private decimal fakeDecimalLastStoredEnergy = 0m;

        private float fakeFloatStoredEnergy = 0f;
        private float fakeFloatLastStoredEnergy = 0f;

        private bool lastValuesSet = false;
        private bool fakeStoredEnergySet = false;

        public BatteryDischargeTest(Map map) : base(map)
        {
            //SimulateBatterySelfDischarge();
        }

        public override void MapComponentTick()
        {
            var allNets = map.powerNetManager.AllNetsListForReading;
            if (allNets.Count > 0)
            {
                var batteryComps = allNets[0].batteryComps;
                if (batteryComps.Count > 0)
                {
                    var batteryComp = batteryComps[0];

                    // sync up fakeStoredEnergySet on first execution
                    if (!fakeStoredEnergySet && batteryComp.StoredEnergy > 0f)
                    {
                        fakeDecimalStoredEnergy = (decimal)batteryComp.StoredEnergy;
                        fakeFloatStoredEnergy = batteryComp.StoredEnergy;
                        fakeStoredEnergySet = true;
                    }

                    if (Find.TickManager.TicksGame % 60000 == 2500) // every in-game day at 7h
                    {
                        // if we have the current and last real energy values, print the difference
                        if (lastValuesSet)
                        {
                            var energyLostInLastDay = lastStoredEnergy - batteryComp.StoredEnergy;
                            Log.Message($"Real: {lastStoredEnergy}Wd - {batteryComp.StoredEnergy}Wd = {energyLostInLastDay}Wd");
                        }

                        // if we have the current and last fake energy float values, print the difference
                        if (lastValuesSet)
                        {
                            float fakeEnergyLostInLastDay = fakeFloatLastStoredEnergy - fakeFloatStoredEnergy;
                            Log.Message($"Fake float: {fakeFloatLastStoredEnergy}Wd - {fakeFloatStoredEnergy}Wd = {fakeEnergyLostInLastDay}Wd");
                        }

                        // if we have the current and last fake energy decimal values, print the difference
                        if (lastValuesSet)
                        {
                            decimal fakeEnergyLostInLastDay = fakeDecimalLastStoredEnergy - fakeDecimalStoredEnergy;
                            Log.Message($"Fake decimal: {fakeDecimalLastStoredEnergy}Wd - {fakeDecimalStoredEnergy}Wd = {fakeEnergyLostInLastDay}Wd");
                        }

                        // hold on to this iteration's energy levels for comparison next execution
                        lastStoredEnergy = batteryComp.StoredEnergy;
                        fakeDecimalLastStoredEnergy = fakeDecimalStoredEnergy;
                        fakeFloatLastStoredEnergy = fakeFloatStoredEnergy;
                        lastValuesSet = true;
                    }

                    // simulate discharge for the fake batteries; the real one will obviously be handled by the game itself
                    if (fakeStoredEnergySet)
                    {
                        fakeDecimalStoredEnergy -= 5m * 1.66666669E-05m;
                        fakeFloatStoredEnergy -= 5f * CompPower.WattsToWattDaysPerTick;
                    }
                }
            }
        }

        //private void SimulateBatterySelfDischarge()
        //{
        //    float batteryStartingEnergyFloat = 600f;
        //    float energyFloat = batteryStartingEnergyFloat;

        //    decimal batteryStartingEnergyDecimal = 600m;
        //    decimal energyDecimal = batteryStartingEnergyDecimal;
        //    const decimal WattsToWattDaysPerTickDecimal = 1.66666669E-05m; // same value as CompPower.WattsToWattDaysPerTick, but as decimal

        //    int ticksInOneDay = 60000;

        //    for (var i = 0; i < ticksInOneDay; i++)
        //    {
        //        energyFloat -= 5f * CompPower.WattsToWattDaysPerTick;
        //        energyDecimal -= 5m * WattsToWattDaysPerTickDecimal;
        //    }

        //    Log.Message("After one simulated day (60,000 iterations):");
        //    Log.Message("Self-discharge when using float: " + (batteryStartingEnergyFloat - energyFloat));
        //    Log.Message("Self-discharge when using decimal: " + (batteryStartingEnergyDecimal - energyDecimal));

        //    Log.Message("Without the loop, float: " + (5f * CompPower.WattsToWattDaysPerTick * ticksInOneDay));
        //    Log.Message("Without the loop, decimal: " + (5m * WattsToWattDaysPerTickDecimal * ticksInOneDay));
        //}
    }
}
