using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Sorting.Evals;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace Sorting.Stages
{
    public interface IStagedSorter<T> where T: IKeyPair 
    {
        ISorterStage<T> SorterStage(int index);
        IImmutableList<ISorterStage<T>> SorterStages { get; }
        Guid Guid { get; }
        int KeyCount { get; }
        int KeyPairCount { get; }
        T KeyPair(int index);
        IReadOnlyList<T> KeyPairs { get; }
    }

    public static class StagedSorter
    {
        public static IStagedSorter<T> Make<T>(
                Guid guid,
                int keyCount,
                IImmutableList<ISorterStage<T>> sorterStages
            )
            where T: IKeyPair 
        {
            return new StagedSorterImpl<T>(guid, keyCount, sorterStages);
        }

        public static IStagedSorter<ISwitchEval> ToStagedSorter(this ISorterEval sorter, bool showUnused)
        {
            if (showUnused)
            {
                return new StagedSorterImpl<ISwitchEval>
                    (
                        guid: sorter.Guid,
                        keyCount: sorter.KeyCount,
                        sorterStages: ImmutableList<ISorterStage<ISwitchEval>>.Empty.AddRange
                        (
                            sorter.SwitchEvals
                                  .ToList()
                                  .ToSorterStages(sorter.KeyCount))
                    );
            }

            return new StagedSorterImpl<ISwitchEval>
                (
                    guid: sorter.Guid,
                    keyCount: sorter.KeyCount,
                    sorterStages: ImmutableList<ISorterStage<ISwitchEval>>.Empty.AddRange
                    (
                        sorter.SwitchEvals
                              .Where(ev => ev.UseCount > 0)
                              .ToList()
                              .ToSorterStages(sorter.KeyCount))
                );
        }

        public static IStagedSorter<IKeyPair> ToStagedSorter(this ISorter sorter)
        {
            return new StagedSorterImpl<IKeyPair>
                (
                    guid: sorter.Guid,
                    keyCount: sorter.KeyCount,
                    sorterStages: ImmutableList<ISorterStage<IKeyPair>>.Empty.AddRange(
                        sorter.KeyPairs.ToList().ToSorterStages(sorter.KeyCount))
                );
        }

        public static string DebugFormat<T>(this IStagedSorter<T> stagedSorter) 
            where T: IKeyPair 
        {
            var sb = new StringBuilder();

            for (var i = 0; i < stagedSorter.KeyCount; i++)
            {
                foreach (var sorterStage in stagedSorter.SorterStages)
                {
                    foreach (var keyPair in sorterStage.KeyPairs)
                    {
                        sb.Append
                            (
                                ((keyPair.LowKey == i) || (keyPair.HiKey == i)) 
                                ?
                                "*\t" : "\t"
                                    
                            );
                    }
                    sb.Append("|\t");
                }
                sb.Append("\n");
            }

            return sb.ToString();

        }

    }

    class StagedSorterImpl<T> : IStagedSorter<T> where T : IKeyPair 
    {
        private readonly Guid _guid;
        private readonly int _keyCount;

        public StagedSorterImpl
            (
                Guid guid, 
                int keyCount,
                IImmutableList<ISorterStage<T>> sorterStages
            )
        {
            _guid = guid;
            _keyCount = keyCount;
            _sorterStages = sorterStages;

            var tempList = new List<T>();
            for (var i = 0; i < _sorterStages.Count; i++)
            {
                var curStage = _sorterStages[i];
                for (var j = 0; j < curStage.KeyPairs.Count; j++)
                {
                    tempList.Add(curStage.KeyPairs[j]);
                }
            }

            _keyPairs = tempList;
            _keyPairCount = SorterStages.Sum(s => s.KeyPairCount);
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly int _keyPairCount;
        public int KeyPairCount
        {
            get { return _keyPairCount; }
        }

        public T KeyPair(int index)
        {
            return _keyPairs[index];
        }

        private IReadOnlyList<T> _keyPairs;
        public IReadOnlyList<T> KeyPairs
        {
            get { return _keyPairs ?? (_keyPairs = _sorterStages.SelectMany(ss=>ss.KeyPairs).ToList()); }
        }

        public ISorterStage<T> SorterStage(int index)
        {
            return _sorterStages[index];
        }

        private readonly IImmutableList<ISorterStage<T>> _sorterStages;

        public IImmutableList<ISorterStage<T>> SorterStages
        {
            get { return _sorterStages; }
        }
    }
}