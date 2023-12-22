using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch(typeof(Hediff_Pregnant), nameof(Hediff_Pregnant.DoBirthSpawn))]
public static class DoBirthSpawn_Patch
{
    public static void Prefix(Pawn mother, Pawn father)
    {
        var motherGeneticInformation = mother?.AnimalGenetics();
        var fatherGeneticInformation = father?.AnimalGenetics();

        if (fatherGeneticInformation == null && motherGeneticInformation != null)
        {
            var fatherGeneticInformationComp = mother.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant)
                .TryGetComp<FatherGeneticInformation>();
            fatherGeneticInformation = fatherGeneticInformationComp?.GeneticInformation;
        }

        ParentReferences.Push(new ParentReferences.Record
            { Mother = motherGeneticInformation, Father = fatherGeneticInformation });
    }

    public static void Postfix()
    {
        ParentReferences.Pop();
    }
}