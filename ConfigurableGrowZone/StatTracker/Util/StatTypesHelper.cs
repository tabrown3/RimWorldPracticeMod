using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    [StaticConstructorOnStartup]
    public static class StatTypesHelper
    {
        public static List<Type> DomainTypes { get; }
        public static List<Type> SourceTypes { get; }
        public static List<Type> AggregatorTypes { get; }
        public static List<Type> OperatorTypes { get; }
        static StatTypesHelper()
        {
            DomainTypes = GenTypes.AllTypes.Where(u => typeof(Domain).IsAssignableFrom(u) && u.IsClass && !u.IsAbstract).ToList();
            SourceTypes = GenTypes.AllTypes.Where(u => typeof(IPullable<float>).IsAssignableFrom(u) && u.IsClass && !u.IsAbstract).ToList();
            AggregatorTypes = GenTypes.AllTypes.Where(u => typeof(IAggregator<float>).IsAssignableFrom(u) && u.IsClass && !u.IsAbstract).ToList();
            OperatorTypes = GenTypes.AllTypes.Where(u => typeof(IOperator<float>).IsAssignableFrom(u) && u.IsClass && !u.IsAbstract).ToList();
        }

        public static bool IsSetMetric(Type inType)
        {
            return inType == typeof(DigestSourceMetric) || inType == typeof(WindowSourceMetric);
        }

        public static bool IsUnaryOperator(Type inType)
        {
            // TODO: this would benefit from a cache
            return typeof(UnaryOperator<float>).IsAssignableFrom(inType);
        }
    }
}
