using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using Verse;

namespace ConfigurableGrowZone
{
    public class DerivedMetric : IMetric
    {
        private readonly Subject<DataPoint> valuePushed = new Subject<DataPoint>();
        private readonly Dictionary<string, DataVolume> history;

        public string ParentName { get; }
        public string Key { get; }
        public string Name { get; }
        public string Unit { get; }
        public TimeDomain Domain { get; }
        public IObservable<DataPoint> ValuePushed => Observable.Concat(RetroactivelyDerivedHistoricalData(), valuePushed);

        public List<SourceMetric> Sources { get; }
        public List<IOperator<float>> Operators { get; }


        public DerivedMetric(string parentName, string key, string name, List<SourceMetric> sources, List<IOperator<float>> operators, Dictionary<string, DataVolume> history)
        {
            var anchorMetric = sources[0];

            ParentName = parentName;
            Key = anchorMetric.Key + "." + key;
            Name = name;
            Unit = anchorMetric.Unit;
            Domain = anchorMetric.Domain;

            Sources = sources;
            Operators = operators;

            this.history = history;
        }

        public virtual void Tick(int gameTick)
        {
            if (Domain.IsResolutionBoundary(gameTick))
            {
                float dataPointValue = TickInt(gameTick);
                valuePushed.OnNext(new DataPoint(gameTick, dataPointValue));
            }
        }

        private float TickInt(int gameTick)
        {
            var argumentList = new List<float>();
            foreach (var source in Sources)
            {
                if (history.ContainsKey(source.Key))
                {
                    // TODO: refactor to make this not garbage
                    argumentList.Add(history[source.Key].DataPoints.Single(u => u.TimeStampGameTicks == gameTick).Value);
                }
                else
                {
                    argumentList.Add(0f);
                }
            }

            float runningValue = argumentList[0];
            int argNum = 1;
            for (var i = 0; i < Operators.Count; i++)
            {
                var op = Operators[i];
                if (op is UnaryOperator<float>)
                {
                    var unaryOp = op as UnaryOperator<float>; // TODO: see if I can prevent having to do this cast every time
                    unaryOp.First = runningValue;
                    runningValue = unaryOp.Call();
                }
                else if (op is BinaryOperator<float>)
                {
                    var binaryOp = op as BinaryOperator<float>; // TODO: see if I can prevent having to do this cast every time
                    binaryOp.First = runningValue;
                    binaryOp.Second = argumentList[argNum];
                    runningValue = binaryOp.Call();

                    argNum++;
                }
                else
                {
                    throw new Exception("Operator must be of type UnaryOperator<float> or BinaryOperator<float>");
                }
            }

            return runningValue;
        }

        // This method returns an observable that will emit to completion immediately after subscription;
        //  it passes historical data through the derivation metric and emits it as if it were occurring in real-time
        public IObservable<DataPoint> RetroactivelyDerivedHistoricalData()
        {
            var anchorMetric = Sources[0];

            return Observable.Start(() =>
            {
                return history[anchorMetric.Key].DataPoints
                .Select(u => new DataPoint(u.TimeStampGameTicks, TickInt(u.TimeStampGameTicks)));
            }).SelectMany(u => u);
        }
    }
}
