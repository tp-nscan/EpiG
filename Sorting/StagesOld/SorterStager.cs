using System.Collections.Generic;
using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.StagesOld
{
    public interface ISorterStager
    {
        ISorterStageOld Current { get; }
        ISorterStageOld Previous { get; } 
    }

    public static class SorterStager
    {
        static readonly ISorterStager emptySorterStager = new SorterStagerImpl(SorterStageOld.Empty, SorterStageOld.Empty);

        public static ISorterStager Empty
        {
            get { return emptySorterStager; }
        }

        public static ISorterStager AppendKeyPair(this ISorterStager sorterStager, IKeyPair keyPair)
        {
            return
                sorterStager.Current.KeyPairs.Any(kp => kp.Overlaps(keyPair))
                ?
                new SorterStagerImpl(
                    previous: sorterStager.Current,
                    current: SorterStageOld.Empty
                    )
                :
                new SorterStagerImpl(
                    previous: sorterStager.Current,
                    current: sorterStager.Current.AppendKeyPair(keyPair)
                    );
        }

        public static IReadOnlyList<ISorterStageOld> ToSorterStagesOld<T>(this IReadOnlyList<T> keyPairs, int keyCount)
                where T : IKeyPair
        {
            var retSorterStages = new List<ISorterStageOld>();
            var sorterStager = SorterStager.Empty;

            for (var i = 0; i < keyPairs.Count; i++)
            {
                sorterStager = sorterStager.AppendKeyPair(keyPairs[i]);
                if (sorterStager.Current == SorterStageOld.Empty)
                {
                    retSorterStages.Add(sorterStager.Previous);
                    sorterStager = SorterStager.Empty.AppendKeyPair(keyPairs[i]);
                }
            }
            if (sorterStager.Current != SorterStageOld.Empty)
            {
                retSorterStages.Add(sorterStager.Current);
            }
            return retSorterStages;
        }
    }

    class SorterStagerImpl : ISorterStager
    {
        private readonly ISorterStageOld _current;
        private readonly ISorterStageOld _previous;

        public SorterStagerImpl(ISorterStageOld previous, ISorterStageOld current)
        {
            _previous = previous;
            _current = current;
        }

        public ISorterStageOld Current
        {
            get { return _current; }
        }

        public ISorterStageOld Previous
        {
            get { return _previous; }
        }
    }
}