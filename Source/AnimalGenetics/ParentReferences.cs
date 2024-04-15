using System.Collections.Generic;
using System.Linq;

namespace AnimalGenetics;

public static class ParentReferences
{
    private static readonly Stack<Record> Data = new Stack<Record>();

    public static GeneticInformation ThisGeneticInformation => Data
        .Select(record => record.This).FirstOrDefault(record => record != null);

    public static GeneticInformation MotherGeneticInformation => Data
        .Select(record => record.Mother).FirstOrDefault(record => record != null);

    public static GeneticInformation FatherGeneticInformation => Data
        .Select(record => record.Father).FirstOrDefault(record => record != null);

    public static void Pop()
    {
        Data.Pop();
    }

    public static void Push(Record record)
    {
        Data.Push(record);
    }

    public class Record
    {
        public GeneticInformation Father;
        public GeneticInformation Mother;
        public GeneticInformation This;
    }
}