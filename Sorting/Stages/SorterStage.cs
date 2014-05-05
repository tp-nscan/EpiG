using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.Stages
{
    public interface ISorterStage
    {
        int KeyCount { get; }
        int KeyPairCount { get; }
        IKeyPair KeyPair(int index);
        IReadOnlyList<IKeyPair> KeyPairs { get; }
    }

    public static class SorterStage
    {
        public static ISorterStage ToSorterStage(this IEnumerable<IKeyPair> keyPairs, int keyCount)
        {
            return new SorterStageImpl(keyCount, keyPairs.OrderBy(kp => kp.Index).ToList());
        }
    }

    class SorterStageImpl : ISorterStage
    {
        public SorterStageImpl(int keyCount, IReadOnlyList<IKeyPair> keyPairs)
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

        public IKeyPair KeyPair(int index)
        {
            return _keyPairs[index];
        }

        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get { return _keyPairs; }
        }

        private readonly IImmutableList<IKeyPair> _keyPairs = ImmutableList<IKeyPair>.Empty;
    }
}
