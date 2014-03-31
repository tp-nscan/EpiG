using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.CompetePools;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace Sorting.Evals
{
    public interface ISorterEval : ISorter
    {
        IReadOnlyList<ISwitchEval> SwitchEvals { get; }
        bool Success { get; }
        int SwitchableGroupCount { get; }
        Guid SwitchableGroupId { get; }
        int SwitchUseCount { get; }
    }

    public static class SorterEval
    {
        public static ISorterEval ToSorterEval(ISortResult sortResult)
        {
            return new SorterEvalImpl(
                switchEvals: Enumerable.Range(0, sortResult.Sorter.KeyPairCount) 
                                        .Select
                                        (
                                            i=> new SwitchEvalImpl
                                                (
                                                    sortResult.Sorter.KeyPair(i), 
                                                    sortResult.SwitchUseList[i]
                                                )
                                        ),
                guid: sortResult.Sorter.Guid,
                keyCount: sortResult.Sorter.KeyCount
                );

        }
    }

    public class SorterEvalImpl : ISorterEval
    {
        public SorterEvalImpl
            (
                IEnumerable<ISwitchEval> switchEvals, 
                Guid guid, 
                int keyCount, 
                Guid switchableGroupId, 
                int switchUseCount,
                int switchableGroupCount
            )
        {
            _guid = guid;
            _keyCount = keyCount;
            _switchableGroupId = switchableGroupId;
            _switchUseCount = switchUseCount;
            _switchableGroupCount = switchableGroupCount;
            _switchableGroupCount = switchableGroupCount;
            _switchEvals = switchEvals.ToList();
        }

        private readonly Guid _guid;
        private readonly int _keyCount;
        public Guid Guid
        {
            get { return _guid; }
        }

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

        private bool _success;
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
    }
}
