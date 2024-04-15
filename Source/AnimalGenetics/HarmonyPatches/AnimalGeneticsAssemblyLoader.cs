using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[StaticConstructorOnStartup]
public static class AnimalGeneticsAssemblyLoader
{
    private static readonly List<PawnColumnDef> _DefaultAnimalsPawnTableDefColumns;
    private static readonly List<PawnColumnDef> _DefaultWildlifePawnTableDefColumns;
    public static readonly bool ColonyManagerLoaded;

    public static readonly List<Type> gatherableTypes;

    static AnimalGeneticsAssemblyLoader()
    {
        var h = new Harmony("AnimalGenetics");
        h.PatchAll();

        DefDatabase<StatDef>.Add(AnimalGenetics.Damage);
        DefDatabase<StatDef>.Add(AnimalGenetics.Health);
        DefDatabase<StatDef>.Add(AnimalGenetics.GatherYield);

        var affectedStats = Constants.affectedStatsToInsert;
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
        foreach (var stat in Constants.affectedStats)
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

        gatherableTypes =
        [
            typeof(CompShearable),
            typeof(CompMilkable),
            typeof(CompEggLayer)
        ];

        // Compatibility patches
        try
        {
            ColonyManagerLoaded = ModLister.GetActiveModWithIdentifier("Fluffy.ColonyManager") != null;

            if (ModLister.GetActiveModWithIdentifier("CETeam.CombatExtended") != null)
            {
                gatherableTypes.Add(AccessTools.TypeByName("CombatExtended.CompShearableRenameable"));
            }

            if (ModLister.GetActiveModWithIdentifier("OskarPotocki.VanillaFactionsExpanded.Core") != null)
            {
                gatherableTypes.Add(AccessTools.TypeByName("AnimalBehaviours.CompAnimalProduct"));
            }

            if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == "rim.job.world"))
            {
                Log.Message("[AnimalGenetics]: Patched RJW");
                h.Patch(AccessTools.Method(AccessTools.TypeByName("rjw.Hediff_BasePregnancy"), "GenerateBabies"),
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

        _DefaultAnimalsPawnTableDefColumns = [..PawnTableDefOf.Animals.columns];
        _DefaultWildlifePawnTableDefColumns = [..PawnTableDefOf.Wildlife.columns];

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
            PawnTableDefOf.Animals.columns = [.._DefaultAnimalsPawnTableDefColumns];
            if (Settings.UI.showGenesInAnimalsTab)
            {
                PawnTableDefOf.Animals.columns.AddRange(PawnTableColumnsDefOf.Genetics.columns);
            }

            PatchState.PatchedGenesInAnimalsTab = Settings.UI.showGenesInAnimalsTab;
        }

        if (PatchState.PatchedGenesInWildlifeTab != Settings.UI.showGenesInWildlifeTab)
        {
            PawnTableDefOf.Wildlife.columns = [.._DefaultWildlifePawnTableDefColumns];
            if (Settings.UI.showGenesInWildlifeTab)
            {
                PawnTableDefOf.Wildlife.columns.AddRange(PawnTableColumnsDefOf.Genetics.columns);
            }

            PatchState.PatchedGenesInWildlifeTab = Settings.UI.showGenesInWildlifeTab;
        }

        var mainButton = DefDatabase<MainButtonDef>.GetNamed("AnimalGenetics");
        mainButton.buttonVisible = Settings.UI.showGeneticsTab;
    }

    public static class PatchState
    {
        public static bool PatchedGenesInAnimalsTab;
        public static bool PatchedGenesInWildlifeTab;
    }
}