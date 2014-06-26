using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using Sorting.KeyPairs;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.Evals
{
    public interface ISortResult
    {
        ISorter Sorter { get; }
        IReadOnlyList<double> SwitchUseList { get; }
        int SwitchableGroupCount { get; } 
        int SwitchUseCount { get; }
        Guid SwitchableGroupGuid { get; }
        bool Success { get; }
    }

    public static class SortResult
    {
        public static ISortResult ToSorterResult(this ISorter sorter)
        {
            var switchables = Switchable.AllSwitchablesForKeyCount(sorter.KeyCount).ToSwitchableGroup
                (
                    guid: SwitchableGroup.GuidOfAllSwitchableGroupsForKeyCount(sorter.KeyCount),
                    keyCount: sorter.KeyCount
                );

            return sorter.Sort(switchables);
        }

        public static ISortResult Make
        (
            ISorter sorter,
            Guid switchableGroupGuid,
            bool success,
            int switchUseCount,
            int switchableGroupCount
        )
        {
            return new SortResultImpl
                (
                    sorter: sorter,
                    switchableGroupGuid: switchableGroupGuid,
                    switchUseList: null,
                    success: success,
                    switchUseCount: switchUseCount,
                    switchableGroupCount: switchableGroupCount
                );
        }

        public static ISortResult Make
            (
                ISorter sorter,
                Guid switchableGroupGuid,
                bool success,
                IReadOnlyList<double> switchUseList,
                int switchableGroupCount
            )
        {
            return new SortResultImpl
                (
                    sorter: sorter,
                    switchableGroupGuid: switchableGroupGuid,
                    switchUseList: switchUseList, 
                    success: success,
                    switchUseCount: (switchUseList == null) ? 0 : switchUseList.Count(t => t > 0),
                    switchableGroupCount: switchableGroupCount
                );    
        }

        public static IEnumerable<IKeyPair> UsedKeyPairs(this ISortResult sortResult)
        {
            for (var dex = 0; dex < sortResult.Sorter.KeyPairCount; dex++)
            {
                if (sortResult.SwitchUseList[dex] > 0)
                {
                   yield return sortResult.Sorter.KeyPair(dex);
                }
            }
        }

        public static ISorter Reduce(this ISortResult sortResult, Guid guid)
        {
            return sortResult.UsedKeyPairs().ToSorter(sortResult.Sorter.KeyCount);
        }

        public static ulong Hash(this IReadOnlyList<int> intList, int start)
        {
            ulong i = 57;
            return intList
                        .Repeat()
                        .Skip(start)
                        .Take(30)
                        .Aggregate((ulong)377, (o, n) => (ulong)(n + 1) * (o + i++));
        }

        public static ulong Hash(this IEnumerable<int> intList)
        {
            ulong i = 57;
            return intList
                        .Aggregate((ulong)377, (o, n) => (ulong)(n + 1) * (o + i++));
        }
    }

    public class SortResultImpl : ISortResult
    {
        private readonly ISorter _sorter;

        public SortResultImpl
        (
            ISorter sorter,
            Guid switchableGroupGuid,
            IReadOnlyList<double> switchUseList, 
            bool success,
            int switchUseCount,
            int switchableGroupCount
        )
        {
            _sorter = sorter;
            _switchUseList = switchUseList;
            _success = success;
            _switchableGroupGuid = switchableGroupGuid;
            _switchUseCount = switchUseCount;
            _switchableGroupCount = switchableGroupCount;
        }

        private readonly Guid _switchableGroupGuid;
        public Guid SwitchableGroupGuid
        {
            get { return _switchableGroupGuid; }
        }

        public ISorter Sorter
        {
            get { return _sorter; }
        }

        private readonly bool _success;
        public bool Success
        {
            get { return _success; }
        }

        private readonly IReadOnlyList<double> _switchUseList;
        public IReadOnlyList<double> SwitchUseList
        {
            get { return _switchUseList; }
        }

        private readonly int _switchableGroupCount;
        public int SwitchableGroupCount
        {
            get { return _switchableGroupCount; }
        }

        private readonly int _switchUseCount;
        public int SwitchUseCount
        {
            get { return _switchUseCount; }
        }
    }
}
