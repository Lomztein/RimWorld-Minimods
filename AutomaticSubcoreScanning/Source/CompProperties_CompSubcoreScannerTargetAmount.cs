using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Lomzie.AutomaticSubcoreScanning
{
    public class CompProperties_CompSubcoreScannerTargetAmount : CompProperties
    {
        public CompProperties_CompSubcoreScannerTargetAmount()
        {
            compClass = typeof(CompSubcoreScannerTargetAmount);
        }
    }
}
