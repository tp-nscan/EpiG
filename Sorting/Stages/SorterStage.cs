using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.Stages
{
    public interface ISorterStage<T> where T: IKeyPair
    {
        int KeyCount { get; }
        int KeyPairCount { get; }
        T KeyPair(int index);
        IReadOnlyList<T> KeyPairs { get; }
    }

    public static class SorterStage
    {
        public static ISorterStage<T> ToSorterStage<T>(this IEnumerable<T> keyPairs, int keyCount)
            where T : IKeyPair
        {
            return new SorterStageImpl<T>(keyCount, keyPairs.OrderBy(kp => kp.Index).ToList());
        }
    }

    class SorterStageImpl<T> : ISorterStage<T> where T: IKeyPair
    {
        public SorterStageImpl(int keyCount, IReadOnlyList<T> keyPairs)
        {
            _keyCount = keyCount;
            _keyPairs =  _keyPairs.AddRange(keyPairs.OrderBy(kp=>kp.Index));
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        public int KeyPairCount
        {
            get { return _keyPairs.Count; }
        }

        public T KeyPair(int index)
        {
            return _keyPairs[index];
        }

        public IReadOnlyList<T> KeyPairs
        {
            get { return _keyPairs; }
        }

        private readonly IImmutableList<T> _keyPairs = ImmutableList<T>.Empty;
    }
}
