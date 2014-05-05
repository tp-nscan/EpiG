using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sorting.Sorters;
using Sorting.Stages;
using Sorting.StagesOld;

namespace Sorting.Test.StagesOld
{
    [TestClass]
    public class StagedSorterFixture
    {
        [TestMethod]
        public void TestAStagedSorter()
        {
            const int keyCount = 14;
            const int keyPairCount = 60;

            var sorter = Rando.Fast(1243).ToSorter(keyCount, keyPairCount, Guid.NewGuid());

            var stagedSorter = sorter.ToStagedSorter();

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(sorter.KeyPairs.Select(kp => kp.Index).ToList()));

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(stagedSorter.KeyPairs.Select(kp => kp.Index).ToList()));

            Assert.AreEqual(stagedSorter.KeyPairCount, sorter.KeyPairCount);
            
        }

        [TestMethod]
        public void TestDebugFormat()
        {
            const int keyCount = 14;
            const int keyPairCount = 60;

            var sorter = Rando.Fast(1243).ToSorter(keyCount, keyPairCount, Guid.NewGuid());

            var stagedSorter = sorter.ToStagedSorter();

            System.Diagnostics.Debug.WriteLine(stagedSorter.DebugFormat());

        }

        [TestMethod]
        public void StagingConservesKeyPairs()
        {
            const int keyCount = 14;
            const int keyPairCount = 60;

            var sorter = Rando.Fast(1243).ToSorter(keyCount, keyPairCount, Guid.NewGuid());

            var stagedSorter = sorter.ToStagedSorter();

            var h1 = keyCount/2;
            h1 = (keyCount -1) / 2;
            h1 = (keyCount - 2) / 2;

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(sorter.KeyPairs.Select(kp => kp.Index).ToList()));

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(stagedSorter.KeyPairs.Select(kp => kp.Index).ToList()));

            Assert.AreEqual(stagedSorter.KeyPairCount, sorter.KeyPairCount);

        }
    }
}
