//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using MathUtils.Rand;
//using Sorting.Sorters;
//using Utils;
//using Workflows;

//namespace SorterGenome.CompPool
//{
//    public interface ISorterCompPoolEnsemble<T> : IRandomWalk<ISorterCompPoolEnsemble<T>>, IEntity
//        where T : ISorter
//    {
//        int ReplicaCount { get; }
//        IEnumerable<ISorterCompPool<T>> SorterCompPools { get; }
//    }

//    public static class SorterCompPoolEnsemble
//    {


//    }

//    public class SorterCompPoolEnsembleStandard<T> : ISorterCompPoolEnsemble<T>
//        where T : ISorter
//    {

//        public SorterCompPoolEnsembleStandard
//            (
//                Guid guid,
//                int seed,
//                int orgCount,
//                int seqenceLength,
//                int keyCount,
//                double deletionRate,
//                double insertionRate,
//                double mutationRate,
//                double legacyRate,
//                double cubRate,
//                int replicaCount
//            )
//        {
//            _replicaCount = replicaCount;
//            _guid = guid;

//            var randy = Rando.Fast(seed);

//            _sorterCompPools = Enumerable.Range(0, ReplicaCount).Select(
                
//                i=> SorterCompPool.InitStandardFromSeed(
                    
//                                return SorterCompPool.InitStandardFromSeed
//                (
//                    seed: Seed,
//                    orgCount: SorterCount,
//                    seqenceLength: KeyPairCount,
//                    keyCount: KeyCount,
//                    deletionRate: DeletionRate,
//                    insertionRate: InsertionRate,
//                    mutationRate: MutationRate,
//                    legacyRate: LegacyRate,
//                    cubRate: CubRate
//                ).ToPassThroughWorkflow(Guid.NewGuid())
//                .ToRecursiveWorkflowRndWlk();
//                    )
                
                
//                )
                
                
                
                
                
                
//                //new List<ISorterCompPool<T>>();

//        }

//        public ISorterCompPoolEnsemble<T> Step(int seed)
//        {
//            throw new NotImplementedException();
//        }

//        private readonly Guid _guid;
//        public Guid Guid
//        {
//            get { return _guid; }
//        }

//        public object GetPart(Guid key)
//        {
//            return _sorterCompPools.SingleOrDefault(p => p.Guid == key);
//        }

//        public string EntityName
//        {
//            get { return "SorterCompPoolEnsembleStandard"; }
//        }

//        private readonly int _replicaCount;
//        public int ReplicaCount
//        {
//            get { return _replicaCount; }
//        }

//        private readonly List<ISorterCompPool<T>> _sorterCompPools;
//        public IEnumerable<ISorterCompPool<T>> SorterCompPools
//        {
//            get { return _sorterCompPools; }
//        }
//    }
//}
