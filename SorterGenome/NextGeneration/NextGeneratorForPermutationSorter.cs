using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using MathUtils.Collections;
using MathUtils.Rand;
using SorterGenome.PhenotypeEvals;

namespace SorterGenome.NextGeneration
{
    public class NextGeneratorForPermutationSorter
    {
        public NextGeneratorForPermutationSorter(
            int keyCount, 
            int orgCount,
            double deletionRate, 
            double insertionRate,
            double mutationRate,
            int legacyCount,
            int cubCount
         )
        {
            _keyCount = keyCount;
            _orgCount = orgCount;
            _mutationRate = mutationRate;
            _legacyCount = legacyCount;
            _cubCount = cubCount;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;

            _nextGenerator = (eD, i) =>
            {
                var randy = Rando.Fast(i);

                var leaderBoard =
                    eD.Values
                    .Where(ev=>ev.SorterEval.Success)
                    .OrderBy(v => v.SorterEval)
                        .Select(ev => ev.SorterPhenotypeEvalBuilder
                                        .SorterPhenotype
                                        .SorterPhenotypeBuilder
                                        .Genome
                            )
                        .ToList();

                var legacies = leaderBoard.Take(LegacyCount).ToList();

                var mutants =
                    leaderBoard.Take(CubCount)
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

        private readonly int _legacyCount;
        public int LegacyCount
        {
            get { return _legacyCount; }
        }

        private readonly int _cubCount;
        public int CubCount
        {
            get { return _cubCount; }
        }

        private readonly 
            Func<
                    IReadOnlyDictionary<Guid, ISorterPhenotypeEval>, 
                    int, 
                    IReadOnlyDictionary<Guid, IGenome>
                >
            _nextGenerator;
        public Func<
                     IReadOnlyDictionary<Guid, ISorterPhenotypeEval>,
                     int,
                     IReadOnlyDictionary<Guid, IGenome>
                    >
                    NextGenerator
        {
            get { return _nextGenerator; }
        }
    }
}
