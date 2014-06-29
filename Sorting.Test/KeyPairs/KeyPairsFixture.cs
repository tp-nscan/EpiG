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
        public void TestToKeyPairs()
        {
            const int seed = 123;
            const int keyCount = 100;
            const int maxKeyIndex = 16;

            var randy = Rando.Fast(seed);

            var testKeys = Enumerable.Range(0, keyCount)
                                     .Select(i => randy.NextInt(maxKeyIndex))
                                     .ToList();

            var keyPairs = testKeys.ToKeyPairs().ToList();

            Assert.AreEqual(keyCount/2, keyPairs.Count);
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
