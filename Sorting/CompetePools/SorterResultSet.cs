using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePools
{
    public interface ISorterResultSet
    {
        IEnumerable<ISortResult> SorterOnSwitchableGroups { get; }
        ISortResult SorterOnSwitchableGroup(ISwitchableGroup switchableGroup);
        ISorter Sorter { get; }
        IReadOnlyList<double> SwitchUseList { get; }
        int SwitchesUsed { get; }
    }

    public static class SorterResultSet
    {
        public static ISorterResultSet MakeSorterOnSwitchableGroups<T>
        (
            this ISorter sorter,
            IEnumerable<ISwitchableGroup<T>> switchableGroups
        )
        {
            return new SorterResultSetImpl
                (
                    sorter: sorter,
                    sorterOnSwitchableGroups: switchableGroups.Select(sorter.Sort)
                );
        }

        public static int UsedKeyPairHash(this ISortResult sortResult)
        {
            //return sorterEval.Sorter.KeyPairs.Filter(i => sorterEval.SwitchUseList[i] > 0).ToHash(k => k.Index);
            return sortResult.Sorter.KeyPairs.Where((t,i) => sortResult.SwitchUseList[i] > 0).ToHash(k => k.Index);
        }
    }

    public class SorterResultSetImpl : ISorterResultSet
    {
        public SorterResultSetImpl
        (
            ISorter sorter,
            IEnumerable<ISortResult> sorterOnSwitchableGroups
        )
        {
            _sorter = sorter;
            _sorterOnSwitchableGroups = sorterOnSwitchableGroups.ToDictionary(t => t.SwitchableGroupGuid);
            _switchUseList = _sorterOnSwitchableGroups.Values.Select(T => T.SwitchUseList).VectorSumDouble();
            _switchesUsed = SwitchUseList.Count(T => T > 0);
        }

        private readonly ISorter _sorter;
        public ISorter Sorter
        {
            get { return _sorter; }
        }

        private readonly Dictionary<Guid, ISortResult> _sorterOnSwitchableGroups;

        public ISortResult SorterOnSwitchableGroup(ISwitchableGroup switchableGroup)
        {
            return _sorterOnSwitchableGroups[switchableGroup.Guid];
        }

        public IEnumerable<ISortResult> SorterOnSwitchableGroups
        {
            get { return _sorterOnSwitchableGroups.Values; }
        }

        private readonly IReadOnlyList<double> _switchUseList;
        public IReadOnlyList<double> SwitchUseList
        {
            get { return _switchUseList; }
        }

        private readonly int _switchesUsed;
        public int SwitchesUsed
        {
            get { return _switchesUsed; }
        }
    }
}
