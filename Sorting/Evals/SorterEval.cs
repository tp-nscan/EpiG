using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace Sorting.Evals
{
    public interface ISorterEval : ISorter, IComparable
    {
        IReadOnlyList<ISwitchEval> SwitchEvals { get; }
        bool Success { get; }
        int SwitchableGroupCount { get; }
        Guid SwitchableGroupId { get; }
        int SwitchUseCount { get; }
    }

    public static class SorterEval
    {
        public static ISorterEval ToSorterEval(this ISortResult sortResult)
        {
            return new SorterEvalImpl
                (
                    switchEvals: Enumerable.Range(0, sortResult.Sorter.KeyPairCount) 
                                            .Select
                                            (
                                                i=> new SwitchEvalImpl
                                                    (
                                                        sortResult.Sorter.KeyPair(i), 
                                                        sortResult.SwitchUseList[i]
                                                    )
                                            ),
                    keyCount: sortResult.Sorter.KeyCount,
                    switchableGroupId: sortResult.SwitchableGroupGuid,
                    switchUseCount: sortResult.SwitchUseCount,
                    switchableGroupCount:sortResult.SwitchableGroupCount,
                    success: sortResult.Success
                );
        }

        public static IEnumerable<ISwitchEval> ToUsedSwitchEvals(this ISortResult sortResult)
        {
            for (var i = 0; i < sortResult.Sorter.KeyPairCount; i++)
            {
                if (sortResult.SwitchUseList[i] > 0)
                {
                    yield return new SwitchEvalImpl(
                            sortResult.Sorter.KeyPair(i),
                            sortResult.SwitchUseList[i]
                        );
                }
            }
        }
    }

    public class SorterEvalImpl : ISorterEval
    {
        public SorterEvalImpl
            (
                IEnumerable<ISwitchEval> switchEvals, 
                int keyCount, 
                Guid switchableGroupId, 
                int switchUseCount,
                int switchableGroupCount, 
                bool success
         )
        {
            _keyCount = keyCount;
            _switchableGroupId = switchableGroupId;
            _switchUseCount = switchUseCount;
            _switchableGroupCount = switchableGroupCount;
            _success = success;
            _switchableGroupCount = switchableGroupCount;
            _switchEvals = switchEvals.ToList();
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        public int KeyPairCount
        {
            get { return _switchEvals.Count; }
        }

        public IKeyPair KeyPair(int index)
        {
            return SwitchEvals[index].KeyPair;
        }

        private List<IKeyPair> _keyPairs;
        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get 
            { 
                return _keyPairs ?? 
                    (
                        _keyPairs = _switchEvals.Select(kp => kp.KeyPair).ToList()
                    ); 
            }
        }

        private readonly IReadOnlyList<ISwitchEval> _switchEvals;
        public IReadOnlyList<ISwitchEval> SwitchEvals
        {
            get { return _switchEvals; }
        }

        private readonly bool _success;
        public bool Success
        {
            get { return _success; }
        }

        private readonly int _switchableGroupCount;
        public int SwitchableGroupCount
        {
            get { return _switchableGroupCount; }
        }

        private readonly Guid _switchableGroupId;
        public Guid SwitchableGroupId
        {
            get { return _switchableGroupId; }
        }

        private readonly int _switchUseCount;
        public int SwitchUseCount
        {
            get { return _switchUseCount; }
        }

        public int CompareTo(object obj)
        {
            var c1 = (ISorterEval)obj;

            if (c1.Success != Success)
            {
                return c1.Success ? 1 : -1;
            }

            if (c1.SwitchUseCount > SwitchUseCount)
            {
                return -1;
            }

            if (c1.SwitchUseCount < SwitchUseCount)
            {
                return 1;
            }

            return 0;
        }
    }
}
