using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class DerivedMetric<T> : IMetric
    {
        public string Key { get; }
        public string Name { get; }
        public string Unit { get; }
        public TimeDomain Domain { get; }

        public List<SourceMetric> Sources = new List<SourceMetric>();
        public List<IOperator<T>> Operators = new List<IOperator<T>>();

        public DerivedMetric(string key, string name, SourceMetric sourceMetric)
        {
            Key = key;
            Name = name;
            Unit = sourceMetric.Unit;
            Domain = sourceMetric.Domain;

            Sources.Add(sourceMetric);
        }

        public void AddSource(SourceMetric metric)
        {
            Sources.Add(metric);
        }

        public void AddOperator(IOperator<T> inOperator)
        {
            if (inOperator is UnaryOperator<T>)
            {

            }
            else if (inOperator is BinaryOperator<T>)
            {

            }
            else
            {
                throw new Exception("You didn't pass in a unary or binary operator; what did you pass in?");
            }
        }
    }
}
