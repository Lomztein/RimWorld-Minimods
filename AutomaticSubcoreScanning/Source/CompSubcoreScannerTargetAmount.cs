using Lomzie.AutomaticSubcoreScanning.AI;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Lomzie.AutomaticSubcoreScanning
{
    public class CompSubcoreScannerTargetAmount : ThingComp
    {
        public Building_SubcoreScanner Parent => (Building_SubcoreScanner)parent;
        public ThingDef ParentSubcoreDef => Parent.def.building.subcoreScannerOutputDef;

        public int TargetAmount = 1;
        public bool AutoLoad = false;
        public bool AutoAllowColonists = false;
        public bool AutoAllowPrisoners = false;

        private int _ticks;
        private const int CHECK_TICKS = 60 * 42 * 3; // About every three in-game hours.

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Command_Toggle command_Toggle = new Command_Toggle();
            command_Toggle.defaultLabel = "SubcoreScannerAutoLoad".Translate();
            command_Toggle.defaultDesc = "SubcoreScannerAutoLoadDesc".Translate();
            command_Toggle.icon = (AutoLoad ? TexCommand.ForbidOff : TexCommand.ForbidOn);
            command_Toggle.isActive = () => AutoLoad;
            command_Toggle.toggleAction = delegate
            {
                AutoLoad = !AutoLoad;
            };
            command_Toggle.activateSound = SoundDefOf.Tick_Tiny;
            yield return command_Toggle;

            if (AutoLoad) {
                Command_Action decrement_command = new Command_Action();
                decrement_command.defaultLabel = "SubcoreTargetAmountChange".Translate("-1");
                decrement_command.icon = ContentFinder<Texture2D>.Get("SubtractCommand");            
                decrement_command.action = () =>
                {
                    TargetAmount--;
                };
                decrement_command.activateSound = SoundDefOf.Tick_Tiny;
                yield return decrement_command;

                Command_Action increment_command = new Command_Action();
                increment_command.defaultLabel = "SubcoreTargetAmountChange".Translate("+1");
                increment_command.icon = ContentFinder<Texture2D>.Get("AddCommand");            
                increment_command.action = () =>
                {
                    TargetAmount++;
                };
                increment_command.activateSound = SoundDefOf.Tick_Tiny;
                yield return increment_command;

                if (!Parent.DestroyOccupantBrain)
                {
                    Command_Toggle colonists_Toggle = new Command_Toggle();
                    colonists_Toggle.defaultLabel = "SubcoreScannerAutoAllowColonists".Translate();
                    colonists_Toggle.defaultDesc = "SubcoreScannerAutoAllowColonistsDesc".Translate();
                    colonists_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Commands/ForColonists");
                    colonists_Toggle.isActive = () => AutoAllowColonists;
                    colonists_Toggle.toggleAction = delegate
                    {
                        AutoAllowColonists = !AutoAllowColonists;
                    };
                    colonists_Toggle.activateSound = SoundDefOf.Tick_Tiny;
                    yield return colonists_Toggle;
                }

                Command_Toggle prisoner_Toggle = new Command_Toggle();
                prisoner_Toggle.defaultLabel = "SubcoreScannerAutoAllowPrisoners".Translate();
                prisoner_Toggle.defaultDesc = "SubcoreScannerAutoAllowPrisonersDesc".Translate(Parent.DestroyOccupantBrain ? PrisonerInteractionModeDefOf.RipscanPrisoner.LabelCap : PrisonerInteractionModeDefOf.SoftscanPrisoner.LabelCap);
                prisoner_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners");
                prisoner_Toggle.isActive = () => AutoAllowPrisoners;
                prisoner_Toggle.toggleAction = delegate
                {
                    AutoAllowPrisoners = !AutoAllowPrisoners;
                };
                prisoner_Toggle.activateSound = SoundDefOf.Tick_Tiny;
                yield return prisoner_Toggle;
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref TargetAmount, "targetAmount", 1);
            Scribe_Values.Look(ref AutoLoad, "autoLoad", false);
            Scribe_Values.Look(ref AutoAllowColonists, "autoAllowColonists", false);
            Scribe_Values.Look(ref AutoAllowPrisoners, "autoAllowPrisoners", false);
        }

        public override string CompInspectStringExtra()
        {
            if (AutoLoad)
            {
                int current = FindScannerUtils.CurrentSubcoreAmountIncludingInProgress(Parent.Map, ParentSubcoreDef, Parent.DestroyOccupantBrain);
                return base.CompInspectStringExtra() + "TargetSubcores".Translate($"{current}/{TargetAmount}").ToStringSafe();
            }
            return base.CompInspectStringExtra();
        }

        public override void CompTick()
        {
            if (_ticks++ > CHECK_TICKS && AutoLoad)
            {
                if (parent is Building_SubcoreScanner scanner)
                {
                    if (scanner.State == SubcoreScannerState.Inactive)
                    {
                        int currentCores = parent.Map.listerThings.ThingsOfDef(ParentSubcoreDef).Sum(x => x.stackCount);
                        var scanners = parent.Map.listerBuildings.AllBuildingsColonistOfClass<Building_SubcoreScanner>().Where(x => x.DestroyOccupantBrain == scanner.DestroyOccupantBrain);
                        currentCores += scanners.Count(x => x.Occupant != null);
                        if (currentCores < TargetAmount)
                        {
                            // Scanner init is private, and I can't find any method for initiazing the scanner, so hacky time it is!
                            scanner.GetType().GetField("initScanner", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(scanner, true);
                        }
                    }
                }
                else
                {
                    Log.Error("CompSubcoreScannerTargetAmount should only be attached to a subcore scanner.");
                }
                _ticks = 0;
            }
        }
    }
}
