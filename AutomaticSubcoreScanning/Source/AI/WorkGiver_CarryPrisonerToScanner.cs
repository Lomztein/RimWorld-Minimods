using RimWorld;
using System.Linq;
using Verse.AI;
using Verse;

namespace Lomzie.AutomaticSubcoreScanning.AI
{
    public class WorkGiver_CarryPrisonerToScanner : WorkGiver_Warden
    {
        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            return base.ShouldSkip(pawn, forced) && !pawn.Map.mapPawns.PrisonersOfColony.Any(x => x.guest.interactionMode == PrisonerInteractionModeDefOf.RipscanPrisoner);
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!ShouldTakeCareOfPrisoner(pawn, t))
            {
                return null;
            }
            Pawn pawn2 = (Pawn)t;
            Building_SubcoreScanner scanner = null;

            if (pawn2.guest.interactionMode == PrisonerInteractionModeDefOf.RipscanPrisoner)
                scanner = FindScannerUtils.FindValidScanners(pawn2, FindScannerUtils.ScanType.Ripscan).FirstOrDefault();
            if (pawn2.guest.interactionMode == PrisonerInteractionModeDefOf.SoftscanPrisoner)
                scanner = FindScannerUtils.FindValidScanners(pawn2, FindScannerUtils.ScanType.Softscan).FirstOrDefault();

            if (scanner != null)
            {
                Job job = JobMaker.MakeJob(JobDefOf.CarryToBuilding, scanner, pawn2);
                job.count = 1;
                return job;
            }

            return null;
        }
    }
}
