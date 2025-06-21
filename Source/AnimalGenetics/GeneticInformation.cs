using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AnimalGenetics;

public class GeneticInformation : ILoadReferenceable, IExposable
{
    private static List<WeakReference<GeneticInformation>> _instances = [];

    private static readonly Dictionary<GeneticInformation, Dictionary<StatDef, GeneRecord>> BackwardCompatibleData =
        new();

    private static int nextLoadId = 1;

    public Dictionary<StatDef, GeneRecord> _geneRecords;

    private int _loadId;
    public GeneticInformation Father;

    public GeneticInformation Mother;

    public GeneticInformation()
    {
        _instances.Add(new WeakReference<GeneticInformation>(this));
        Mother = ParentReferences.MotherGeneticInformation;
        Father = ParentReferences.FatherGeneticInformation;
    }

    public GeneticInformation(Dictionary<StatDef, GeneRecord> geneRecords)
    {
        _geneRecords = geneRecords;
        _loadId = nextLoadId++;
        _instances.Add(new WeakReference<GeneticInformation>(this));
    }

    private static IEnumerable<GeneticInformation> Instances
    {
        get
        {
            _instances = _instances.Where(wr => wr.IsAlive).ToList();
            return _instances.Where(wr => wr.Target != null)
                .Select(wr => wr.Target);
        }
    }

    public Dictionary<StatDef, GeneRecord> GeneRecords
    {
        get
        {
            _geneRecords ??= GeneticCalculator.GenerateGenesRecord(Mother, Father);

            return _geneRecords;
        }
    }

    void IExposable.ExposeData()
    {
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            ((ILoadReferenceable)this).GetUniqueLoadID();
        }

        Scribe_Values.Look(ref _loadId, "loadID", 0, true);

        Scribe_References.Look(ref Mother, "motherLoadID");
        Scribe_References.Look(ref Father, "fatherLoadID");

        Scribe_Collections.Look(ref _geneRecords, "genes");
    }

    string ILoadReferenceable.GetUniqueLoadID()
    {
        if (_loadId == 0)
        {
            _loadId = nextLoadId++;
        }

        return _loadId.ToString();
    }

    public static void ExposeData()
    {
        if (Scribe.mode == LoadSaveMode.LoadingVars)
        {
            _instances.Clear();
        }

        var instances = Instances.ToList();
        Scribe_Collections.Look(ref instances, "geneticInformation", LookMode.Deep);

        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            nextLoadId = instances.Count > 0 ? instances.Max(e => e._loadId) + 1 : 1;
        }
    }
}