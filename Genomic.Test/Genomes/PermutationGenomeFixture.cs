using System;
using System.Linq;
using Genomic.Genomes.Builders;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genomic.Test.Genomes
{
    [TestClass]
    public class PermutationGenomeFixture
    {
        [TestMethod]
        public void TestToPermutationGenomeBuilderRandom()
        {
            const int degree = 10;
            const int permutationCount = 100;
            const int builderCount = 10;
            const int seed = 123;

            var permutationGenome = Rando.Fast(seed)
                .ToPermutationGenomeBuilders(degree, builderCount, permutationCount)
                .Single()
                .Make();

            Assert.AreEqual(degree * permutationCount, permutationGenome.SequenceLength);
        }

        [TestMethod]
        public void TestPermutationMutateWithZeroMutationRate()
        {
            const int degree = 10;
            const int permutationCount = 100;
            const double deletionRate = 0;
            const double insertionRate = 0;
            const double mutationRate = 0;
            const int builderCount = 10;
            const int seed = 123;
            Guid guid = Guid.NewGuid();

            var permutationGenome = Rando.Fast(seed)
                .ToPermutationGenomeBuilders(degree, builderCount, permutationCount)
                .Single()
                .Make();


            var builder = permutationGenome.ToPermutationMutatorBuilder
                (
                    guid: guid,
                    seed: seed,
                    degree: degree,
                    permutationCount: permutationCount,
                    mutationRate: mutationRate,
                    insertionRate: insertionRate,
                    deletionRate: deletionRate
                );

            var mutantGenome = builder.Make();


            Assert.AreEqual(guid, mutantGenome.Guid);
            Assert.AreEqual(permutationGenome.SequenceLength, mutantGenome.SequenceLength);
            Assert.AreEqual(0, permutationGenome.Sequence.GetDiffs(mutantGenome.Sequence).ToList().Count);


        }


        [TestMethod]
        public void TestPermutationMutateWithNonZeroMutationRate()
        {
            const int degree = 10;
            const int permutationCount = 100;
            const int builderCount = 10; 
            const double deletionRate = 0;
            const double insertionRate = 0;
            const double mutationRate = 0.1;
            const int seed = 12837;
            Guid guid = Guid.NewGuid();

            var permutationGenome = Rando.Fast(seed)
                .ToPermutationGenomeBuilders(degree, builderCount, permutationCount)
                .Single()
                .Make();


            var builder = permutationGenome.ToPermutationMutatorBuilder
                (
                    guid: guid,
                    seed: seed,
                    mutationRate: mutationRate,
                    insertionRate: insertionRate,
                    deletionRate: deletionRate,
                    degree: degree,
                    permutationCount: permutationCount
                );

            var mutantGenome = builder.Make();


            Assert.AreEqual(guid, mutantGenome.Guid);
            Assert.AreEqual(permutationGenome.SequenceLength, mutantGenome.SequenceLength);
            var diffs= permutationGenome.Sequence.GetDiffs(mutantGenome.Sequence).ToList();


        }
    }
}
