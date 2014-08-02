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
        SorterCompPoolStageType SorterCompPoolStageType { get; }
    }

    public static class SorterCompPoolEnsemble
    {

        public static ISorterCompPoolEnsemble InitStandardFromSeed
            (
                int seed,
                int orgCount,
                int seqenceLength,
                int keyCount,
                double deletionRate,
                double insertionRate,
                double mutationRate,
                int legacyCount,
                int cubCount,
                int stepCount,
                double startingValue,
                double increment,
                SorterCompPoolParameterType sorterCompPoolParameterType,
                int reps
            )
        {
            var randy = Rando.Fast(seed);

            var sorterCompPoolParameters = SorterCompPoolParametersExt.GetParameterSet
                (
                    seed: seed,
                    orgCount: orgCount,
                    deletionRate: deletionRate,
                    insertionRate: insertionRate,
                    mutationRate: mutationRate,
                    legacyCount: legacyCount,
                    cubCount: cubCount,
                    startingValue: startingValue,
                    increment: increment,
                    sorterCompPoolParameterType: sorterCompPoolParameterType,
                    reps: reps,
                    stepCount: stepCount
                );

            return
                new SorterCompPoolEnsembleStandard
                    (
                        guid: randy.NextGuid(),
                        sorterCompPools: sorterCompPoolParameters.Select
                        (
                            p => SorterCompPool.InitStandardFromSeed
                                (
                                    seed: p.Seed,
                                    orgCount: p.OrgCount,
                                    seqenceLength: seqenceLength,
                                    keyCount: keyCount,
                                    deletionRate: p.DeletionRate, 
                                    insertionRate: p.InsertionRate,
                                    mutationRate: p.MutationRate,
                                    legacyCount: p.LegacyCount,
                                    cubCount: p.CubCount,
                                    name: p.Name
                                )
                        )
                    );
        }


        public static ISorterCompPoolEnsemble InitPermuterFromSeed
        (
            int seed,
            int orgCount,
            int permutationCount,
            int degree,
            double deletionRate,
            double insertionRate,
            double mutationRate,
            int legacyCount,
            int cubCount,
            int stepCount,
            double startingValue,
            double increment,
            SorterCompPoolParameterType sorterCompPoolParameterType,
            int reps
        )
        {
            var randy = Rando.Fast(seed);

            var sorterCompPoolParameters = SorterCompPoolParametersExt.GetParameterSet
                (
                    seed: seed,
                    orgCount: orgCount,
                    deletionRate: deletionRate,
                    insertionRate: insertionRate,
                    mutationRate: mutationRate,
                    legacyCount: legacyCount,
                    cubCount: cubCount,
                    startingValue: startingValue,
                    increment: increment,
                    sorterCompPoolParameterType: sorterCompPoolParameterType,
                    reps: reps,
                    stepCount: stepCount
                );

            return
                new SorterCompPoolEnsembleStandard
                    (
                        guid: randy.NextGuid(),
                        sorterCompPools: sorterCompPoolParameters.Select
                        (
                            p => SorterCompPool.InitPermuterFromSeed
                                (
                                    seed: p.Seed,
                                    orgCount: p.OrgCount,
                                    permutationCount: permutationCount,
                                    degree: degree,
                                    deletionRate: p.DeletionRate,
                                    insertionRate: p.InsertionRate,
                                    mutationRate: p.MutationRate,
                                    legacyCount: p.LegacyCount,
                                    cubCount: p.CubCount,
                                    name: p.Name
                                )
                        )
                    );
        }
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

        public SorterCompPoolStageType SorterCompPoolStageType
        {
            get { return _sorterCompPools[0].SorterCompPoolStageType; }
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
