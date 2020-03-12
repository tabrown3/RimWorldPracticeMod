using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ConfigurableGrowZone
{
    public class StatHistory
    {
        private readonly Dictionary<string, DataVolume> history = new Dictionary<string, DataVolume>();

        public void Save(string key, DataPoint dataPoint)
        {
            if(!ContainsKey(key))
            {
                Log.Error($"Cannot save to a key that doesn't exist; use {nameof(CreateVolume)} first");
            }
            else
            {
                history[key].DataPoints.Add(dataPoint);
            }
        }

        public bool ContainsKey(string key)
        {
            return history.ContainsKey(key);
        }

        public void CreateVolume(string key, DataVolume dataVolume)
        {
            if(ContainsKey(key))
            {
                Log.Error($"Data volume with key {key} already exists; use {nameof(Save)} to add {nameof(DataPoint)}");
            }
            else
            {
                history[key] = dataVolume;
            }
        }

        public DataVolume Get(string key)
        {
            if (!ContainsKey(key))
            {
                Log.Error($"Key {key} does not exist; use {nameof(CreateVolume)} first");
                return null;
            }

            return history[key];
        }
    }
}
