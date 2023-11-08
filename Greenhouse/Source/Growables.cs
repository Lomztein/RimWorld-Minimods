using RimWorld;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Verse;

namespace Lomzie.Greenhouse
{
    // I used the code from Mines 2.0 as template. Credit to anglewyrm for the original.
    // Template: https://github.com/AngleWyrm/Mines/blob/master/Source/Mines/Mineables.cs

    [StaticConstructorOnStartup]
    internal static class Greenhouse_Initializer
    {
        static Greenhouse_Initializer()
        {
            LongEventHandler.QueueLongEvent(Setup, "LibraryStartup", false, null);
        }

        /* Look for mineable resources and add them to the Mine */
        public static void Setup()
        {

            // localized title-case capitalization class
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;  // usage: textInfo.ToTitleCase(string)

            List<RecipeDef> RecipeDefs = DefDatabase<RecipeDef>.AllDefsListForReading;
            List<ThingDef> ThingDefs = DefDatabase<ThingDef>.AllDefsListForReading;

            // scan all the things
            for (int someThing = 0; someThing < ThingDefs.Count; someThing++)
            {
                var def = ThingDefs[someThing];
                var plant = def.plant;
                if (plant != null && plant.Sowable && !plant.IsTree && plant.harvestYield > 0)
                {
                    // Create recipe
                    RecipeDef recipe = new RecipeDef();

                    recipe.defName = "Grow" + ThingDefs[someThing].defName;

                    int yieldPerCraft = (int)Math.Min(plant.harvestYield, 5);
                    recipe.label = "Grow".Translate(yieldPerCraft.ToString(), ThingDefs[someThing].label);
                    recipe.description = "Grow".Translate(yieldPerCraft.ToString(), ThingDefs[someThing].label, ".");
                    recipe.jobString = "Growing".Translate(yieldPerCraft.ToString(), ThingDefs[someThing].label);

                    int area = 5 * 8;
                    float prodPerDay = area * (plant.harvestYield / plant.growDays);
                    float productivityMultiplier = GreenhouseMod.Settings.ProductivityMultipliier;
                    int ticksPerDay = 60000;

                    // Compute work amount. Use sow work + harvest work
                    int workAmount = (int)(ticksPerDay / (prodPerDay / yieldPerCraft) / productivityMultiplier) + (int)(plant.harvestWork + plant.sowWork);
                    Log.Message($"Found growable {def.defName} with work amount {workAmount}.");

                    recipe.effectWorking = EffecterDefOf.Harvest_Plant;
                    recipe.efficiencyStat = StatDefOf.PlantWorkSpeed;
                    recipe.workAmount = workAmount;
                    recipe.workSkill = SkillDefOf.Plants;
                    recipe.workSkillLearnFactor = 0.25f;
                    recipe.skillRequirements = new List<SkillRequirement>() { new SkillRequirement()
                    {
                        minLevel = plant.sowMinSkill,
                        skill = SkillDefOf.Plants,
                    } };

                    if (plant.sowResearchPrerequisites != null)
                        recipe.researchPrerequisites = new List<ResearchProjectDef>(plant.sowResearchPrerequisites);

                    recipe.products.Add(new ThingDefCountClass(plant.harvestedThingDef, yieldPerCraft));
                    recipe.fixedIngredientFilter = new ThingFilter();
                    recipe.defaultIngredientFilter = new ThingFilter();

                    // add bill to greenhouses
                    recipe.recipeUsers = new List<ThingDef>();
                    recipe.recipeUsers.Add(ThingDef.Named("FueledGreenhouse"));
                    recipe.recipeUsers.Add(ThingDef.Named("ElectricGreenhouse"));


                    RecipeDefs.Add(recipe);
                }
            }
        }
    }

}
