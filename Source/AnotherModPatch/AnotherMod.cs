using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using rjw;
using Verse;

namespace AnimalGenetics;

[StaticConstructorOnStartup]
public class AnotherMod
{
    static AnotherMod()
    {
        var harmony = new Harmony("AnimalGenetics.AnotherMod");
        harmony.PatchAll();
    }

    [HarmonyPatch]
    public static class PregnancyPatch
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(Hediff_BestialPregnancy), "GiveBirth");
            yield return AccessTools.Method(typeof(Hediff_HumanlikePregnancy), "GiveBirth");
        }

        public static void Prefix(Hediff_BestialPregnancy __instance)
        {
            if (__instance.babies == null || __instance.babies.Count == 0)
            {
                return;
            }

            var motherGeneticInformation = __instance.pawn?.AnimalGenetics();
            var fatherGeneticInformation = __instance.father?.AnimalGenetics();

            if (fatherGeneticInformation == null && motherGeneticInformation != null)
            {
                var fatherGeneticInformationComp = __instance.pawn.health.hediffSet
                    .GetFirstHediffOfDef(HediffDefOf.Pregnant)
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
}