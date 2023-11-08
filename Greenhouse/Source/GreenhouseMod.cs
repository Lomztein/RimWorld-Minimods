using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Lomzie.Greenhouse
{
    public class GreenhouseMod : Mod
    {
        public static GreenhouseSettings Settings { get; private set; }

        public GreenhouseMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<GreenhouseSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label($"Greenhouse productivity multiplier (requires restart): {Settings.ProductivityMultipliier}");
            Settings.ProductivityMultipliier = listingStandard.Slider(Settings.ProductivityMultipliier, 0.1f, 5f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Greenhouse";
        }

        public class GreenhouseSettings : ModSettings
        {
            public float ProductivityMultipliier = 0.75f;

            public override void ExposeData()
            {
                Scribe_Values.Look(ref ProductivityMultipliier, "productivityMultiplier");
                base.ExposeData();
            }
        }
    }
}
