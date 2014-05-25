using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.KeyPairs;
using Sorting.Stages;

namespace Sorting.Test.Stages
{
    /// <summary>
    /// Summary description for SorterStage
    /// </summary>
    [TestClass]
    public class SorterStageFixture
    {
        [TestMethod]
        public void TestCollectStageWhenEmpty()
        {
            const int keyCount = 16;
            var stage = ImmutableList<IKeyPair>.Empty.CollectStage(keyCount);

            Assert.AreEqual(keyCount, stage.KeyCount);
            Assert.AreEqual(0, stage.StageKeyPairs.Count());
            Assert.AreEqual(0, stage.RemainingKeyPairs.Count());
        }



        [TestMethod]
        public void TestCollectStageWithSingleKeyPair()
        {
            const int keyCount = 16;
            var stage = ImmutableList<IKeyPair>.Empty.AddRange(SingleKeyPair).CollectStage(keyCount);

            Assert.AreEqual(keyCount, stage.KeyCount);
            Assert.AreEqual(1, stage.StageKeyPairs.Count());
            Assert.AreEqual(0, stage.RemainingKeyPairs.Count());
        }

        [TestMethod]
        public void TestCollectStageWithManyKeyPairs()
        {
            const int keyCount = 16;
            const int keyPairCount = 160;
            var stage = ImmutableList<IKeyPair>.Empty.AddRange
                (
                   Rando.Fast(123).RandomKeyPairs
                    (
                        keyCount: keyCount
                    ).Take(keyPairCount)
                )
                .CollectStage(keyCount);

            Assert.AreEqual(keyPairCount, stage.StageKeyPairs.Count() + stage.RemainingKeyPairs.Count());

        }

        [TestMethod]
        public void TestCollectAllStagesWithManyKeyPairs()
        {
            const int keyCount = 16;
            const int keyPairCount = 160;
            var stage = ImmutableList<IKeyPair>.Empty.AddRange
                (
                    Rando.Fast(123).RandomKeyPairs
                    (
                        keyCount: keyCount
                    ).Take(keyPairCount)
                ).CollectStage(keyCount);

            Assert.AreEqual(keyPairCount, stage.StageKeyPairs.Count() + stage.RemainingKeyPairs.Count());
        }

        public static IEnumerable<IKeyPair> SingleKeyPair
        {
            get { yield return KeyPairRepository.ForKeys(0, 1); }
        }

    }


}
