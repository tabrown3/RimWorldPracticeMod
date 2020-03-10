using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ConfigurableGrowZone
{
    public class PowerStatData
    {
        public readonly List<PowerStatMetric> Metrics = new List<PowerStatMetric>();
        //public event EventHandler<DataPointEventArgs> OnHistoryAdded; // useful for real-time if I get to that point
        public readonly Dictionary<string, Dictionary<int, DataPoint>> History = new Dictionary<string, Dictionary<int, DataPoint>>();


        public void AddMetric(PowerStatMetric metric)
        {
            metric.OnDigest += (o, ev) => {

                var dataPoint = ev.DataPoint;
                
                if(!History.ContainsKey(dataPoint.Key)) // if first datapoint for this metric
                {
                    History[dataPoint.Key] = new Dictionary<int, DataPoint>();
                }

                History[dataPoint.Key][dataPoint.TimeStamp] = dataPoint;

                //foreach(var metricKey in History.Keys)
                //{
                //    var dataDict = History[metricKey];
                //    foreach(var timeStamp in dataDict.Keys)
                //    {
                //        var ittyBittyPoint = dataDict[timeStamp];
                //        Log.Message($"{ittyBittyPoint.Key} at {ittyBittyPoint.TimeStamp}: {ittyBittyPoint.DigestValue}");
                //    }
                //}

                /*** Below line throwing errors for some reason ***/
                //OnHistoryAdded.Invoke(this, ev);
                /*** Above line throwing errors for some reason ***/
            };

            this.Metrics.Add(metric);
        }
    }
}
