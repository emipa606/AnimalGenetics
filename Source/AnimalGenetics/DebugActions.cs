using System.Linq;
using RimWorld;
using Verse;

namespace AnimalGenetics;

public static class DebugActions
{
    [DebugAction("General", allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void GiveBirthTogether()
    {
        var males = Find.Selector.SelectedPawns.Where(candidate => candidate.gender == Gender.Male);
        var females = Find.Selector.SelectedPawns.Where(candidate => candidate.gender == Gender.Female);

        if (males.Count() != 1 || females.Count() != 1)
        {
            return;
        }

        Hediff_Pregnant.DoBirthSpawn(females.First(), males.First());
        DebugActionsUtility.DustPuffFrom(females.First());
    }

    [DebugAction("General", allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void MakePregnancyTogether()
    {
        var males = Find.Selector.SelectedPawns.Where(candidate => candidate.gender == Gender.Male);
        var females = Find.Selector.SelectedPawns.Where(candidate => candidate.gender == Gender.Female);

        if (males.Count() != 1 || females.Count() != 1)
        {
            return;
        }

        var hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, females.First());
        hediff_Pregnant.father = males.First();
        females.First().health.AddHediff(hediff_Pregnant);
    }

    [DebugAction("General", allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void FinishPregnancy()
    {
        var pregnancies = Find.Selector.SelectedPawns
            .Where(candidate => candidate.health.hediffSet.HasHediff(HediffDefOf.Pregnant))
            .Select(candidate => candidate.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant));

        foreach (var hediff1 in pregnancies)
        {
            var hediff = (Hediff_Pregnant)hediff1;
            Hediff_Pregnant.DoBirthSpawn(hediff.pawn, hediff.father);
            hediff.pawn.health.RemoveHediff(hediff);
        }
    }

    [DebugAction("General", allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void LayEggTogether()
    {
        var males = Find.Selector.SelectedPawns.Where(candidate => candidate.gender == Gender.Male);
        var females = Find.Selector.SelectedPawns.Where(candidate => candidate.gender == Gender.Female);

        if (males.Count() != 1 || females.Count() != 1)
        {
            return;
        }

        var female = females.First();
        var eggLayerComp = female.GetComp<CompEggLayer>();

        if (eggLayerComp == null)
        {
            return;
        }

        eggLayerComp.Fertilize(males.First());

        var egg = eggLayerComp.ProduceEgg();
        if (egg == null)
        {
            return;
        }

        GenSpawn.Spawn(egg, female.Position, female.Map);

        DebugActionsUtility.DustPuffFrom(female);
    }
}

/*
[HarmonyPatch(typeof(Pawn_AgeTracker), nameof(Pawn_AgeTracker.AgeTick))]
public class GrowUp
{
    static public bool Prefix(Pawn_AgeTracker __instance, Pawn ___pawn)
    {
        if (___pawn.RaceProps.FleshType == FleshTypeDefOf.Normal)
        {
            while (!___pawn.Dead && !__instance.CurLifeStage.reproductive)
            {
                Log.Warning("[AnimalGenetics]: Aging pawn");
                __instance.DebugMake1YearOlder();
            }
        }
        return false;
    }
}
    */
/*
[HarmonyPatch(typeof(Hediff_Pregnant), nameof(Hediff_Pregnant.Tick))]
public class BabyOut
{
    static public bool Prefix(Hediff_Pregnant __instance)
    {
        __instance.Severity = 1;
        return true;
    }
}
*/
/*
[HarmonyPatch(typeof(PawnUtility), nameof(PawnUtility.BodyResourceGrowthSpeed))]
public static class FastGrowth
{
    static public bool Prefix(ref float __result)
    {
        __result = 10000f;
        return false;
    }
}
*/
/*
[HarmonyPatch(typeof(CompHatcher), nameof(CompHatcher.CompTick))]
public static class FastHatch
{
    static public bool Prefix(ref CompHatcher __instance)
    {
        __instance.Hatch();
        return false;
    }
}
*/