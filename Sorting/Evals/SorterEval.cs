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
                int keyCount
            )
        {
            _guid = guid;
            _keyCount = keyCount;
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
    }
}
