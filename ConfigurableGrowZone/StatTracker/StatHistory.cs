using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ConfigurableGrowZone
{
    public class StatHistory
    {
        public Dictionary<string, DataVolume> History { get; set; } = new Dictionary<string, DataVolume>();

        public void Save(string key, DataPoint dataPoint)
        {
            if(!ContainsKey(key))
            {
                Log.Error($"Cannot save to a key that doesn't exist; use {nameof(CreateVolume)} first");
            }
            else
            {
                History[key].DataPoints.Add(dataPoint);
            }
        }

        public bool ContainsKey(string key)
        {
            return History.ContainsKey(key);
        }

        public void CreateVolume(string key, DataVolume dataVolume)
        {
            if(ContainsKey(key))
            {
                Log.Error($"Data volume with key {key} already exists; use {nameof(Save)} to add {nameof(DataPoint)}");
            }
            else
            {
                History[key] = dataVolume;
            }
        }

        public DataVolume Get(string key)
        {
            if (!ContainsKey(key))
            {
                Log.Error($"Key {key} does not exist; use {nameof(CreateVolume)} first");
                return null;
            }

            return History[key];
        }
    }
}
