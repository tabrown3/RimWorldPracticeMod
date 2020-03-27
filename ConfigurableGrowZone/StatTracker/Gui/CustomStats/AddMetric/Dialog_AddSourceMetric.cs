using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class Dialog_AddSourceMetric : Window
    {
        private string name = "";
        private string key = "";

        private List<Type> domains;
        private List<Type> sources;
        private List<Type> aggregators;

        public Dialog_AddSourceMetric(List<Type> domains, List<Type> sources, List<Type> aggregators)
        {
            this.domains = domains;
            this.sources = sources;
            this.aggregators = aggregators;

            this.closeOnClickedOutside = true;
            this.doCloseX = true;
            this.focusWhenOpened = true;
            this.absorbInputAroundWindow = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            float textInputHeight = 30f;

            new RectStacker(inRect).Then(u =>
            {
                Rect nameEntryRect = new Rect(u);
                nameEntryRect.height = textInputHeight;
                nameEntryRect.width = 200f;
                name = Widgets.TextEntryLabeled(nameEntryRect, "Name", name);

                return nameEntryRect;
            })
            .ThenGap(15f)
            .Then(u =>
            {
                Rect keyEntryRect = new Rect(u);
                keyEntryRect.height = textInputHeight;
                keyEntryRect.width = 200f;
                key = Widgets.TextEntryLabeled(keyEntryRect, "Key", key);

                return keyEntryRect;
            })
            .ThenGap(15f)
            .Then(u =>
            {
                return new RectStacker(u).Then(v =>
                {
                    Rect radioButtonRect = new Rect(v);
                    radioButtonRect.width = 100f;
                    radioButtonRect.height = 20f;
                    if(Widgets.RadioButtonLabeled(radioButtonRect, "Poll", false))
                    {

                    }

                    return radioButtonRect;
                })
                .ThenGap(10f)
                .Then(v =>
                {
                    Rect radioButtonRect = new Rect(v);
                    radioButtonRect.width = 100f;
                    radioButtonRect.height = 20f;
                    if(Widgets.RadioButtonLabeled(radioButtonRect, "Digest", false))
                    {

                    }

                    return radioButtonRect;
                })
                .ThenGap(10f)
                .Then(v =>
                {
                    Rect radioButtonRect = new Rect(v);
                    radioButtonRect.width = 100f;
                    radioButtonRect.height = 20f;
                    if(Widgets.RadioButtonLabeled(radioButtonRect, "Window", false))
                    {

                    }

                    return radioButtonRect;
                });
            })
            .ThenGap(15f)
            .Then(u =>
            {
                Rect textButtonRect = new Rect(u);
                textButtonRect.width = 100f;
                textButtonRect.height = 35f;
                if(Widgets.ButtonText(textButtonRect, "Domain"))
                {
                    List<FloatMenuOption> list = domains.Select(v => new FloatMenuOption(v.FullName, delegate
                    {
                        Log.Message($"Chosed domain: {v.FullName}");
                    })).ToList();

                    Find.WindowStack.Add(new FloatMenu(list));
                }

                return textButtonRect;
            })
            .Then(u =>
            {
                Rect textButtonRect = new Rect(u);
                textButtonRect.width = 100f;
                textButtonRect.height = 35f;
                if (Widgets.ButtonText(textButtonRect, "Source"))
                {
                    List<FloatMenuOption> list = sources.Select(v => new FloatMenuOption(v.FullName, delegate
                    {
                        Log.Message($"Chosed source: {v.FullName}");
                    })).ToList();

                    Find.WindowStack.Add(new FloatMenu(list));
                }

                return textButtonRect;
            })
            .Then(u =>
            {
                Rect textButtonRect = new Rect(u);
                textButtonRect.width = 100f;
                textButtonRect.height = 35f;
                if (Widgets.ButtonText(textButtonRect, "Aggregator"))
                {
                    List<FloatMenuOption> list = aggregators.Select(v => new FloatMenuOption(v.FullName, delegate
                    {
                        Log.Message($"Chosed aggregator: {v.FullName}");
                    })).ToList();

                    Find.WindowStack.Add(new FloatMenu(list));
                }

                return textButtonRect;
            });
        }
    }
}
