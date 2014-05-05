using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;
using Sorting.Stages;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.StagesOld
{
    public static class StageGenerator
    {
        public static ISorter ToSorter2(this IRando rando, IReadOnlyList<IKeyPair> keyPairs, int keyPairCount, int keyCount, Guid guid)
        {
            return rando.Pick(keyPairs).Take(keyPairCount).ToSorter2(guid, keyCount);
        }

        public static ISorter ToSorter2(this IRando rando, int keyCount, int keyPairCount, Guid guid)
        {
            var keyPairSet = KeyPairRepository.KeyPairSet(keyCount);
            return rando.ToSorter2(keyPairSet.KeyPairs, keyPairCount, keyCount, guid);
        }


        public static ISorter ToSorter2(this IEnumerable<IKeyPair> keyPairs, Guid guid, int keyCount)
        {
            IList<IReadOnlyList<IKeyPair>> keyPairGroups = keyPairs
                    .Slice(40)
                    .ToList();

            return StagedSorter.Make
                (
                    guid: guid,
                    keyCount: keyCount,
                    sorterStages: Enumerable.Range(0, 160)
                                    .Select(
                                    i => keyPairGroups[i].ToReducedSorterStage(SwitchableGroups[i])
                                    ).ToList()
                );
        }        
        
        private const int SwitchableGroupSize = 120;

        private static List<ISwitchableGroup<uint>> _switchableGroups;
        public static IReadOnlyList<ISwitchableGroup<uint>> SwitchableGroups
        {
            get
            {
                return _switchableGroups ?? (
                    _switchableGroups = Enumerable.Range(0, 160)
                        .Select(i => Rando.Fast(109 + i)
                            .ToSwitchableGroup<uint>(Guid.NewGuid(), 16, SwitchableGroupSize))
                        .ToList()
                );

            }
        }


        public static ISorterStage ToReducedSorterStage<T>(this IReadOnlyList<IKeyPair> keyPairs,
            ISwitchableGroup<T> switchableGroup)
        {
            var switchSet = KeyPairSwitchSet.Make<T>(switchableGroup.KeyCount);
            var dependentKeyPairs = keyPairs.ToDependentKeyPairs().ToList();
            foreach (var switchable in switchableGroup.Switchables)
            {
                var currentItem = switchable.Item;
                for (var i = 0; i < dependentKeyPairs.Count; i++)
                {
                    var currentDp = dependentKeyPairs[i];
                    if (currentDp.IsDisabled)
                    {
                        continue;
                    }
                    if (switchSet.IsSorted(currentItem))
                    {
                        break;
                    }
                    var res = switchSet.SwitchFunction(currentDp)(currentItem);
                    currentItem = res.Item1;
                    if (res.Item2)
                    {
                        if (! currentDp.IsUsed)
                        {
                            currentDp.IsUsed = true;
                            currentDp.DisableDependentKeyPairs();
                        }
                        if (switchSet.IsSorted(currentItem))
                        {
                            break;
                        }
                    }
                }
            }

            IReadOnlyList<IKeyPair> reducedKeyPairs = dependentKeyPairs.Reduce().ToList();

            return reducedKeyPairs.ToSorterStage(switchableGroup.KeyCount);
        }
    }
}
