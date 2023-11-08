using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Lomzie.PsychicSensitivityBandwidth
{
    public class PsychicSensitivityBandwidthMod : Mod
    {
        public static PsychicSensitivityBandwidthSettings Settings { get; private set; }

        public PsychicSensitivityBandwidthMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<PsychicSensitivityBandwidthSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label($"Psychic sensitivity bandwidth multiplier: {Settings.BandwidthMultiplier}");
            Settings.BandwidthMultiplier = listingStandard.Slider(Settings.BandwidthMultiplier, 0.0f, 2f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Psychic Sensitivity Affects Bandwidth";
        }

        public class PsychicSensitivityBandwidthSettings : ModSettings
        {
            public float BandwidthMultiplier = 0.5f;

            public override void ExposeData()
            {
                Scribe_Values.Look(ref BandwidthMultiplier, "bandwidthMultiplier", 0.5f);
                base.ExposeData();
            }
        }
    }
}
