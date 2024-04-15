using HarmonyLib;
using Verse;

namespace AnimalGenetics;

[StaticConstructorOnStartup]
public class AnotherMod
{
    static AnotherMod()
    {
        new Harmony("AnimalGenetics.AnotherMod").PatchAll();
    }
}