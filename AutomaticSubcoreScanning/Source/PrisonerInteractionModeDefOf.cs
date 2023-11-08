using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomzie.AutomaticSubcoreScanning
{
    [DefOf]
    public class PrisonerInteractionModeDefOf
    {
        [MayRequireBiotech]
        public static PrisonerInteractionModeDef RipscanPrisoner;
        [MayRequireBiotech]
        public static PrisonerInteractionModeDef SoftscanPrisoner;
    }
}
