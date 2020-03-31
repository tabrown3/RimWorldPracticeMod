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
    public class AddOperatorListComponent : IValidatable
    {
        private readonly List<string> allTrackerNames;
        private readonly List<AddOperatorRowComponent> rows = new List<AddOperatorRowComponent>();

        private List<AddOperatorRowComponent> TopRows => rows.Take(rows.Count - 1).ToList();
        private AddOperatorRowComponent BottomRow => rows.Count > 0 ? rows.Last() : null;
        private IObservable<bool> rowsBecameValid;

        public List<Type> Operators
        {
            get
            {
                var operators = rows.Select(u => u.Model.ChosenOperator).ToList();
                if (BottomRow.IsEmpty())
                {
                    operators.Pop();
                }

                return operators;
            }
        }

        public List<SourceMetric> SourceMetrics
        {
            get
            {
                var sourceMetrics = rows.Select(u => u.Model.ChosenSourceMetric).ToList();
                if (BottomRow.IsEmpty())
                {
                    sourceMetrics.Pop();
                }

                return sourceMetrics;
            }
        }

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

        public bool IsValid()
        {
            if(BottomRow.IsEmpty())
            {
                return TopRows.All(u => u.IsValid());
            }
            else
            {
                return rows.All(u => u.IsValid());
            }
        }

        private void AddRow(List<Type> allOperatorTypes, List<string> allTrackerNames, List<SourceMetric> allSourceMetrics)
        {
            var newRow = new AddOperatorRowComponent(allOperatorTypes, allTrackerNames, allSourceMetrics);
            rows.Add(newRow);

            rowsBecameValid = RowsBecameValidFactory();
            rowsBecameValid.Subscribe(u =>
            {
                AddRow(allOperatorTypes, allTrackerNames, allSourceMetrics);
            });
        }

        private IObservable<bool> RowsBecameValidFactory()
        {
            bool wasValid = false; // can start false because after a new row is added, it will always be invalid

            // observable that emits when the list's rows go from "not all being valid" to "all being valid"
            return rows.ToObservable()
                .SelectMany(u => u.RowBecameValid)
                .Select(u => rows.All(v => v.IsValid()))
                .Where(isValid =>
                {
                    bool validityStateChanged = isValid != wasValid;
                    wasValid = isValid;
                    return validityStateChanged && isValid;
                }
            );
        }
    }
}
