using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;
using Utils;
using Workflows;

namespace SorterGenome
{
    public interface ISorterCompPool<T> : IRandomWalk<ISorterCompPool<T>>, IEntity
        where T : ISorter
    {
        int Generation { get; }

        SorterCompPoolStageType SorterCompPoolStageType { get; }

        IReadOnlyDictionary<Guid, IGenome> Genomes { get; }

        IReadOnlyDictionary<Guid, IPhenotype<T>> Phenotypes { get; }

        IReadOnlyDictionary<Guid, IPhenotypeEval<T>> PhenotypeEvals { get; }

        Func<IGenome, IRando, IPhenotype<T>> Phenotyper { get; }

        Func<IPhenotype<T>, IRando, IPhenotypeEval<T>> PhenotypeEvaluator { get; }

        Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IGenome>> NextGenerator { get; }

    }

    public static class SorterCompPool
    {



        public static ISorterCompPool<ISorter> InitStandardFromSeed

        (
            int seed,
            int orgCount,
            int seqenceLength,
            int keyCount,
            double deletionRate, 
            double insertionRate,
            double mutationRate,
            double multiplicationRate,
            double cubRate
        )
        {
            return new SorterCompPoolStandard<ISorter>(
                    generation: 0,
                    genomes: GenomeBuilder.MakeSimpleGenomeBuilderRandoms
                            (
                               seed: seed,
                               builderCount: orgCount,
                               symbolCount: (uint)KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount),
                               sequenceLength: seqenceLength
                            ).Select(b => b.Make()).ToDictionary(v => v.GenomeBuilder.Guid),
                    phenotypes: null,
                    phenotypeEvals: null,
                    sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                    keyCount: keyCount,
                    orgCount: orgCount,
                    deletionRate: deletionRate,
                    insertionRate: insertionRate,
                    mutationRate: mutationRate,
                    multiplicationRate: multiplicationRate
                );
        }





