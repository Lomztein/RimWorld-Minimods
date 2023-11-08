using RimWorld;
using System.ComponentModel;
using UnityEngine;
using Verse;

namespace Lomzie.PsychicSensitivityBandwidth
{
    // I imagine there is a better way of implementing this, but I don't know how so here we are.
    public class StatPart_PsychicSensitivity : StatPart
    {
        private float GetModifier(Pawn pawn)
        {
            float multiplier = PsychicSensitivityBandwidthMod.Settings.BandwidthMultiplier;
            float sensitivity = pawn.GetStatValue(StatDefOf.PsychicSensitivity);
            float delta = (sensitivity - 1f) * multiplier;
            return Mathf.Max(1f + delta, 0f);
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing && req.Thing is Pawn pawn)
            {
                return "Psychic sensitivity: x" + GetModifier(pawn).ToStringPercent();
            }
            return null;
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.HasThing && req.Thing is Pawn pawn)
            {
                val = Mathf.RoundToInt(val * GetModifier(pawn));
            }
        }
    }
}