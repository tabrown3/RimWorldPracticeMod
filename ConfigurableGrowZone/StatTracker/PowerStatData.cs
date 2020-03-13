using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class PowerStatData
    {
        private readonly Vector2 latLong;
        public PowerStatData(Vector2 latLong)
        {
            this.latLong = latLong;
        }

        public readonly List<DigestStatMetric> Metrics = new List<DigestStatMetric>();
        //public readonly Dictionary<string, Dictionary<int, DataPoint>> History = new Dictionary<string, Dictionary<int, DataPoint>>();
        public readonly StatHistory History = new StatHistory();

        public void AddMetric(DigestStatMetric metric)
        {
            metric.OnDigest += (o, ev) => {

                var dataPoint = ev.DataPoint;
                
                if(!History.ContainsKey(metric.Key)) // if first datapoint for this metric
                {
                    //History[dataPoint.Key] = new Dictionary<int, DataPoint>();
                    History.CreateVolume(metric.Key, new DataVolume(metric.Key, metric.Name, metric.Unit, metric.Resolution, latLong));
                }

                History.Save(metric.Key, dataPoint);

                /*** Below line throwing errors for some reason ***/
                //OnHistoryAdded.Invoke(this, ev);
                /*** Above line throwing errors for some reason ***/
            };

            this.Metrics.Add(metric);
        }
    }
}
