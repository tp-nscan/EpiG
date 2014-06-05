using System;
using System.Collections.ObjectModel;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WpfUtils.Test
{
    [TestClass]
    public class ObservableCollectionUtilsFixture
    {
        [TestMethod]
        public void TestReplace()
        {
            var observableCol = new ObservableCollection<string>();
            observableCol.Add("zero");
            observableCol.Add("first");
            observableCol.Add("second");
            observableCol.Add("third");
            Assert.IsTrue(observableCol.IndexOf("second")==2);

            observableCol.Replace("second", "newsecond");
            Assert.IsTrue(observableCol.IndexOf("newsecond") == 2);

            observableCol.Replace("zero", "newzero");
            Assert.IsTrue(observableCol.IndexOf("newzero") == 0);
        }

        [TestMethod]
        public void TestInsertWhen()
        {
            var testColl = new ObservableCollection<int>();
            var a = 1;
            testColl.InsertWhen(a, t => t > a);
            a = 26;
            testColl.InsertWhen(a, t => t > a);
            a = 4;
            testColl.InsertWhen(a, t => t > a);
            a = 19;
            testColl.InsertWhen(a, t => t > a);
            a = 7;
            testColl.InsertWhen(a, t => t > a);
        }

        [TestMethod]
        public void TestOrderedInsertWhenEmpty()
        {
            var testColl = new ObservableCollection<int>();
            
            testColl.OrderedInsert(1, null, 1);
            Assert.AreEqual(1, testColl.Count);
        }

        [TestMethod]
        public void TestOrderedInsertWhenThereIsRoom()
        {
            var testColl = new ObservableCollection<int> {1};

            testColl.OrderedInsert(2, (a,b)=>false, 2);

            Assert.AreEqual(2, testColl.Count);
        }


        [TestMethod]
        public void TestOrderedInserts()
        {
            const int totalListSize = 10000;
            const int maxSortedListSize = 1000;

            var testColl = new ObservableCollection<int>();
            var randy = Rando.Fast(13);

            var totalList = Enumerable.Range(0, totalListSize)
                                      .Select(_ => randy.NextInt(1000))
                                      .ToArray();


            for (var i = 0; i < totalListSize; i++)
            {
                testColl.OrderedInsert(totalList[i], (a, b) => a > b, maxSortedListSize);
            }


            Assert.AreEqual(maxSortedListSize, testColl.Count);
        }

    }
}
