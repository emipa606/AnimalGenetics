using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AnimalGenetics;

public static class Constants
{
    // order here dictates order displayed in game.
    public static readonly List<StatDef> AffectedStats =
    [
        AnimalGenetics.Health,
        AnimalGenetics.Damage,
        StatDefOf.MoveSpeed,
        StatDefOf.CarryingCapacity,
        StatDefOf.MeatAmount,
        StatDefOf.LeatherAmount,
        StatDefOf.Wildness,
        AnimalGenetics.GatherYield
    ];

    public static readonly List<StatDef> InvertedStats =
    [
        StatDefOf.Wildness
    ];

    public static readonly List<StatDef> AffectedStatsToInsert =
    [
        StatDefOf.MoveSpeed,
        StatDefOf.LeatherAmount,
        StatDefOf.MeatAmount,
        StatDefOf.CarryingCapacity,
        StatDefOf.Wildness
    ];

    private static readonly Dictionary<StatDef, string> labelOverrides = new()
    {
        { StatDefOf.MoveSpeed, "AG.Speed".Translate() },
        { AnimalGenetics.Health, "AG.Health".Translate() },
        { AnimalGenetics.Damage, "AG.Damage".Translate() },
        { StatDefOf.CarryingCapacity, "AG.Capacity".Translate() },
        { StatDefOf.MeatAmount, "AG.Meat".Translate() },
        { StatDefOf.LeatherAmount, "AG.Leather".Translate() },
        { StatDefOf.Wildness, "AG.Wildness".Translate() },
        { AnimalGenetics.GatherYield, "AG.GatherYield".Translate() }
    };

    private static readonly Dictionary<StatDef, string> descriptionOverrides = new()
    {
        { StatDefOf.MoveSpeed, "AG.SpeedDesc".Translate() },
        { StatDefOf.CarryingCapacity, "AG.CapacityDesc".Translate() }
    };

    public static readonly Dictionary<int, string> SortMode = new()
    {
        { 0, "AG.None".Translate() },
        { 1, "AG.Asc".Translate() },
        { 2, "AG.Desc".Translate() }
    };

    public static string GetLabel(StatDef stat)
    {
        return !labelOverrides.TryGetValue(stat, out var getLabel) ? stat.label : getLabel;
    }

    public static string GetDescription(StatDef stat)
    {
        return !descriptionOverrides.TryGetValue(stat, out var getDescription) ? stat.description : getDescription;
    }
}