using RimWorld;
using RimWorld.Planet;
using Verse;
using AnimalGeneticsSettings = AnimalGenetics.Settings;

namespace AnimalGenetics;

public class AnimalGenetics(World world) : WorldComponent(world)
{
    public static readonly StatDef GatherYield = new StatDef
        { defName = "GatherYield", description = "AG.GatherYieldDesc".Translate(), alwaysHide = true };

    public static readonly StatDef Damage = new StatDef
        { defName = "Damage", description = "AG.DamageDesc".Translate(), alwaysHide = true };

    public static readonly StatDef Health = new StatDef
        { defName = "Health", description = "AG.HealthDesc".Translate(), alwaysHide = true };

    public readonly CoreSettings Settings = (CoreSettings)AnimalGeneticsSettings.InitialCore.Clone();

    public override void ExposeData()
    {
        GeneticInformation.ExposeData();

        if (!Scribe.EnterNode("settings"))
        {
            return;
        }

        Settings.ExposeData();
        Scribe.ExitNode();
    }
}