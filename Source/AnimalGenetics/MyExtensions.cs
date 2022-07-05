using Verse;

namespace ExtensionMethods;

public static class MyExtensions
{
    public static void CheckboxLabeled(this Listing_Standard listingStandard, string label, ref bool checkOn,
        string tooltip, bool disabled)
    {
        var lineHeight = Text.LineHeight;
        var rect = listingStandard.GetRect(lineHeight);
        if (!tooltip.NullOrEmpty())
        {
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
            }

            TooltipHandler.TipRegion(rect, tooltip);
        }

        Widgets.CheckboxLabeled(rect, label, ref checkOn, disabled);
        listingStandard.Gap(listingStandard.verticalSpacing);
    }
}