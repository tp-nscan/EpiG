//using System;
//using Genomic.Genomes.Builders;
//using Genomic.Genomes.Parsers;
//using MathUtils.Collections;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Genomic.Test.Genomes.Builders
//{
//    [TestClass]
//    public class PermutationGenomeBuilderFixture
//    {
//        [TestMethod]
//        public void TestMakeRandom()
//        {
//            const int seed = 123;
//            const int degree = 16;
//            const int targetPermutationCount = 50;
//            var guid = Guid.NewGuid();

//            var genomeBuilder = PermutationGenomeBuilder.MakeRandom
//                (
//                    guid: guid,
//                    seed: seed,
//                    degree: degree,
//                    targetPermutationCount: targetPermutationCount
//                );

//            Assert.AreEqual(guid, genomeBuilder.Guid);
//            Assert.AreEqual(degree, genomeBuilder.Degree);
//            Assert.AreEqual(targetPermutationCount, genomeBuilder.TargetPermutationCount);

//            var genome = genomeBuilder.Make();

//            Assert.AreEqual(targetPermutationCount*degree, genome.Sequence.Count);
//        }


//        [TestMethod]
//        public void TestMutateWithZeroRate()
//        {
//            const int builderSeed = 123;
//            const int degree = 16;
//            const int targetPermutationCount = 50;
//            var builderGuid = Guid.NewGuid();

//            var genomeBuilder = PermutationGenomeBuilder.MakeRandom
//                (
//                    guid: builderGuid,
//                    seed: builderSeed,
//                    degree: degree,
//                    targetPermutationCount: targetPermutationCount
//                );

//            var genome = genomeBuilder.Make();

//            var mutatorGuid = Guid.NewGuid();
//            const double mutationRate = 0;
//            const int mutatorSeed = 6555;

//            var genomeMutator = PermutationGenomeBuilder.MakeMutator
//                (
//                    guid: mutatorGuid,
//                    seed: mutatorSeed,
//                    degree: degree,
//                    sourceGenomeOld: genome,
//                    mutationRate: mutationRate
//                );

//            var mutatedGenome = genomeMutator.Make();

//            Assert.AreEqual(mutatorGuid, genomeMutator.Guid);
//            Assert.AreEqual(degree, genomeMutator.Degree);
//            Assert.AreEqual(targetPermutationCount, genomeMutator.TargetPermutationCount);
//            Assert.AreEqual(targetPermutationCount * degree, mutatedGenome.Sequence.Count);

//            var parser = PermutationGenomeParser.Make(degree);
//            var origBlocks = parser.GetSequenceBlocks(genome);
//            var mutatedBlocks = parser.GetSequenceBlocks(mutatedGenome);

//            for (var i = 0; i < origBlocks.Count; i++)
//            {
//                Assert.IsTrue
//                (
//                    origBlocks[i].Data.IsEqualTo
//                    (
//                        mutatedBlocks[i].Data
//                    )
//                );
//            }

//        }


//        [TestMethod]
//        public void TestMutateWithinBounds()
//        {
//            const int builderSeed = 123;
//            const int degree = 16;
//            const int targetPermutationCount = 500;
//            var builderGuid = Guid.NewGuid();

//            var genomeBuilder = PermutationGenomeBuilder.MakeRandom
//                (
//                    guid: builderGuid,
//                    seed: builderSeed,
//                    degree: degree,
//                    targetPermutationCount: targetPermutationCount
//                );

//            var genome = genomeBuilder.Make();

//            var mutatorGuid = Guid.NewGuid();
//            const double mutationRate = 0.05;
//            const int mutatorSeed = 6555;

//            var genomeMutator = PermutationGenomeBuilder.MakeMutator
//                (
//                    guid: mutatorGuid,
//                    seed: mutatorSeed,
//                    degree: degree,
//                    sourceGenomeOld: genome,
//                    mutationRate: mutationRate
//                );

//            var mutatedGenome = genomeMutator.Make();

//            Assert.AreEqual(mutatorGuid, genomeMutator.Guid);
//            Assert.AreEqual(degree, genomeMutator.Degree);
//            Assert.AreEqual(targetPermutationCount, genomeMutator.TargetPermutationCount);
//            Assert.AreEqual(targetPermutationCount * degree, mutatedGenome.Sequence.Count);

//            var parser = PermutationGenomeParser.Make(degree);
//            var origBlocks = parser.GetSequenceBlocks(genome);
//            var mutatedBlocks = parser.GetSequenceBlocks(mutatedGenome);

//            var diffCount = 0;
//            for (var i = 0; i < origBlocks.Count; i++)
//            {
//                if (
//                    origBlocks[i].Data.IsEqualTo (
//                        mutatedBlocks[i].Data
//                    )
//                   )
//                {
//                    diffCount++;
//                }
//            }

//            Assert.IsTrue(diffCount < targetPermutationCount * 0.6);

//            Assert.IsTrue(diffCount > targetPermutationCount * 0.4);

//        }

//    }
//}
