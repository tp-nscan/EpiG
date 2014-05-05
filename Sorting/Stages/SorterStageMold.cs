using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.Stages
{
    public static class SorterStager
    {
        public static IEnumerable<ISorterStage> ToSorterStages<T>(this IReadOnlyList<T> keyPairs, int keyCount)
            where T : IKeyPair
        {
            var stageMold = keyPairs.CollectStage(keyCount);
            while (stageMold.StageKeyPairs.Any())
            {
                yield return stageMold.StageKeyPairs.ToSorterStage(keyCount);
                stageMold = stageMold.RemainingKeyPairs.CollectStage(keyCount);
            }
        }

        public static SorterStageMold CollectStage<T>(this IReadOnlyList<T> keyPairs, int keyCount)
            where T : IKeyPair
        {
            IImmutableList<IKeyPair> remainingKeyPairs = ImmutableList<IKeyPair>.Empty;
            IImmutableList<IKeyPair> stageKeyPairs = ImmutableList<IKeyPair>.Empty;

            var keyUsed = new bool[keyCount];
            var keysRemaining = keyCount;
            foreach (var keyPair in keyPairs)
            {
                if (keyUsed[keyPair.LowKey])
                {
                    keyUsed[keyPair.HiKey] = true;
                    keysRemaining--;
                    remainingKeyPairs = remainingKeyPairs.Add(keyPair);
                    continue;
                }

                if (keyUsed[keyPair.HiKey])
                {
                    keyUsed[keyPair.LowKey] = true;
                    keysRemaining--;
                    remainingKeyPairs = remainingKeyPairs.Add(keyPair);
                    continue;
                }

                stageKeyPairs = stageKeyPairs.Add(keyPair);
                keyUsed[keyPair.LowKey] = true;
                keyUsed[keyPair.HiKey] = true;
                keysRemaining--;
                keysRemaining--;

            }

            return new SorterStageMold
            (
                keyCount: keyCount,
                stageKeyPairs: stageKeyPairs,
                remainingKeyPairs: remainingKeyPairs
            );
        }


    }

    public class SorterStageMold
    {
        public SorterStageMold(int keyCount, IEnumerable<IKeyPair> stageKeyPairs, IReadOnlyList<IKeyPair> remainingKeyPairs)
        {
            _keyCount = keyCount;
            _stageKeyPairs = stageKeyPairs.ToList();
            _remainingKeyPairs = _remainingKeyPairs.AddRange(remainingKeyPairs);
        }

        private readonly IReadOnlyList<IKeyPair> _stageKeyPairs;
        public IEnumerable<IKeyPair> StageKeyPairs { get { return _stageKeyPairs; } }

        private readonly IImmutableList<IKeyPair> _remainingKeyPairs = ImmutableList<IKeyPair>.Empty;
        public IReadOnlyList<IKeyPair> RemainingKeyPairs { get { return _remainingKeyPairs; } }


        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

    }
}
