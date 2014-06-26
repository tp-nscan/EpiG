using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.Evals;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.Test.Evals
{
    [TestClass]
    public class SorterEvalFixture
    {
        [TestMethod]
        public void TestAStagedSorter()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 14;
            const int keyPairCount = 600;
            const int switchableCount = 2000;

            var sorter = Rando.Fast(1243).ToSorter(keyCount, keyPairCount);
            var switchableGroup = Rando.Fast(1234).ToSwitchableGroup<uint>(Guid.NewGuid(), keyCount, switchableCount);

            stopwatch.Start();

            var sorterTestOnSwitchableGroup = sorter.Sort(switchableGroup);

            stopwatch.Stop();

            var reducedSorter = sorterTestOnSwitchableGroup.Reduce(Guid.NewGuid());
        }

        [TestMethod]
        public void SorterEvalComparison()
        {

           var sucessfulEval = new SorterEvalImpl
            (
                switchEvals: Enumerable.Empty<ISwitchEval>(), 
                keyCount: 0, 
                switchableGroupId: Guid.Empty, 
                switchUseCount: 100,
                switchableGroupCount: 0, 
                success: true
            );

           var sucessfulEvalBetter = new SorterEvalImpl
            (
                switchEvals: Enumerable.Empty<ISwitchEval>(),
                keyCount: 0,
                switchableGroupId: Guid.Empty,
                switchUseCount: 90,
                switchableGroupCount: 0,
                success: true
            );

           var sucessfulEvalBest = new SorterEvalImpl
            (
                switchEvals: Enumerable.Empty<ISwitchEval>(),
                keyCount: 0,
                switchableGroupId: Guid.Empty,
                switchUseCount: 80,
                switchableGroupCount: 0,
                success: true
            );

           var failedEval = new SorterEvalImpl
            (
                switchEvals: Enumerable.Empty<ISwitchEval>(),
                keyCount: 0,
                switchableGroupId: Guid.Empty,
                switchUseCount: 70,
                switchableGroupCount: 0,
                success: false
            );

           var failedEvalBetter = new SorterEvalImpl
            (
                switchEvals: Enumerable.Empty<ISwitchEval>(),
                keyCount: 0,
                switchableGroupId: Guid.Empty,
                switchUseCount: 60,
                switchableGroupCount: 0,
                success: false
            );

            var evalList = new List<ISorterEval>
            {
                sucessfulEval,
                failedEval,
                sucessfulEvalBest,
                sucessfulEvalBetter,
                failedEvalBetter
            };

            var sortedEvals = evalList.OrderBy(t => t).ToList();
            Assert.AreEqual(80, sortedEvals[0].SwitchUseCount);
            Assert.AreEqual(90, sortedEvals[1].SwitchUseCount);
            Assert.AreEqual(100, sortedEvals[2].SwitchUseCount);
            Assert.AreEqual(60, sortedEvals[3].SwitchUseCount);
            Assert.AreEqual(70, sortedEvals[4].SwitchUseCount);
        }
    }
}
