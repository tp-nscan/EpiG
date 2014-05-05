using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.KeyPairs;
using Sorting.Stages;

namespace Sorting.Test.KeyPairs
{
    [TestClass]
    public class KeyPairsFixture
    {
        [TestMethod]
        public void TestRandomFullStage()
        {
            const int keyCount = 16;
            IReadOnlyList<int> keys = Enumerable.Range(0, keyCount).ToList();
            var keyPairs = keys.RandomFullStage(Rando.Fast(111));

            var stages = keyPairs.ToSorterStages(keyCount);

            Assert.AreEqual(1, stages.Count());
        }

        [TestMethod]
        public void TestRandomFullStages()
        {
            const int keyCount = 64;
            const int stageCount = 5;

            var keyPairs = Rando.Fast(1111).RandomFullStages(keyCount)
                                      .Take(stageCount)
                                      .SelectMany(s => s)
                                      .ToList();

            var stages = keyPairs.ToSorterStages(keyCount)
                            .ToList();

            Assert.AreEqual(stageCount, stages.Count());
            Assert.AreEqual(stages.SelectMany(s => s.KeyPairs).Count(), keyPairs.Count());
        }
    }
}
