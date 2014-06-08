using System;
using System.Collections.Generic;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Collections
{
    [TestClass]
    public class GuidFixture
    {
        [TestMethod]
        public void TestMakeGuidHash()
        {
            var result = Guid.NewGuid().ToHash();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGuidHashErgodictiy()
        {
            var guidsAndKeys = new Dictionary<Guid, int>();
            var curGuid = Guid.NewGuid();

            for (var i = 0; i < 100000; i++)
            {
                var curSeed = curGuid.ToHash();
                guidsAndKeys[curGuid] = curSeed;
                var randy = Rando.Fast(curSeed);
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
            }
        }

        [TestMethod]
        public void TestGuidFromTail()
        {
            const int testCount = 1000;
            var guidsAndKeys = new Dictionary<Guid, int>();
            var randy = Rando.Fast(123);
            var ints = new List<int>();


            for (var i = 0; i < testCount; i++)
            {
                ints.Add(randy.NextInt());
                var curGuid = ints.TailToGuid();
                guidsAndKeys[curGuid] = 1;
            }

            var dupes = guidsAndKeys.Count;

        }

        [TestMethod]
        public void TestGuidAddGuid()
        {
            var lhs = Guid.NewGuid();
            var rhs = Guid.NewGuid();
            var result = lhs.Add(rhs);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGuidAddUint()
        {
            var lhs = Guid.NewGuid();
            const uint rhs = 1234567;

            var result = lhs.Add(rhs);

            Assert.IsNotNull(result);
        }
    }
}
