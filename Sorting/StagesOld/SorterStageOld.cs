using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.StagesOld
{
    public interface ISorterStageOld
    {
        int KeyCount { get; }
        int KeyPairCount { get; }
        IKeyPair KeyPair(int index);
        IReadOnlyList<IKeyPair> KeyPairs { get; }
        ISorterStageOld AppendKeyPair(IKeyPair keyPair);
    }

    public static class SorterStageOld
    {
        static readonly ISorterStageOld emptySorterStageOld = new SorterStageOldImpl(0, Enumerable.Empty<IKeyPair>().ToList());

        public static ISorterStageOld Empty
        {
            get { return emptySorterStageOld; }
        }

        public static ISorterStageOld Make(int keyCount, IReadOnlyList<IKeyPair> keyPairs)
        {
            return new SorterStageOldImpl(keyCount, keyPairs);
        }

    }

    class SorterStageOldImpl : ISorterStageOld
    {
        public SorterStageOldImpl(int keyCount, IReadOnlyList<IKeyPair> keyPairs)
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
        public ISorterStageOld AppendKeyPair(IKeyPair keyPair)
        {
            return new SorterStageOldImpl(KeyCount, _keyPairs.Add(keyPair));
        }
    }
}
