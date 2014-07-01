using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using Genomic.PhenotypeEvals;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.Sorters;

namespace SorterGenome
{
    public class NextGeneratorForPermutationSorter<T>
        where T : ISorter
    {
        public NextGeneratorForPermutationSorter(
            int keyCount, 
            int orgCount,
            double deletionRate, 
            double insertionRate,
            double mutationRate, 
            double legacyRate, 
            double cubRate
         )
        {
            _keyCount = keyCount;
            _orgCount = orgCount;
            _mutationRate = mutationRate;
            _legacyRate = legacyRate;
            _cubRate = cubRate;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;

            _nextGenerator = (eD, i) =>
            {

                var randy = Rando.Fast(i);

                var leaderBoard =
                    eD.Values.OrderBy(v => v)
                        .Select(ev => ev.Phenotype.PhenotypeBuilder.Genome)
                        .ToList();

                var legacies = leaderBoard.Take((int) (OrgCount*LegacyRate)).ToList();

                var mutants =
                    leaderBoard.Take((int)(OrgCount * CubRate))
                    .Repeat()
                    .Take(OrgCount - legacies.Count)
                    .Select
                    (
                        g => g.ToPermutationMutatorBuilder
                            (
                                guid: randy.NextGuid(),
                                degree: keyCount,
                                seed: randy.NextInt(),
                                deletionRate: DeletionRate,
                                insertionRate: InsertionRate,
                                mutationRate: MutationRate

                            ).Make()
                    ).ToList();

                return legacies.Concat(mutants).ToDictionaryIgnoreDupes(g => g.Guid);
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

        private readonly 
            Func<
                    IReadOnlyDictionary<Guid, IPhenotypeEval>, 
                    int, 
                    IReadOnlyDictionary<Guid, IGenome>
                >
            _nextGenerator;
        public Func<
                     IReadOnlyDictionary<Guid, IPhenotypeEval>,
                     int,
                     IReadOnlyDictionary<Guid, IGenome>
                    >
                    NextGenerator
        {
            get { return _nextGenerator; }
        }
    }
}
