using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace MathUtils.Collections
{
    public interface IPermutation : IEquatable<IPermutation>
    {
        int Degree { get; }
        int Value(int index);
        int IndexOf(int value);
        IEnumerable<int> Values { get; } 
    }

    public static class Permutation
    {
        private static readonly IList<IReadOnlyList<int>> identityPermutations;
        private const int MaxDegree = 32;

        static Permutation()
        {
            identityPermutations = Enumerable.Range(0, MaxDegree + 1)
                .Select<int, IReadOnlyList<int>>(i => null).ToList();

            for (var i = 1; i < MaxDegree + 1; i++)
            {
                identityPermutations[i] = Enumerable.Range(0, i).ToArray();
            }
        }

        public static IEnumerable<uint> ToPermutationBlocks(this IRando rando, int degree)
        {
            IReadOnlyList<int> singles = Enumerable.Range(0, degree).ToList();

            while (true)
            {
                var scrambles = singles.FisherYatesShuffle(rando);
                foreach (var item in scrambles)
                {
                    yield return (uint) item;
                }
            }
        }

        public static IPermutation ToPermutation(this IReadOnlyList<int> values)
        {
            return new PermuationImpl
                (
                    values: values.ToArray()
                );
        }

        public static IPermutation ToPermutation(this IReadOnlyList<uint> values)
        {
            return new PermuationImpl
                (
                    values: values.Select(v => (int)v).ToArray()
                );
        }


        public static IEnumerable<IPermutation> ToPermutations(this IRando rando, int degree)
        {
            while (true)
            {
                yield return new PermuationImpl
                        (
                            values: identityPermutations[degree].FisherYatesShuffle(rando)
                        );
            }
        }

        public static IEnumerable<IPermutation> ToPermutations(this IEnumerable<uint> sequence, int degree)
        {
            return sequence.Select(v=>(int)v).Chunk(degree).Select(c => new PermuationImpl(c));
        }

        public static IPermutation Mutate(this IPermutation permutation, IRando rando, double mutationRate)
        {
            return new PermuationImpl
                (
                    values: permutation.Values
                                    .ToList().FisherYatesPartialShuffle(rando, mutationRate) 
                );
        }

        public static IPermutation Inverse(this IPermutation permutation)
        {
            return new PermuationImpl
                (
                    values: Enumerable.Range(0, permutation.Degree)
                                      .Select(permutation.IndexOf)
                                      .ToArray()
                );
        }

        public static IPermutation Compose(this IPermutation lhs, IPermutation rhs)
        {
            return new PermuationImpl
                (
                    values: Enumerable.Range(0, lhs.Degree)
                                      .Select(i=>rhs.Value(lhs.Value(i)))
                                      .ToArray()
                );
        }

        public static bool IsEqualTo(this IPermutation lhs, IPermutation rhs)
        {
            if (lhs.Degree != rhs.Degree)
            {
                return false;
            }
            for (var i = 0; i < lhs.Degree; i++)
            {
                if (rhs.Value(i) != lhs.Value(i))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsUnit(this IPermutation permutation)
        {
            for (var i = 0; i < permutation.Degree; i++)
            {
                if (permutation.Value(i) != i)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(this IPermutation permutation)
        {
            var counts = new int[permutation.Degree];
            for (var i = 0; i < permutation.Degree; i++)
            {
                var indexValue = permutation.Value(i);

                if ((indexValue < 0) || (indexValue >= permutation.Degree))
                {
                    return false;
                }

                if (counts[indexValue] > 0)
                {
                    return false;
                }
                counts[indexValue] = 1;
            }
            return true;
        }

        public static IEnumerable<IPermutation> Orbits(this IPermutation seed)
        {
            var curVal = seed;
            while (true)
            {
                yield return curVal;

                curVal = curVal.Compose(seed);
            }
        }

        public static IEnumerable<IPermutation> Orbit(this IPermutation seed)
        {
            var curVal = seed;
            var dict = new Dictionary<IPermutation, int>(new MyPermutationEqualityComparer());
            while (true)
            {
                if (dict.ContainsKey(curVal))
                {
                    yield break;
                }
                dict[curVal] = 1;
                yield return curVal;
                curVal = curVal.Compose(seed);
            }
        }

    }


    public class PermuationImpl : IPermutation
    {
        public PermuationImpl(int[] values)
        {
            _values = values;
        }

        public int Degree
        {
            get { return _values.Length; }
        }

        private readonly int[] _values;
        public int Value(int index)
        {
            return _values[index];
        }

        public int IndexOf(int value)
        {
            for (var i = 0; i < Degree; i++)
            {
                if (_values[i] == value)
                {
                    return i;
                }
            }
            throw new Exception("permutation value not found");
        }

        public IEnumerable<int> Values
        {
            get { return _values; }
        }

        public bool Equals(IPermutation other)
        {
            for (var i = 0; i < Degree; i++)
            {
                if (Value(i) != other.Value(i))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class MyPermutationEqualityComparer : IEqualityComparer<IPermutation>
    {
        public bool Equals(IPermutation x, IPermutation y)
        {
            for (var i = 0; i < x.Degree; i++)
            {
                if (x.Value(i) != y.Value(i))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(IPermutation obj)
        {
            unchecked
            {
                var hashCode = obj.Value(0);
                for (var i = 1; i < obj.Degree; i++)
                {
                    hashCode = ((obj.Value(i) + 1) * 397) ^ hashCode;
                }

                return hashCode;
            }
        }

    }
}
