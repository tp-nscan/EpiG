using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace MathUtils.Collections
{
    public interface IPermutation
    {
        int Degree { get; }
        int Value(int index);
        int IndexOf(int value);
    }

    public static class Permutation
    {
        private static readonly IList<IReadOnlyList<int>> Units;
        private const int MaxDegree = 32;

        static Permutation()
        {
            Units = Enumerable.Range(0, MaxDegree + 1)
                .Select<int, IReadOnlyList<int>>(i => null).ToList();

            for (var i = 1; i < MaxDegree + 1; i++)
            {
                Units[i] = Enumerable.Range(0, i).ToArray();
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

        public static IEnumerable<int> Values(this IPermutation permutation)
        {
            for (var i = 0; i < permutation.Degree; i++)
            {
                yield return permutation.Value(i);
            }
        }

        public static IPermutation Make(IEnumerable<int> values)
        {
            var valueList = values.ToArray();
            return new PermuationImpl
                (
                    degree: valueList.Length,
                    values: valueList
                );
        }

        public static IPermutation Make(IEnumerable<uint> values)
        {
            var valueList = values.Select(v=>(int)v).ToArray();

            return new PermuationImpl
                (
                    degree: valueList.Length,
                    values: valueList
                );
        }

        public static IPermutation MakeRandom(int degree, IRando rando)
        {
            return new PermuationImpl
                (
                    degree: degree,
                    values: Units[degree].FisherYatesShuffle(rando)
                );
        }

        public static IPermutation Mutate(this IPermutation permutation, IRando rando, double mutationRate)
        {
            return new PermuationImpl
                (
                    degree: permutation.Degree,
                    values: permutation.Values().ToList().FisherYatesPartialShuffle(rando, mutationRate) 
                );
        }

        public static IPermutation Inverse(this IPermutation permutation)
        {
            return new PermuationImpl
                (
                    degree: permutation.Degree,
                    values: Enumerable.Range(0, permutation.Degree)
                                      .Select(permutation.IndexOf)
                                      .ToArray()
                );
        }

        public static IPermutation Compose(this IPermutation lhs, IPermutation rhs)
        {
            return new PermuationImpl
                (
                    degree: lhs.Degree,
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
    }

    public class PermuationImpl : IPermutation
    {
        public PermuationImpl(int degree, int[] values)
        {
            _degree = degree;
            _values = values;
        }

        private readonly int _degree;
        public int Degree
        {
            get { return _degree; }
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
    }
}
