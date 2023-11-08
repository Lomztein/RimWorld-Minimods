using Lomzie.AutomaticSubcoreScanning;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace Lomzie.AutomaticSubcoreScanning.AI
{
    public class WorkGiver_EnterScanner : WorkGiver_Scanner
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
            => FindScannerUtils.FindValidScanners(pawn, FindScannerUtils.ScanType.Softscan);

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Job job = JobMaker.MakeJob(JobDefOf.EnterBuilding, t);
            job.count = 1;
            return job;
        }
    }
}
