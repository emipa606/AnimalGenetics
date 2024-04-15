using UnityEngine;
using Verse;

namespace AnimalGenetics;

public class UIMod : Mod
{
    public UIMod(ModContentPack content) : base(content)
    {
        Settings.UI = GetSettings<UISettings>();
    }

    public override void DoSettingsWindowContents(Rect rect)
    {
        SettingsUI.DoSettings(Settings.UI, rect);
    }

    public override string SettingsCategory()
    {
        return "Animal Genetics - UI Settings";
    }
}