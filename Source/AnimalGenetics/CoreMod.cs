using AnimalGenetics.HarmonyPatches;
using UnityEngine;
using Verse;

namespace AnimalGenetics;

public class CoreMod : Mod
{
    public static CoreSettings InitialSettings;
    private int selectedTab;

    public CoreMod(ModContentPack content) : base(content)
    {
        Settings.InitialCore = GetSettings<CoreSettings>();
        // Do not read multiple different settings classes via GetSettings on a single Mod
        // Initialize non-persisted instances for UI and Integration to avoid warnings      
        Settings.UI ??= new UISettings();

        Settings.Integration ??= new IntegrationSettings();
    }

    public static bool ConfigureInitialSettings => Current.ProgramState == ProgramState.Entry;

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
                SettingsUI.DoSettings(ConfigureInitialSettings ? Settings.InitialCore : Settings.Core, contentRect);
                break;
            case 1:
                SettingsUI.DoSettings(Settings.UI, contentRect);
                break;
            case 2:
                SettingsUI.DoSettings(Settings.Integration, contentRect);
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