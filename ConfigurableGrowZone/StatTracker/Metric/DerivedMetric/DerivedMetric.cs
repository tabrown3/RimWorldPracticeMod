using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ConfigurableGrowZone
{
    public class DerivedMetric : IMetric
    {
        public string Key { get; }
        public string Name { get; }
        public string Unit { get; }
        public TimeDomain Domain { get; }
        public event EventHandler<DataPointEventArgs> ValuePushed;

        public List<SourceMetric> Sources { get; }
        public List<IOperator<float>> Operators { get; }


        public DerivedMetric(string key, string name, List<SourceMetric> sources, List<IOperator<float>> operators)
        {
            Key = sources[0].Key + "." + key;
            Name = name;
            Unit = sources[0].Unit;
            Domain = sources[0].Domain;

            Sources = sources;
            Operators = operators;
        }

        public virtual void Tick(int gameTick, Dictionary<string, DataVolume> history)
        {
            if(Domain.IsResolutionBoundary(gameTick))
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
                for(var i = 0; i < Operators.Count; i++)
                {
                    var op = Operators[i];
                    if (op is UnaryOperator<float>)
                    {
                        var unaryOp = op as UnaryOperator<float>; // TODO: see if I can prevent having to do this cast every time
                        unaryOp.First = runningValue;
                        runningValue = unaryOp.Call();
                    }
                    else if(op is BinaryOperator<float>)
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

                ValuePushed.Invoke(this, new DataPointEventArgs(new DataPoint(gameTick, runningValue)));
            }
        }
    }
}
