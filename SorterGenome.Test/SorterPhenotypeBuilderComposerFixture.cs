using System;
using System.Linq;
using Genomic.Genomes.Builders;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterGenome.Phenotypes;

namespace SorterGenome.Test
{
    [TestClass]
    public class SorterPhenotypeBuilderComposerFixture
    {
        [TestMethod]
        public void TestBuilder()
        {
            const int degree = 10;
            const int permutationCount = 100;
            const int builderCount = 1;
            const int seed = 123;
            const int skips = 4;

            var builders = Rando.Fast(seed)
                .ToPermutationGenomeBuilders
                (
                    degree: degree,
                    builderCount: builderCount,
                    permutationCount: permutationCount
                )
                .ToList();

            var builderGuid = Guid.NewGuid();

            var sbc = new SorterPhenotypeBuilderComposer
                (
                    guid: builderGuid,
                    genome: builders[0].Make(),
                    keyCount: degree, 
                    skips:0
                );

            var phenoGuid = Guid.NewGuid();

            var phenotype = sbc.Make(phenoGuid);



        }
    }
}
