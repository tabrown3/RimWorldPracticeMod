using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class AddOperatorListComponent
    {
        private readonly List<string> allTrackerNames;
        private readonly List<AddOperatorRowComponent> rows = new List<AddOperatorRowComponent>();
        //private AddOperatorRowComponent bottomRow => rows.Count > 0 ? rows[rows.Count - 1] : null;
        private IObservable<bool> rowsBecameValid;

        public List<Type> Operators { get; } = new List<Type>();
        public List<SourceMetric> SourceMetrics { get; } = new List<SourceMetric>();

        public AddOperatorListComponent(List<SourceMetric> allSourceMetrics, List<Type> allOperatorTypes)
        {
            allTrackerNames = allSourceMetrics.Select(u => u.ParentName).Distinct().ToList();

            AddRow(allOperatorTypes, allTrackerNames, allSourceMetrics);
        }

        public Rect Draw(Rect inRect)
        {
            return new RectStacker(inRect)
                .Then(u =>
                {
                    return new RectSpanner(u)
                        .Then(v => StatWidgets.DrawSectionHeader(v, "Operator"))
                        .ThenGap(50f)
                        .Then(v => StatWidgets.DrawSectionHeader(v, "Tracker"))
                        .ThenGap(50f)
                        .Then(v => StatWidgets.DrawSectionHeader(v, "Metric"))
                        .GetRect();
                })
                .ThenForEach(rows, (u, row, ind) => row.Draw(u)).GetRect();
        }

        private void AddRow(List<Type> allOperatorTypes, List<string> allTrackerNames, List<SourceMetric> allSourceMetrics)
        {
            var newRow = new AddOperatorRowComponent(allOperatorTypes, allTrackerNames, allSourceMetrics);
            rows.Add(newRow);

            rowsBecameValid = RowsBecameValidFactory();
            rowsBecameValid.Subscribe(u =>
            {
                AddRow(allOperatorTypes, allTrackerNames, allSourceMetrics);
                Log.Message("Rows all valid!");
            });
        }

        private IObservable<bool> RowsBecameValidFactory()
        {
            bool lastValidityState = false; // can start false because after a new row is added, it will always be invalid

            // observable that emits when the list's rows go from "not all being valid" to "all being valid"
            return rows.ToObservable()
                .SelectMany(u => u.RowBecameValid)
                .Select(u => rows.All(v => v.IsValid()))
                .Where(curValidityState =>
                {
                    bool validityStateChanged = curValidityState != lastValidityState;
                    lastValidityState = curValidityState;
                    return validityStateChanged && curValidityState;
                }
            );
        }
    }
}
