using System;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using Genomic.Genomes.Parsers;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genomic.Test.Genomes.Parsers
{
    [TestClass]
    public class PermutationGenomeParserFixture
    {
        [TestMethod]
        public void TestGetSequenceBlocks()
        {
            const int seed = 123;
            const int degree = 16;
            const int permutationCount = 50;
            var guid = Guid.NewGuid();

            var genomeBuilder = PermutationGenomeBuilder.MakeRandom
                (
                    guid: guid,
                    seed: seed,
                    degree: degree,
                    targetPermutationCount: permutationCount
                );


            var parser = PermutationGenomeParser.Make(degree);

            var  blocks = parser.GetSequenceBlocks(genomeBuilder.Make());

            foreach (var sequenceBlock in blocks)
            {
                Assert.IsTrue(sequenceBlock.Data.IsValid());
            }
        }

        [TestMethod]
        public void TestGetGenome()
        {
            const int seed = 123;
            const int degree = 16;
            const int permutationCount = 50;
     
            var sequenceBlocks = Enumerable.Range(0, permutationCount)
                .Select(i => Permutation.MakeRandom(degree, Rando.Fast(seed))
                                        .ToSequenceBlock()
                ).ToList();

            var parser = PermutationGenomeParser.Make(degree);

            var genome = parser.GetGenome(sequenceBlocks);

            var blocksOut = parser.GetSequenceBlocks(genome);

            for (var i = 0; i < sequenceBlocks.Count; i++)
            {
                Assert.IsTrue(
                    sequenceBlocks[i].Data.IsEqualTo(
                            blocksOut[i].Data
                        )
                    );
            }

        }
    }
}
