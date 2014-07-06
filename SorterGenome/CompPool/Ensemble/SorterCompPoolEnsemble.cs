using System;
using System.Collections.Generic;
using System.Linq;
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
                int seed,
                int orgCount,
                int seqenceLength,
                int keyCount,
                double deletionRate,
                double insertionRate,
                double mutationRate,
                double legacyRate,
                double cubRate,
                int replicaCount
            )
        {
            _guid = guid;

            var randy = Rando.Fast(seed);

            //_sorterCompPools = Enumerable.Range(0, ReplicaCount).Select(
                
            //    i=> SorterCompPool.InitStandardFromSeed(
                    
            //                    return SorterCompPool.InitStandardFromSeed
            //    (
            //        seed: Seed,
            //        orgCount: SorterCount,
            //        seqenceLength: KeyPairCount,
            //        keyCount: KeyCount,
            //        deletionRate: DeletionRate,
            //        insertionRate: InsertionRate,
            //        mutationRate: MutationRate,
            //        legacyRate: LegacyRate,
            //        cubRate: CubRate
            //    ).ToPassThroughWorkflow(Guid.NewGuid())
            //    .ToRecursiveWorkflowRndWlk();
            //        )
                
                
            //    )
                
                
                
                
                
                
                //new List<ISorterCompPool<T>>();

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
            throw new NotImplementedException();
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
