using UnityEngine;
using Verse;

namespace AnimalGenetics;

public class IntegrationMod : Mod
{
    public IntegrationMod(ModContentPack content) : base(content)
    {
        Settings.Integration = GetSettings<IntegrationSettings>();
    }

    public override void DoSettingsWindowContents(Rect rect)
    {
        SettingsUI.DoSettings(Settings.Integration, rect);
    }

    public override string SettingsCategory()
    {
        return "Animal Genetics - Integration Settings";
    }
}