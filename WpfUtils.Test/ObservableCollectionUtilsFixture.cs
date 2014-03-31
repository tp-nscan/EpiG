using System.Collections.ObjectModel;
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

    }
}
