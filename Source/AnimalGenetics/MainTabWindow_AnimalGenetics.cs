using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace AnimalGenetics;

public class MainTabWindow_AnimalGenetics : MainTabWindow_PawnTable
{
    private const float CheckboxHeight = 24f;
    private const float Gap = 12f;
    private const float CheckboxWidth = 26f;
    private static readonly MethodInfo doTextField;
    private readonly float animalsWidth = Text.CalcSize("AG.Animals".Translate()).x + CheckboxWidth;
    private readonly float colonyWidth = Text.CalcSize("AG.Colony".Translate()).x + CheckboxWidth;
    private readonly float humanlikesWidth = Text.CalcSize("AG.Humanlikes".Translate()).x + CheckboxWidth;
    private readonly float otherFactionsWidth = Text.CalcSize("AG.OtherFactions".Translate()).x + CheckboxWidth;
    private readonly float wildWidth = Text.CalcSize("AG.Wild".Translate()).x + CheckboxWidth;
    private AnimalGenetics animalGenetics;
    private int filterTextId = -1;

    static MainTabWindow_AnimalGenetics()
    {
        doTextField = typeof(GUI).GetTypeInfo().GetMethod("DoTextField", BindingFlags.NonPublic | BindingFlags.Static,
            null, [typeof(Rect), typeof(int), typeof(GUIContent), typeof(bool), typeof(int), typeof(GUIStyle)],
            null);
    }

    public MainTabWindow_AnimalGenetics()
    {
        forcePause = false;
    }

    private AnimalGenetics AnimalGenetics
    {
        get
        {
            animalGenetics ??= Find.World.GetComponent<AnimalGenetics>();

            return animalGenetics;
        }
    }

    public override PawnTableDef PawnTableDef => PawnTableDefs.Genetics;

    public override IEnumerable<Pawn> Pawns
    {
        get
        {
            var toReturn = new List<Pawn>();
            toReturn.AddRange(from p in Find.CurrentMap.mapPawns.AllPawns
                where p.Spawned && !p.Position.Fogged(p.Map) && visibleSpecies(p) && visibleFactions(p) && textFilter(p)
                select p);
            return toReturn;
        }
    }

    private static string textField(int id, Rect position, string text)
    {
        if (doTextField == null)
        {
            return text;
        }

        var guicontent = new GUIContent(text);
        doTextField.Invoke(null, [position, id, guicontent, false, -1, GUI.skin.textField]);
        return guicontent.text;
    }

    public override void DoWindowContents(Rect rect)
    {
        bool[] originalSettings =
        [
            AnimalGenetics.ShowAnimals, AnimalGenetics.ShowFaction, AnimalGenetics.ShowOther, AnimalGenetics.ShowWild,
            AnimalGenetics.ShowHumans
        ];
        if (filterTextId == -1)
        {
            filterTextId = GUIUtility.GetControlID(FocusType.Keyboard);
        }

        var curX = 5f;
        var curX2 = rect.width - 300f;
        if (!Settings.Core.humanMode)
        {
            AnimalGenetics.ShowHumans = false;
        }

        base.DoWindowContents(rect);

        //working from left side
        Text.Anchor = TextAnchor.LowerLeft;
        if (Settings.Core.humanMode)
        {
            Widgets.CheckboxLabeled(new Rect(curX, 10f, animalsWidth, CheckboxHeight), "AG.Animals".Translate(),
                ref AnimalGenetics.ShowAnimals, false, null, null, true);
            curX += animalsWidth + Gap;
            Widgets.CheckboxLabeled(new Rect(curX, 10f, humanlikesWidth, CheckboxHeight), "AG.Humanlikes".Translate(),
                ref AnimalGenetics.ShowHumans, false, null, null, true);
            curX += humanlikesWidth + Gap + 20f; //extra 20 for category gap
        }

        if (Settings.Core.omniscientMode)
        {
            Widgets.CheckboxLabeled(new Rect(curX, 10f, colonyWidth, CheckboxHeight), "AG.Colony".Translate(),
                ref AnimalGenetics.ShowFaction, false, null, null, true);
            curX += colonyWidth + Gap;
            Widgets.CheckboxLabeled(new Rect(curX, 10f, wildWidth, CheckboxHeight), "AG.Wild".Translate(),
                ref AnimalGenetics.ShowWild, false, null, null, true);
            curX += wildWidth + Gap;
            Widgets.CheckboxLabeled(new Rect(curX, 10f, otherFactionsWidth, CheckboxHeight),
                "AG.OtherFactions".Translate(), ref AnimalGenetics.ShowOther, false, null, null, true);
            Text.Anchor = TextAnchor.UpperLeft;
        }
        else
        {
            AnimalGenetics.ShowFaction = true;
            AnimalGenetics.ShowWild = false;
            AnimalGenetics.ShowOther = false;
        }

        if (originalSettings != (bool[])
            [
                AnimalGenetics.ShowAnimals, AnimalGenetics.ShowFaction, AnimalGenetics.ShowOther,
                AnimalGenetics.ShowWild,
                AnimalGenetics.ShowHumans
            ])
        {
            SetDirty();
        }

        // Working from right side
        curX2 -= 50f;
        Text.Anchor = TextAnchor.MiddleCenter;
        if (Widgets.ButtonText(new Rect(curX2, 10f, 42f, 24f), Constants.SortMode[Settings.UI.sortMode]))
        {
            Settings.UI.sortMode += 1;
            if (Settings.UI.sortMode >= Constants.SortMode.Count)
            {
                Settings.UI.sortMode = 0;
            }

            SetDirty();
        }

        curX2 -= 55f;
        Text.Font = GameFont.Tiny;
        Widgets.Label(new Rect(curX2, 5f, 50f, 32f), "AG.PrimarySort".Translate());
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;
        curX2 -= 125f;

        var current = AnimalGenetics.FilterText;
        AnimalGenetics.FilterText =
            textField(filterTextId, new Rect(curX2, 10f, 120f, 24f), AnimalGenetics.FilterText);

        if (current != AnimalGenetics.FilterText)
        {
            SetDirty();
        }
    }

    public override void PostOpen()
    {
        base.PostOpen();
        Find.World.renderer.wantedMode = WorldRenderMode.None;
    }

    private bool visibleFactions(Pawn p)
    {
        if (AnimalGenetics.ShowFaction && p.Faction == Faction.OfPlayer)
        {
            return true;
        }

        if (AnimalGenetics.ShowWild && p.Faction == null)
        {
            return true;
        }

        return AnimalGenetics.ShowOther && p.Faction != Faction.OfPlayer && p.Faction != null;
    }

    private bool visibleSpecies(Pawn p)
    {
        if (AnimalGenetics.ShowAnimals && p.RaceProps.Animal)
        {
            return true;
        }

        return AnimalGenetics.ShowHumans && p.RaceProps.Humanlike;
    }

    private bool textFilter(Pawn p)
    {
        if (string.IsNullOrEmpty(AnimalGenetics.FilterText))
        {
            return true;
        }

        if (p.Name != null && match(p.Name.ToStringFull))
        {
            return true;
        }

        return match(p.KindLabel) || match(p.def.label);

        bool match(string str)
        {
            return str != null && str.IndexOf(AnimalGenetics.FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }


    [DefOf]
    public static class PawnTableDefs
    {
        public static PawnTableDef Genetics;
    }
}