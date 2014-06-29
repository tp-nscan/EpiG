using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using Genomic.PhenotypeEvals;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome
{

    public class NextGeneratorForStandardSorter<T>
        where T : ISorter
    {
        public NextGeneratorForStandardSorter(
            int keyCount, 
            int orgCount,
            double deletionRate, 
            double insertionRate,
            double mutationRate, 
            double multiplicationRate, 
            double cubRate
         )
        {
            _keyCount = keyCount;
            _orgCount = orgCount;
            _mutationRate = mutationRate;
            _multiplicationRate = multiplicationRate;
            _cubRate = cubRate;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;

            _nextGenerator = (eD, i) =>
            {

                var randy = Rando.Fast(i);

                var winningGenomes =
                    eD.Values.OrderBy(v => v)
                        .Take((int) (eD.Count/multiplicationRate))
                        .Select(ev => ev.Phenotype.PhenotypeBuilder.Genome)
                        .ToList();

                var mutants = winningGenomes
                    .Repeat()
                    .Take(eD.Count - winningGenomes.Count)
                    .Select
                    (
                        g => g.ToSimpleGenomeBuilderMutator
                            (
                                guid: randy.NextGuid(),
                                symbolCount: (uint) KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount),
                                seed: randy.NextInt(),
                                deletionRate: DeletionRate,
                                insertionRate: InsertionRate,
                                mutationRate: MutationRate

                            ).Make()
                    ).ToList();

                return winningGenomes.Concat(mutants).ToDictionary(g => g.Guid);
            };
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

        private readonly 
            Func<
                    IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, 
                    int, 
                    IReadOnlyDictionary<Guid, IGenome>
                >
            _nextGenerator;
        public Func<
                     IReadOnlyDictionary<Guid, IPhenotypeEval<T>>,
                     int,
                     IReadOnlyDictionary<Guid, IGenome>
                    >
                    NextGenerator
        {
            get { return _nextGenerator; }
        }
    }


}
