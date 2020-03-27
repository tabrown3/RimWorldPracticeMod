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

            DomainTypes.ForEach(u => Log.Message($"From DomainTypes: {u.Name}"));
            SourceTypes.ForEach(u => Log.Message($"From SourceTypes: {u.Name}"));
            AggregatorTypes.ForEach(u => Log.Message($"From AggregatorTypes: {u.Name}"));
            OperatorTypes.ForEach(u => Log.Message($"From OperatorTypes: {u.Name}"));
        }
    }
}
