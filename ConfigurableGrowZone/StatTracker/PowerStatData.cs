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

        public readonly List<StatMetric> Metrics = new List<StatMetric>();
        public readonly StatHistory History = new StatHistory();

        public void AddMetric(StatMetric metric)
        {
            metric.ValuePushed += (o, ev) => {

                var dataPoint = ev.DataPoint;
                
                if(!History.ContainsKey(metric.Key)) // if first datapoint for this metric
                {
                    History.CreateVolume(metric.Key, new DataVolume(metric.Key, metric.Name, metric.Unit, metric.Domain, latLong));
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
