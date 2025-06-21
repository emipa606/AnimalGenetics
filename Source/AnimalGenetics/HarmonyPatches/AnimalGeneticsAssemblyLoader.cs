using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[StaticConstructorOnStartup]
public static class AnimalGeneticsAssemblyLoader
{
    private static readonly List<PawnColumnDef> defaultAnimalsPawnTableDefColumns;
    private static readonly List<PawnColumnDef> defaultWildlifePawnTableDefColumns;
    public static readonly bool ColonyManagerLoaded;

    public static readonly List<Type> GatherableTypes;

    static AnimalGeneticsAssemblyLoader()
    {
        new Harmony("AnimalGenetics").PatchAll(Assembly.GetExecutingAssembly());

        DefDatabase<StatDef>.Add(AnimalGenetics.Damage);
        DefDatabase<StatDef>.Add(AnimalGenetics.Health);
        DefDatabase<StatDef>.Add(AnimalGenetics.GatherYield);

        var affectedStats = Constants.AffectedStatsToInsert;
        foreach (var stat in affectedStats)
        {
            try
            {
                stat.parts?.Insert(0, new StatPart(stat));
            }
            catch
            {
                Log.Error($"[AnimalGenetics]: {stat} is broken");
            }
        }

        var category = new StatCategoryDef
            { defName = "AnimalGenetics_Category", label = "Genetics", displayAllByDefault = true, displayOrder = 200 };
        DefDatabase<StatCategoryDef>.Add(category);
        foreach (var stat in Constants.AffectedStats)
        {
            DefDatabase<StatDef>.Add(new StatDefWrapper
            {
                defName = $"AnimalGenetics_{stat.defName}",
                label = Constants.GetLabel(stat),
                Underlying = stat,
                category = category,
                workerClass = typeof(StatWorker),
                toStringStyle = ToStringStyle.PercentZero
            });
        }

        StatDefOf.MarketValue.parts.Add(new MarketValueCalculator());

        GatherableTypes =
        [
            typeof(CompShearable),
            typeof(CompMilkable),
            typeof(CompEggLayer)
        ];

        // Compatibility patches
        try
        {
            ColonyManagerLoaded = ModLister.GetActiveModWithIdentifier("Fluffy.ColonyManager", true) != null;

            if (ModLister.GetActiveModWithIdentifier("CETeam.CombatExtended", true) != null)
            {
                GatherableTypes.Add(AccessTools.TypeByName("CombatExtended.CompShearableRenameable"));
            }

            if (ModLister.GetActiveModWithIdentifier("OskarPotocki.VanillaFactionsExpanded.Core", true) != null)
            {
                GatherableTypes.Add(AccessTools.TypeByName("AnimalBehaviours.CompAnimalProduct"));
            }

            if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == "rim.job.world"))
            {
                Log.Message("[AnimalGenetics]: Patched RJW");
                new Harmony("AnimalGenetics").Patch(
                    AccessTools.Method(AccessTools.TypeByName("rjw.Hediff_BasePregnancy"), "GenerateBabies"),
                    new HarmonyMethod(typeof(CompatibilityPatches),
                        nameof(CompatibilityPatches.RJW_GenerateBabies_Prefix)),
                    new HarmonyMethod(typeof(CompatibilityPatches),
                        nameof(CompatibilityPatches.RJW_GenerateBabies_Postfix)));
            }
        }
        catch
        {
            // ignored
        }

        defaultAnimalsPawnTableDefColumns = [..PawnTableDefOf.Animals.columns];
        defaultWildlifePawnTableDefColumns = [..PawnTableDefOf.Wildlife.columns];

        var placeholderPosition =
            MainTabWindow_AnimalGenetics.PawnTableDefs.Genetics.columns.FindIndex(def =>
                def.defName == "AnimalGenetics_Placeholder");
        MainTabWindow_AnimalGenetics.PawnTableDefs.Genetics.columns.RemoveAt(placeholderPosition);
        MainTabWindow_AnimalGenetics.PawnTableDefs.Genetics.columns.InsertRange(placeholderPosition,
            PawnTableColumnsDefOf.Genetics.columns);

        PatchUI();
    }

    public static void PatchUI()
    {
        if (PatchState.PatchedGenesInAnimalsTab != Settings.UI.showGenesInAnimalsTab)
        {
            PawnTableDefOf.Animals.columns = [..defaultAnimalsPawnTableDefColumns];
            if (Settings.UI.showGenesInAnimalsTab)
            {
                PawnTableDefOf.Animals.columns.AddRange(PawnTableColumnsDefOf.Genetics.columns);
            }

            PatchState.PatchedGenesInAnimalsTab = Settings.UI.showGenesInAnimalsTab;
        }

        if (PatchState.PatchedGenesInWildlifeTab != Settings.UI.showGenesInWildlifeTab)
        {
            PawnTableDefOf.Wildlife.columns = [..defaultWildlifePawnTableDefColumns];
            if (Settings.UI.showGenesInWildlifeTab)
            {
                PawnTableDefOf.Wildlife.columns.AddRange(PawnTableColumnsDefOf.Genetics.columns);
            }

            PatchState.PatchedGenesInWildlifeTab = Settings.UI.showGenesInWildlifeTab;
        }

        var mainButton = DefDatabase<MainButtonDef>.GetNamed("AnimalGenetics");
        mainButton.buttonVisible = Settings.UI.showGeneticsTab;
    }

    private static class PatchState
    {
        public static bool PatchedGenesInAnimalsTab;
        public static bool PatchedGenesInWildlifeTab;
    }
}