using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Collections
{
    /// <summary>
    /// Summary description for PermutationFixture
    /// </summary>
    [TestClass]
    public class PermutationFixture
    {
        [TestMethod]
        public void TestMake()
        {
            const int Degree = 16;
            const int Seed = 123;

            var permutation = Permutation.MakeRandom(degree: Degree, rando: Rando.Fast(Seed));

            Assert.AreEqual(Degree, permutation.Degree);
        }

        [TestMethod]
        public void TestAreEqualWhenEqual()
        {
            const int Degree = 16;
            const int Seed = 123;

            var permutation = Permutation.MakeRandom(degree: Degree, rando: Rando.Fast(Seed));
            var permutationCopy = Permutation.Make(permutation.Values());

            Assert.IsTrue(permutation.IsEqualTo(permutationCopy));
        }

        [TestMethod]
        public void TestAreNotEqualWhenNotEqual()
        {
            const int Degree = 16;
            const int Seed = 123;

            var permutation = Permutation.MakeRandom(degree: Degree, rando: Rando.Fast(Seed));
            var permutationReverse = Permutation.Make(permutation.Values().Reverse());

            Assert.IsFalse(permutation.IsEqualTo(permutationReverse));
        }

        [TestMethod]
        public void TestInverse()
        {
            const int Degree = 16;
            const int Seed = 123;

            var permutation = Permutation.MakeRandom(degree: Degree, rando: Rando.Fast(Seed));
            var permutationInverse = permutation.Inverse();

            Assert.AreEqual(Degree, permutationInverse.Degree);

            Assert.IsTrue(permutation.Compose(permutationInverse).IsUnit());

        }

        [TestMethod]
        public void TestValidPermutationIsValid()
        {
            const int Degree = 16;
            const int Seed = 123;
            for (var i = 0; i < 10; i++)
            {
                var permutation = Permutation.MakeRandom(degree: Degree, rando: Rando.Fast(Seed + i));
                Assert.IsTrue(permutation.IsValid());

            }
        }

        [TestMethod]
        public void TestInValidPermutationsAreNotValid()
        {
            var permutation = Permutation.Make(new[] { 1, 2, 3, 3 });
            Assert.IsFalse(permutation.IsValid());

            permutation = Permutation.Make(new[] { 1, 2, 3, 4 });
            Assert.IsFalse(permutation.IsValid());

            permutation = Permutation.Make(new[] { 0, 0, 3, 1 });
            Assert.IsFalse(permutation.IsValid());

            permutation = Permutation.Make(new[] { 4, 2, 4, 3 });
            Assert.IsFalse(permutation.IsValid());

            permutation = Permutation.Make(new[] { 0, 2, 3, 3 });
            Assert.IsFalse(permutation.IsValid());
        }
    }
}
