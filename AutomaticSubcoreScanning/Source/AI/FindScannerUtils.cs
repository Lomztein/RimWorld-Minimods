using Lomzie.AutomaticSubcoreScanning;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Lomzie.AutomaticSubcoreScanning.AI
{
    public class FindScannerUtils
    {
        public enum ScanType { Softscan, Ripscan, Any }

        public static IEnumerable<Building_SubcoreScanner>FindValidScanners(Pawn pawn, ScanType scanType)
        {
            var scanners = pawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_SubcoreScanner>();
            scanners = scanners.Where(x => x.CanAcceptPawn(pawn) && x.AllRequiredIngredientsLoaded);

            foreach (var scanner in scanners)
            {
                if (scanType == ScanType.Softscan && scanner.DestroyOccupantBrain)
                    continue; // Safety measure, just in case.

                if (scanType == ScanType.Ripscan && !scanner.DestroyOccupantBrain)
                    continue;

                CompSubcoreScannerTargetAmount comp = scanner.GetComp<CompSubcoreScannerTargetAmount>();

                // Suboptimal to do this in loop if there are multiple of each scanner on the map.
                int currentCores = CurrentSubcoreAmountIncludingInProgress(pawn.Map, scanner.def.building.subcoreScannerOutputDef, scanner.DestroyOccupantBrain);

                if (pawn.IsColonist && comp.AutoAllowColonists && comp.TargetAmount > currentCores)
                {
                    yield return scanner;
                }
                if (pawn.IsPrisoner && comp.AutoAllowPrisoners && comp.TargetAmount > currentCores)
                {
                    yield return scanner;
                }
            }
        }

        public static int CurrentSubcoreAmountIncludingInProgress(Map map, ThingDef subcoreDef, bool destroyBrain)
        {
            var scanners = map.listerBuildings.AllBuildingsColonistOfClass<Building_SubcoreScanner>();
            return map.listerThings.ThingsOfDef(subcoreDef).Sum(x => x.stackCount) + scanners.Count(x => x.DestroyOccupantBrain == destroyBrain && x.Occupant != null);
        }
    }
}
