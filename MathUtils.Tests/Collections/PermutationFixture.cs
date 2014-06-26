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
            const int degree = 16;
            const int seed = 123;

            var permutation = Rando.Fast(seed).ToPermutations(degree).Single();

            Assert.AreEqual(degree, permutation.Degree);
        }

        [TestMethod]
        public void TestAreEqualWhenEqual()
        {
            const int degree = 16;
            const int seed = 123;

            var permutation = Rando.Fast(seed).ToPermutations(degree).Single();
            var permutationCopy = Permutation.ToPermutation(permutation.Values.ToList());

            Assert.IsTrue(permutation.IsEqualTo(permutationCopy));
        }

        [TestMethod]
        public void TestAreNotEqualWhenNotEqual()
        {
            const int degree = 16;
            const int seed = 123;

            var permutation = Rando.Fast(seed).ToPermutations(degree).Single();
            var permutationReverse = Permutation.ToPermutation(permutation.Values.Reverse().ToList());

            Assert.IsFalse(permutation.IsEqualTo(permutationReverse));
        }

        [TestMethod]
        public void TestInverse()
        {
            const int degree = 16;
            const int seed = 123;

            var permutation = Rando.Fast(seed).ToPermutations(degree).Single();
            var permutationInverse = permutation.Inverse();

            Assert.AreEqual(degree, permutationInverse.Degree);

            Assert.IsTrue(permutation.Compose(permutationInverse).IsUnit());

        }

        [TestMethod]
        public void TestValidPermutationIsValid()
        {
            const int degree = 16;
            const int seed = 123;
            for (var i = 0; i < 10; i++)
            {
                var permutation = Rando.Fast(seed + i).ToPermutations(degree).Single(); ;
                Assert.IsTrue(permutation.IsValid());

            }
        }

        [TestMethod]
        public void TestInValidPermutationsAreNotValid()
        {
            var permutation = Permutation.ToPermutation(new[] { 1, 2, 3, 3 });
            Assert.IsFalse(permutation.IsValid());

            permutation = Permutation.ToPermutation(new[] { 1, 2, 3, 4 });
            Assert.IsFalse(permutation.IsValid());

            permutation = Permutation.ToPermutation(new[] { 0, 0, 3, 1 });
            Assert.IsFalse(permutation.IsValid());

            permutation = Permutation.ToPermutation(new[] { 4, 2, 4, 3 });
            Assert.IsFalse(permutation.IsValid());

            permutation = Permutation.ToPermutation(new[] { 0, 2, 3, 3 });
            Assert.IsFalse(permutation.IsValid());
        }
    }
}
