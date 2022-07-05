using UnityEngine;
using Verse;

namespace AnimalGenetics;

public class CoreMod : Mod
{
    public static CoreSettings InitialSettings;

    public CoreMod(ModContentPack content) : base(content)
    {
        Settings.InitialCore = GetSettings<CoreSettings>();
    }

    public bool ConfigureInitialSettings => Current.ProgramState == ProgramState.Entry;

    public override void DoSettingsWindowContents(Rect rect)
    {
        SettingsUI.DoSettings(ConfigureInitialSettings ? Settings.InitialCore : Settings.Core, rect);
    }

    public override string SettingsCategory()
    {
        if (ConfigureInitialSettings)
        {
            return "Animal Genetics - Initial Game Settings";
        }

        return "Animal Genetics - Game Settings";
    }
}