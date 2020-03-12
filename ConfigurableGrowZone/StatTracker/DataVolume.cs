using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class DataVolume
    {
        public DataVolume(string key, string name, string unit, GameTime.InTicks resolution, Vector2 latLong)
        {
            Key = key;
            Name = name;
            Unit = unit;
            Resolution = resolution;
            LatLong = latLong;
        }
        public string Key { get; }
        public string Name { get; }
        public string Unit { get; }
        public GameTime.InTicks Resolution { get; }
        public Vector2 LatLong { get; } // Latitude and longitude this data was sampled at; useful for generating dates
        public List<DataPoint> DataPoints { get; } = new List<DataPoint>().ToList();
    }
}
