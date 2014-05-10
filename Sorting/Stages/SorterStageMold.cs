using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.Stages
{
    public static class SorterStager
    {
        public static IEnumerable<ISorterStage<T>> ToSorterStages<T>(this IReadOnlyList<T> keyPairs, int keyCount)
            where T : IKeyPair
        {
            var stageMold = keyPairs.CollectStage(keyCount);
            while (stageMold.StageKeyPairs.Any())
            {
                yield return stageMold.StageKeyPairs.ToSorterStage(keyCount);
                stageMold = stageMold.RemainingKeyPairs.CollectStage(keyCount);
            }
        }

        public static SorterStageMold<T> CollectStage<T>(this IReadOnlyList<T> keyPairs, int keyCount)
            where T : IKeyPair
        {
            IImmutableList<T> remainingKeyPairs = ImmutableList<T>.Empty;
            IImmutableList<T> stageKeyPairs = ImmutableList<T>.Empty;

            var keyUsed = new bool[keyCount];
            foreach (var keyPair in keyPairs)
            {
                if (keyUsed[keyPair.LowKey])
                {
                    keyUsed[keyPair.HiKey] = true;
                    remainingKeyPairs = remainingKeyPairs.Add(keyPair);
                    continue;
                }

                if (keyUsed[keyPair.HiKey])
                {
                    keyUsed[keyPair.LowKey] = true;
                    remainingKeyPairs = remainingKeyPairs.Add(keyPair);
                    continue;
                }

                stageKeyPairs = stageKeyPairs.Add(keyPair);
                keyUsed[keyPair.LowKey] = true;
                keyUsed[keyPair.HiKey] = true;

            }

            return new SorterStageMold<T>
            (
                keyCount: keyCount,
                stageKeyPairs: stageKeyPairs,
                remainingKeyPairs: remainingKeyPairs
            );
        }


    }

    public class SorterStageMold<T> where T : IKeyPair
    {
        public SorterStageMold(int keyCount, IEnumerable<T> stageKeyPairs, IReadOnlyList<T> remainingKeyPairs)
        {
            _keyCount = keyCount;
            _stageKeyPairs = stageKeyPairs.ToList();
            _remainingKeyPairs = _remainingKeyPairs.AddRange(remainingKeyPairs);
        }

        private readonly IReadOnlyList<T> _stageKeyPairs;
        public IEnumerable<T> StageKeyPairs { get { return _stageKeyPairs; } }

        private readonly IImmutableList<T> _remainingKeyPairs = ImmutableList<T>.Empty;
        public IReadOnlyList<T> RemainingKeyPairs { get { return _remainingKeyPairs; } }


        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

    }
}
