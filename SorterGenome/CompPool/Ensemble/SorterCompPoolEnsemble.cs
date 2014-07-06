using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathUtils;
using MathUtils.Rand;
using Utils;

namespace SorterGenome.CompPool.Ensemble
{
    public interface ISorterCompPoolEnsemble : IRandomWalk<ISorterCompPoolEnsemble>, IEntity
    {
        int ReplicaCount { get; }
        IEnumerable<ISorterCompPool> SorterCompPools { get; }
    }

    public static class SorterCompPoolEnsemble
    {


    }

    public class SorterCompPoolEnsembleStandard : ISorterCompPoolEnsemble
    {
        public SorterCompPoolEnsembleStandard
            (
                Guid guid,
                IEnumerable<ISorterCompPool> sorterCompPools 
            )
        {
            _guid = guid;
            _sorterCompPools = sorterCompPools.ToList();

        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public string EntityName
        {
            get { return "SorterCompPoolEnsembleStandard"; }
        }

        private readonly List<ISorterCompPool> _sorterCompPools;
        public IEnumerable<ISorterCompPool> SorterCompPools
        {
            get { return _sorterCompPools; }
        }

        public ISorterCompPoolEnsemble Step(int seed)
        {
            var randy = Rando.Fast(seed);
            var newSorterCompPools = new List<ISorterCompPool>();
            foreach (var sorterCompPool in SorterCompPools)
            {
                newSorterCompPools.Add(sorterCompPool.Step(randy.NextInt()));
            }
            return new SorterCompPoolEnsembleStandard(
                    guid: randy.NextGuid(),
                    sorterCompPools: newSorterCompPools
                );
        }

        public IEntity GetPart(Guid key)
        {
            if (Guid == key)
            {
                return this;
            }

            var child = _sorterCompPools.FirstOrDefault(p => (p.GetPart(key) != null));

            return (child == null) ? null : child.GetPart(key);
        }

        public int ReplicaCount
        {
            get { return _sorterCompPools.Count; }
        }
    }
}
