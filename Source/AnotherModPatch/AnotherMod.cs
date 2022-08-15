using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
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
            var type = AccessTools.TypeByName("PATCH_Hediff_Pregnant_DoBirthSpawn");
            yield return AccessTools.FirstMethod(type, method => method.Name.Contains("ProcessVanillaPregnancy"));
            yield return AccessTools.FirstMethod(type, method => method.Name.Contains("ProcessVanillaEggPregnancy"));
        }

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
            Log.Message($"AnimalGenetics: Mother is {mother}, Father is {father}");
        }

        public static void Postfix()
        {
            ParentReferences.Pop();
        }
    }
}