using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils.Rand;
using Sorting.Sorters;
using Utils;

namespace SorterGenome.CompPool
{
    public class SorterCompPoolPermutation<T> : ISorterCompPool<T> where T : ISorter
    {
        public SorterCompPoolPermutation
            (
                Guid guid,
                int generation,
                IReadOnlyDictionary<Guid, IGenome> genomes,
                IReadOnlyDictionary<Guid, IPhenotype<T>> phenotypes,
                IReadOnlyDictionary<Guid, IPhenotypeEval<T>> phenotypeEvals,
                SorterCompPoolStageType sorterCompPoolStageType,
                int keyCount,
                int orgCount,
                double legacyRate,
                double deletionRate,
                double insertionRate,
                double mutationRate,
                double cubRate
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
            _cubRate = cubRate;
            _guid = guid;
            _legacyRate = legacyRate;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;
        }

        public ISorterCompPool<T> Step(int seed)
        {
            switch (SorterCompPoolStageType)
            {
                case SorterCompPoolStageType.MakePhenotypes:

                    var randy = Rando.Fast(seed);
                    return new SorterCompPoolPermutation<T>
                        (
                            guid: Guid.NewGuid(),
                            generation: Generation,
                            genomes: Genomes,
                            phenotypes: Genomes.Values
                                .SelectMany(g => Phenotyper(g, randy))
                                .ToDictionary(p => p.Guid),
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.EvaluatePhenotypes,
                            keyCount: KeyCount,
                            orgCount: OrgCount,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            legacyRate: LegacyRate,
                            cubRate: CubRate
                        );

                case SorterCompPoolStageType.EvaluatePhenotypes:

                    var randy2 = Rando.Fast(seed);
                    return new SorterCompPoolPermutation<T>
                        (
                            guid: Guid.NewGuid(),
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
                            legacyRate: LegacyRate,
                            cubRate: CubRate
                        );

                case SorterCompPoolStageType.MakeNextGeneration:

                    return new SorterCompPoolPermutation<T>
                        (
                            guid: Guid.NewGuid(),
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
                            legacyRate: LegacyRate,
                            cubRate: CubRate
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

        private readonly double _legacyRate;
        public double LegacyRate
        {
            get { return _legacyRate; }
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

        private Func<IGenome, IRando, IEnumerable<IPhenotype<T>>> _phenotyper;

        public Func<IGenome, IRando, IEnumerable<IPhenotype<T>>> Phenotyper
        {
            get { return _phenotyper ?? (_phenotyper = Phenotypers.MakePermuter<T>(KeyCount)); }
        }

        private Func<IPhenotype<T>, IRando, IPhenotypeEval<T>> _phenotypeEvaluator;

        public Func<IPhenotype<T>, IRando, IPhenotypeEval<T>> PhenotypeEvaluator
        {
            get
            {
                return _phenotypeEvaluator ?? 
                    (_phenotypeEvaluator = PhenotypeEvaluators.MakeStandard<T>());
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
                           _nextGenerator = new NextGeneratorForStagedSorter<T>
                               (
                                   keyCount: KeyCount,
                                   orgCount: OrgCount,
                                   deletionRate: DeletionRate,
                                   insertionRate: InsertionRate,
                                   mutationRate: MutationRate,
                                   legacyRate: LegacyRate,
                                   cubRate: CubRate
                               ).NextGenerator
                           );
            }
        }

        ISorterCompPool<T> IRandomWalk<ISorterCompPool<T>>.Step(int seed)
        {
            return Step(seed);
        }

        private readonly Guid _guid;
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
            get { return "SorterCompPool.Permutation"; }
        }
    }
}