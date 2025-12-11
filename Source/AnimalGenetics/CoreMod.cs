using AnimalGenetics.HarmonyPatches;
using UnityEngine;
using Verse;

namespace AnimalGenetics;

public class CoreMod : Mod
{
    public static CoreMod Instance;
    public readonly CoreSettings Settings;
    private int selectedTab;

    public CoreMod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<CoreSettings>();
        Instance = this;
    }

    public override void DoSettingsWindowContents(Rect rect)
    {
        // Tabbed UI for settings
        var tabsRect = new Rect(rect.x, rect.y, rect.width, 40f);
        var contentRect = new Rect(rect.x, rect.y + tabsRect.height, rect.width, rect.height - tabsRect.height);

        var tabWidth = Mathf.Floor(rect.width / 3f);
        var coreTabRect = new Rect(tabsRect.x, tabsRect.y, tabWidth, tabsRect.height);
        var uiTabRect = new Rect(tabsRect.x + tabWidth, tabsRect.y, tabWidth, tabsRect.height);
        var integrationTabRect = new Rect(tabsRect.x + (tabWidth * 2), tabsRect.y, tabWidth, tabsRect.height);

        if (Widgets.ButtonText(coreTabRect, "Core"))
        {
            selectedTab = 0;
        }

        if (Widgets.ButtonText(uiTabRect, "UI"))
        {
            selectedTab = 1;
        }

        if (Widgets.ButtonText(integrationTabRect, "Integration"))
        {
            selectedTab = 2;
        }

        switch (selectedTab)
        {
            case 0:
                SettingsUI.DoCoreSettings(contentRect);
                break;
            case 1:
                SettingsUI.DoUISettings(contentRect);
                break;
            case 2:
                SettingsUI.DoIntegrationSettings(contentRect);
                break;
        }
    }

    public override string SettingsCategory()
    {
        return "Animal Genetics";
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        AnimalGeneticsAssemblyLoader.VerifyWildness();
        AnimalGeneticsAssemblyLoader.PatchUI();
    }
}