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
    private static readonly MethodInfo _DoTextField;
    private static readonly float checkboxHeight = 24f;
    private static readonly float gap = 12f;
    private static readonly float checkboxWidth = 26f;
    private readonly float animalsWidth = Text.CalcSize("AG.Animals".Translate()).x + checkboxWidth;
    private readonly float colonyWidth = Text.CalcSize("AG.Colony".Translate()).x + checkboxWidth;
    private readonly float humanlikesWidth = Text.CalcSize("AG.Humanlikes".Translate()).x + checkboxWidth;
    private readonly float otherFactionsWidth = Text.CalcSize("AG.OtherFactions".Translate()).x + checkboxWidth;
    private readonly float wildWidth = Text.CalcSize("AG.Wild".Translate()).x + checkboxWidth;

    private int _filterTextId = -1;

    private Options current;
    private Options previous;

    static MainTabWindow_AnimalGenetics()
    {
        _DoTextField = typeof(GUI).GetTypeInfo().GetMethod("DoTextField", BindingFlags.NonPublic | BindingFlags.Static,
            null, new[] { typeof(Rect), typeof(int), typeof(GUIContent), typeof(bool), typeof(int), typeof(GUIStyle) },
            null);
    }

    public MainTabWindow_AnimalGenetics()
    {
        current = new Options
        {
            ShowAnimals = true,
            ShowHumans = true,
            ShowFaction = true,
            ShowWild = true,
            ShowOther = true,
            FilterText = ""
        };
        previous = current;

        forcePause = false;
    }

    protected override PawnTableDef PawnTableDef => PawnTableDefs.Genetics;

    protected override IEnumerable<Pawn> Pawns
    {
        get
        {
            var toReturn = new List<Pawn>();
            toReturn.AddRange(from p in Find.CurrentMap.mapPawns.AllPawns
                where p.Spawned && !p.Position.Fogged(p.Map) && VisibleSpecies(p) && VisibleFactions(p) && TextFilter(p)
                select p);
            return toReturn;
        }
    }

    public string TextField(int id, Rect position, string text)
    {
        if (_DoTextField == null)
        {
            return text;
        }

        var guicontent = new GUIContent(text);
        _DoTextField.Invoke(null, new object[] { position, id, guicontent, false, -1, GUI.skin.textField });
        return guicontent.text;
    }

    public override void DoWindowContents(Rect rect)
    {
        if (_filterTextId == -1)
        {
            _filterTextId = GUIUtility.GetControlID(FocusType.Keyboard);
        }

        var curX = 5f;
        var curX2 = rect.width - 300f;
        if (!Settings.Core.humanMode)
        {
            current.ShowHumans = false;
        }

        base.DoWindowContents(rect);

        //working from left side
        Text.Anchor = TextAnchor.LowerLeft;
        if (Settings.Core.humanMode)
        {
            Widgets.CheckboxLabeled(new Rect(curX, 10f, animalsWidth, checkboxHeight), "AG.Animals".Translate(),
                ref current.ShowAnimals, false, null, null, true);
            curX += animalsWidth + gap;
            Widgets.CheckboxLabeled(new Rect(curX, 10f, humanlikesWidth, checkboxHeight), "AG.Humanlikes".Translate(),
                ref current.ShowHumans, false, null, null, true);
            curX += humanlikesWidth + gap + 20f; //extra 20 for category gap
        }

        if (Settings.Core.omniscientMode)
        {
            Widgets.CheckboxLabeled(new Rect(curX, 10f, colonyWidth, checkboxHeight), "AG.Colony".Translate(),
                ref current.ShowFaction, false, null, null, true);
            curX += colonyWidth + gap;
            Widgets.CheckboxLabeled(new Rect(curX, 10f, wildWidth, checkboxHeight), "AG.Wild".Translate(),
                ref current.ShowWild, false, null, null, true);
            curX += wildWidth + gap;
            Widgets.CheckboxLabeled(new Rect(curX, 10f, otherFactionsWidth, checkboxHeight),
                "AG.OtherFactions".Translate(), ref current.ShowOther, false, null, null, true);
            Text.Anchor = TextAnchor.UpperLeft;
        }
        else
        {
            current.ShowFaction = true;
            current.ShowWild = false;
            current.ShowOther = false;
        }

        // Working from right side
        curX2 -= 50f;
        Text.Anchor = TextAnchor.MiddleCenter;
        if (Widgets.ButtonText(new Rect(curX2, 10f, 42f, 24f), Constants.sortMode[Settings.UI.sortMode]))
        {
            Settings.UI.sortMode += 1;
            if (Settings.UI.sortMode >= Constants.sortMode.Count)
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

        current.FilterText = TextField(_filterTextId, new Rect(curX2, 10f, 120f, 24f), current.FilterText);

        if (current.Equals(previous))
        {
            return;
        }

        previous = current;
        SetDirty();
    }

    public override void PostOpen()
    {
        base.PostOpen();
        Find.World.renderer.wantedMode = WorldRenderMode.None;
    }

    private bool VisibleFactions(Pawn p)
    {
        if (current.ShowFaction && p.Faction == Faction.OfPlayer)
        {
            return true;
        }

        if (current.ShowWild && p.Faction == null)
        {
            return true;
        }

        if (current.ShowOther && p.Faction != Faction.OfPlayer && p.Faction != null)
        {
            return true;
        }

        return false;
    }

    private bool VisibleSpecies(Pawn p)
    {
        if (current.ShowAnimals && p.RaceProps.Animal)
        {
            return true;
        }

        if (current.ShowHumans && p.RaceProps.Humanlike)
        {
            return true;
        }

        return false;
    }

    private bool TextFilter(Pawn p)
    {
        if (current.FilterText == "")
        {
            return true;
        }

        bool Match(string str)
        {
            return str != null && str.IndexOf(current.FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        if (p.Name != null && Match(p.Name.ToStringFull))
        {
            return true;
        }

        return Match(p.KindLabel) || Match(p.def.label);
    }

    private struct Options
    {
        public bool ShowAnimals;
        public bool ShowHumans;
        public bool ShowFaction;
        public bool ShowWild;
        public bool ShowOther;
        public string FilterText;
    }

    [DefOf]
    public static class PawnTableDefs
    {
        public static PawnTableDef Genetics;
    }
}