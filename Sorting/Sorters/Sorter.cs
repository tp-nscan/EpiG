using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.KeyPairs;

namespace Sorting.Sorters
{
    public interface ISorter
    {
        int KeyCount { get; }
        int KeyPairCount { get; }
        IKeyPair KeyPair(int index);
        IReadOnlyList<IKeyPair> KeyPairs { get; }
    }

    public static class Sorter
    {
        public static ISorter ToSorter(this IEnumerable<IKeyPair> keyPairs, int keyCount)
        {
            return new SorterImpl(keyPairs, keyCount);
        }

        public static ISorter ToSorter(this IReadOnlyList<IKeyPair> keyPairs, IEnumerable<uint> keyPairChoices, int keyCount)
        {
            return keyPairs.PickMembers(keyPairChoices).ToSorter(keyCount);
        }

        public static ISorter ToSorter(this IRando rando, int keyCount, int keyPairCount)
        {
           //return rando.RandomKeyPairs(keyCount).Take(keyPairCount).ToSorter(keyCount);
           return rando.RandomKeyPairsFullStage(keyCount).Take(keyPairCount).ToSorter(keyCount);
        }

        public static ISorter ToSorter(this IRando rando, IReadOnlyList<IKeyPair> keyPairs, int keyPairCount, int keyCount)
        {
            return rando.Pick(keyPairs).Take(keyPairCount).ToSorter(keyCount);
        }
 
        public static IEnumerable<ISorter> Mutate(this ISorter sorter, IRando rando, double mutationRate)
        {
            var randoK = rando.Spawn();
            var keyPairList = KeyPairRepository.KeyPairSet(sorter.KeyCount).KeyPairs;
            var newb = randoK.Pick(keyPairList).ToMoveNext();
            while (true)
            {
                yield return new SorterImpl
                    (
                        keyPairs: sorter.KeyPairs.MutateInsertDelete
                        (
                             rando.Spawn().ToBoolEnumerator(mutationRate),       
                             rando.Spawn().ToBoolEnumerator(mutationRate),
                             rando.Spawn().ToBoolEnumerator(mutationRate),
                             T => newb.Next(),
                             T => newb.Next(),
                             T => newb.Next()
                        ),
                        keyCount: sorter.KeyCount
                    );
            }
        }
    }

    class SorterImpl : ISorter
    {
        public SorterImpl(IEnumerable<IKeyPair> keyPairs, int keyCount)
        {
            _keyCount = keyCount;
            _keyPairs = keyPairs.ToArray();
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        public int KeyPairCount
        {
            get { return _keyPairs.Length; }
        }

        private readonly IKeyPair[] _keyPairs;
        public IKeyPair KeyPair(int index)
        {
            return _keyPairs[index];
        }

        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get { return _keyPairs; }
        }
    }
}
