using System;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genomic.Test.Genomes
{
    [TestClass]
    public class GenomeBuilderFixture
    {
        [TestMethod]
        public void TestGenomeGenerator()
        {
            const uint  symbolCount = 5;
            const int sequenceLength = 100;
            const int seed = 123;
            var guid = Guid.NewGuid();


            var generator = GenomeBuilderOld.MakeGenerator
                (
                    symbolCount: symbolCount,
                    sequenceLength: sequenceLength,
                    seed: seed,
                    guid: guid
                );

            var genome = generator.Make();

            Assert.AreEqual(sequenceLength, genome.Sequence.Count);
            Assert.AreEqual(guid, generator.Guid);
            Assert.AreEqual(seed, generator.Seed);

            Assert.IsFalse(genome.Sequence.Any(v => v >= symbolCount));
        }

        [TestMethod]
        public void TestGenomeMutator()
        {
            const uint symbolCount = 5;
            const int sequenceLength = 100;
            const int sourceSeed = 123;
            var sourceGuid = Guid.NewGuid();


            var generator = GenomeBuilderOld.MakeGenerator
                (
                    symbolCount: symbolCount,
                    sequenceLength: sequenceLength,
                    seed: sourceSeed,
                    guid: sourceGuid
                );

            var genome = generator.Make();

            const double deletionRate = 0.0;
            const double insertionRate = 0.0;
            const double mutationRate = 0.1;
            const int mutationSeed = 345;
            var mutationGuid = Guid.NewGuid();

            var genomeMutator = GenomeBuilderOld.MakeMutator
                (
                    sourceGenomeOld: genome,
                    symbolCount: symbolCount,
                    seed: mutationSeed,
                    deletionRate: deletionRate,
                    insertionRate: insertionRate,
                    mutationRate: mutationRate,
                    guid: mutationGuid
                );

            var mutatedGenome = genomeMutator.Make();

            Assert.AreEqual(sequenceLength, mutatedGenome.Sequence.Count);
            Assert.AreEqual(sourceGuid, genomeMutator.Guid);
            Assert.AreEqual(sourceSeed, genomeMutator.Seed);

            Assert.IsFalse(genome.Sequence.Any(v => v >= symbolCount));
        }
    }
}
