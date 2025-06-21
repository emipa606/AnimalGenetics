using RimWorld;
using RimWorld.Planet;
using Verse;
using AnimalGeneticsSettings = AnimalGenetics.Settings;

namespace AnimalGenetics;

public class AnimalGenetics(World world) : WorldComponent(world)
{
    public static readonly StatDef GatherYield = new()
        { defName = "GatherYield", description = "AG.GatherYieldDesc".Translate(), alwaysHide = true };

    public static readonly StatDef Damage = new()
        { defName = "Damage", description = "AG.DamageDesc".Translate(), alwaysHide = true };

    public static readonly StatDef Health = new()
        { defName = "Health", description = "AG.HealthDesc".Translate(), alwaysHide = true };

    public readonly CoreSettings Settings = (CoreSettings)AnimalGeneticsSettings.InitialCore.Clone();

    public string FilterText;
    public bool ShowAnimals = true;
    public bool ShowFaction = true;
    public bool ShowHumans = true;
    public bool ShowOther = true;
    public bool ShowWild = true;

    public override void ExposeData()
    {
        GeneticInformation.ExposeData();
        Scribe_Values.Look(ref FilterText, "filterText");
        Scribe_Values.Look(ref ShowAnimals, "showAnimals", true);
        Scribe_Values.Look(ref ShowFaction, "showFaction", true);
        Scribe_Values.Look(ref ShowHumans, "showHumans", true);
        Scribe_Values.Look(ref ShowOther, "showOther", true);
        Scribe_Values.Look(ref ShowWild, "showWild", true);

        if (!Scribe.EnterNode("settings"))
        {
            return;
        }

        Settings.ExposeData();
        Scribe.ExitNode();
    }
}