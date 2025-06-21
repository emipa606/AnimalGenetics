using System.Linq;
using LudeonTK;
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

        var maleArray = males as Pawn[] ?? males.ToArray();
        var femaleArray = females as Pawn[] ?? females.ToArray();
        if (maleArray.Count() != 1 || femaleArray.Count() != 1)
        {
            return;
        }

        Hediff_Pregnant.DoBirthSpawn(femaleArray.First(), maleArray.First());
        DebugActionsUtility.DustPuffFrom(femaleArray.First());
    }

    [DebugAction("General", allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void MakePregnancyTogether()
    {
        var males = Find.Selector.SelectedPawns.Where(candidate => candidate.gender == Gender.Male);
        var females = Find.Selector.SelectedPawns.Where(candidate => candidate.gender == Gender.Female);

        var femaleArray = females as Pawn[] ?? females.ToArray();
        var maleArray = males as Pawn[] ?? males.ToArray();
        if (maleArray.Count() != 1 || femaleArray.Count() != 1)
        {
            return;
        }

        var hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, femaleArray.First());
        hediff_Pregnant.father = maleArray.First();
        femaleArray.First().health.AddHediff(hediff_Pregnant);
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

        var maleArray = males as Pawn[] ?? males.ToArray();
        var femaleArray = females as Pawn[] ?? females.ToArray();
        if (maleArray.Count() != 1 || femaleArray.Count() != 1)
        {
            return;
        }

        var female = femaleArray.First();
        var eggLayerComp = female.GetComp<CompEggLayer>();

        if (eggLayerComp == null)
        {
            return;
        }

        eggLayerComp.Fertilize(maleArray.First());

        var egg = eggLayerComp.ProduceEgg();
        if (egg == null)
        {
            return;
        }

        GenSpawn.Spawn(egg, female.Position, female.Map);

        DebugActionsUtility.DustPuffFrom(female);
    }
}