        public static ISorterCompPool<ISorter> InitPermutorFromSeed
        (
            int seed,
            int orgCount,
            int seqenceLength,
            int keyCount,
            double deletionRate,
            double insertionRate,
            double mutationRate,
            double multiplicationRate,
            double cubRate
        )
        {
            return new SorterCompPoolStandard<ISorter>(
                    generation: 0,
                    genomes: GenomeBuilder.MakeSimpleGenomeBuilderRandoms
                            (
                               seed: seed,
                               builderCount: orgCount,
                               symbolCount: (uint)KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount),
                               sequenceLength: seqenceLength
                            ).Select(b => b.Make()).ToDictionary(v => v.GenomeBuilder.Guid),
                    phenotypes: null,
                    phenotypeEvals: null,
                    sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                    keyCount: keyCount,
                    orgCount: orgCount,
                    deletionRate: deletionRate,
                    insertionRate: insertionRate,
                    mutationRate: mutationRate,
                    multiplicationRate: multiplicationRate
                );
        }






    }

    public enum SorterCompPoolStageType
    {
        MakePhenotypes,
        EvaluatePhenotypes,
        MakeNextGeneration
    }


    public class SorterCompPoolStandard<T> : ISorterCompPool<T> where T : ISorter
    {
        public SorterCompPoolStandard
        (
            int generation,
            IReadOnlyDictionary<Guid, IGenome> genomes,
            IReadOnlyDictionary<Guid, IPhenotype<T>> phenotypes,
            IReadOnlyDictionary<Guid, IPhenotypeEval<T>> phenotypeEvals,
            SorterCompPoolStageType sorterCompPoolStageType,
            int keyCount, 
            int orgCount, 
            double multiplicationRate, 
            double deletionRate, 
            double insertionRate,
            double mutationRate
       )
    {
        _generation = generation;
        _genomes = genomes;
        _phenotypes = phenotypes;
        _phenotypeEvals = phenotypeEvals;
        _sorterCompPoolStageType = sorterCompPoolStageType;
        _keyCount = keyCount;
        _orgCount = orgCount;
        _mutationRate = mutationRate;
        _multiplicationRate = multiplicationRate;
        _deletionRate = deletionRate;
        _insertionRate = insertionRate;
    }

    public ISorterCompPool<T> Step(int seed)
    {
        switch (SorterCompPoolStageType)
        {
            case SorterCompPoolStageType.MakePhenotypes:

                var randy = Rando.Fast(seed);
                return new SorterCompPoolStandard<T>
                    (
                        generation: Generation,
                        genomes: Genomes,
                        phenotypes: Genomes.Values
                            .Select(g => Phenotyper(g, randy))
                            .ToDictionary(p => p.Guid),
                        phenotypeEvals: PhenotypeEvals,
                        sorterCompPoolStageType: SorterCompPoolStageType.EvaluatePhenotypes,
                        keyCount: KeyCount,
                        orgCount: OrgCount,
                        deletionRate: DeletionRate,
                        insertionRate: InsertionRate,
                        mutationRate: MutationRate,
                        multiplicationRate: MultiplicationRate
                    );

            case SorterCompPoolStageType.EvaluatePhenotypes:

                var randy2 = Rando.Fast(seed);
                return new SorterCompPoolStandard<T>
                    (
                        generation: Generation,
                        genomes: Genomes,
                        phenotypes: Phenotypes,
                        phenotypeEvals: Phenotypes.Values
                            .Select(p => PhenotypeEvaluator(p, randy2))
                            .ToDictionary(pe => pe.Guid),
                        sorterCompPoolStageType: SorterCompPoolStageType.MakeNextGeneration,
                        keyCount: KeyCount,
                        orgCount: OrgCount,
                        deletionRate: DeletionRate,
                        insertionRate: InsertionRate,
                        mutationRate: MutationRate,
                        multiplicationRate: MultiplicationRate
                    );

            case SorterCompPoolStageType.MakeNextGeneration:

                return new SorterCompPoolStandard<T>
                    (
                        generation: Generation + 1,
                        genomes: NextGenerator(PhenotypeEvals, seed),
                        phenotypes: Phenotypes,
                        phenotypeEvals: PhenotypeEvals,
                        sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                        keyCount: KeyCount,
                        orgCount: OrgCount,
                        deletionRate: DeletionRate,
                        insertionRate: InsertionRate,
                        mutationRate: MutationRate,
                        multiplicationRate: MultiplicationRate
                    );
            default:
                throw new Exception(SorterCompPoolStageType + " not handled");
        }

        return null;

    }

    private readonly int _keyCount;

    public int KeyCount
    {
        get { return _keyCount; }
    }


    private readonly int _orgCount;
    public int OrgCount
    {
        get { return _orgCount; }
    }

    private readonly double _deletionRate;
    public double DeletionRate
    {
        get { return _deletionRate; }
    }

    private readonly double _insertionRate;
    public double InsertionRate
    {
        get { return _insertionRate; }
    }

    private readonly double _mutationRate;
    public double MutationRate
    {
        get { return _mutationRate; }
    }

    private readonly double _multiplicationRate;
    public double MultiplicationRate
    {
        get { return _multiplicationRate; }
    }

    private readonly double _cubRate;
    public double CubRate
    {
        get { return _cubRate; }
    }

    private readonly int _generation;
    public int Generation
    {
        get { return _generation; }
    }

    private readonly SorterCompPoolStageType _sorterCompPoolStageType;

    public SorterCompPoolStageType SorterCompPoolStageType
    {
        get { return _sorterCompPoolStageType; }
    }

    private readonly IReadOnlyDictionary<Guid, IGenome> _genomes;

    public IReadOnlyDictionary<Guid, IGenome> Genomes
    {
        get { return _genomes; }
    }

    private readonly IReadOnlyDictionary<Guid, IPhenotype<T>> _phenotypes;

    public IReadOnlyDictionary<Guid, IPhenotype<T>> Phenotypes
    {
        get { return _phenotypes; }
    }

    private readonly IReadOnlyDictionary<Guid, IPhenotypeEval<T>> _phenotypeEvals;

    public IReadOnlyDictionary<Guid, IPhenotypeEval<T>> PhenotypeEvals
    {
        get { return _phenotypeEvals; }
    }

    private Func<IGenome, IRando, IPhenotype<T>> _phenotyper;

    public Func<IGenome, IRando, IPhenotype<T>> Phenotyper
    {
        get { return _phenotyper ?? (_phenotyper = Phenotypers.MakeStandard<T>(KeyCount)); }
    }

    private Func<IPhenotype<T>, IRando, IPhenotypeEval<T>> _phenotypeEvaluator;

    public Func<IPhenotype<T>, IRando, IPhenotypeEval<T>> PhenotypeEvaluator
    {
        get
        {
            return _phenotypeEvaluator ?? (_phenotypeEvaluator = PhenotypeEvaluators.MakeStandard<T>());
        }
    }

    private Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IGenome>>
        _nextGenerator;

    public Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IGenome>> NextGenerator
    {
        get 
        { 
            return _nextGenerator ?? 
                (
                    _nextGenerator = new NextGeneratorImpl<T>
                    (
                        keyCount : KeyCount, 
                        orgCount : OrgCount,
                        deletionRate: DeletionRate,
                        insertionRate: InsertionRate,
                        mutationRate: MutationRate,
                        multiplicationRate: MultiplicationRate,
                        cubRate: CubRate
                    ).NextGenerator
                ); 
        }
    }

    ISorterCompPool<T> IRandomWalk<ISorterCompPool<T>>.Step(int seed)
    {
        return Step(seed);
    }

    private Guid _guid;

    public Guid Guid
    {
        get { return _guid; }
    }

    public object GetPart(Guid key)
    {
        if (Genomes.ContainsKey(key))
        {
            return Genomes[key];
        }
        if (Phenotypes.ContainsKey(key))
        {
            return Phenotypes[key];
        }
        if (PhenotypeEvals.ContainsKey(key))
        {
            return PhenotypeEvals[key];
        }
        return null;
    }

    public string EntityName
    {
        get { return "SorterCompPool.Standard"; }
    }
}
}







