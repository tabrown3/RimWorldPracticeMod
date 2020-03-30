using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class AddOperatorListComponent
    {
        private readonly List<string> allTrackerNames;

        private readonly List<AddOperatorRowComponent> rows = new List<AddOperatorRowComponent>();

        public List<Type> Operators { get; } = new List<Type>();
        public List<SourceMetric> SourceMetrics { get; } = new List<SourceMetric>();

        public AddOperatorListComponent(List<SourceMetric> allSourceMetrics, List<Type> allOperatorTypes)
        {
            allTrackerNames = allSourceMetrics.Select(u => u.ParentName).Distinct().ToList();

            rows.Add(new AddOperatorRowComponent(allOperatorTypes, allTrackerNames, allSourceMetrics));
        }

        public Rect Draw(Rect inRect)
        {
            return new RectStacker(inRect).ThenForEach(rows, (u, row, ind) => row.Draw(u)).GetRect();
        }
    }
}
