using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace SorterGenome.CompPool.Ensemble
{

    public static class SorterCompPoolParametersExt
    {
        public static IReadOnlyList<SorterCompPoolParameters> GetParameterSet
        (
                int seed,
                int orgCount,
                double deletionRate,
                double insertionRate,
                double mutationRate,
                int legacyCount,
                int cubCount,
                double startingValue,
                double increment,
                SorterCompPoolParameterType sorterCompPoolParameterType,
                int stepCount,
                int reps
        )
        {
            var sorterCompPoolParameters = new SorterCompPoolParameters
                        (
                            name: string.Empty,
                            seed: 0,
                            orgCount: orgCount,
                            legacyCount: legacyCount,
                            deletionRate: deletionRate,
                            insertionRate: insertionRate,
                            mutationRate: mutationRate,
                            cubCount: cubCount
                        );

            return sorterCompPoolParameters.GetRange
                (
                    sorterCompPoolParameterType: sorterCompPoolParameterType,
                    seed:seed,
                    reps:reps,
                    startingValue:startingValue,
                    stepCount: stepCount,
                    increment: increment
                ).ToList();
        }

        public static IEnumerable<SorterCompPoolParameters> GetRange
            (
                this SorterCompPoolParameters initial,
                int seed,
                SorterCompPoolParameterType sorterCompPoolParameterType,
                int reps,
                double startingValue,
                int stepCount,
                double increment
            )
        {
            var seeds = Rando.Fast(seed).ToIntEnumerator().Take(reps).ToList();
            var steps = Enumerable.Range(0, stepCount).Select(s => startingValue + s*increment).ToList();

            for (var rep = 0; rep < reps; rep++)
            {
                for (var step = 0; step < stepCount; step++)
                {
                    yield return initial.Modify
                        (
                            sorterCompPoolParameterType: sorterCompPoolParameterType,
                            newParam: steps[step],
                            seed: seeds[rep],
                            name: steps[step].ToString("0.000")
                        );
                }

            }
        }

    }

    public class SorterCompPoolParameters
    {
        public SorterCompPoolParameters
            (
                string name,
                double seed,
                double orgCount,
                int legacyCount, 
                double deletionRate, 
                double insertionRate,
                double mutationRate,
                int cubCount
            )
        {
            _name = name;
            _seed = (int) seed;
            _orgCount = (int) orgCount;
            _mutationRate = mutationRate;
            _cubCount = cubCount;
            _legacyCount = legacyCount;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        private readonly int _orgCount;
        public int OrgCount
        {
            get { return _orgCount; }
        }

        private readonly double _deletionRate;
        public double DeletionRate
        {
            get { return _deletionRate; }
        }

        private readonly double _insertionRate;
        public double InsertionRate
        {
            get { return _insertionRate; }
        }

        private readonly double _mutationRate;
        public double MutationRate
        {
            get { return _mutationRate; }
        }

        private readonly int _legacyCount;
        public int LegacyCount
        {
            get { return _legacyCount; }
        }

        private readonly int _cubCount;
        public int CubCount
        {
            get { return _cubCount; }
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        public SorterCompPoolParameters Modify
            (
                SorterCompPoolParameterType sorterCompPoolParameterType, 
                double newParam,
                int seed,
                string name
            )
        {
            switch (sorterCompPoolParameterType)
            {
                case SorterCompPoolParameterType.PoolSize:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: newParam,
                            legacyCount: LegacyCount,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubCount: CubCount
                        );

                case SorterCompPoolParameterType.LegacyCount:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyCount: (int) newParam,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubCount: CubCount
                        );

                case SorterCompPoolParameterType.CubCount:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyCount: LegacyCount,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubCount: (int)newParam
                        );

                case SorterCompPoolParameterType.DeletionRate:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyCount: LegacyCount,
                            deletionRate: newParam,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubCount: CubCount
                        );

                case SorterCompPoolParameterType.InsertionRate:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyCount: LegacyCount,
                            deletionRate: DeletionRate,
                            insertionRate: newParam,
                            mutationRate: MutationRate,
                            cubCount: CubCount
                        );

                case SorterCompPoolParameterType.MutationRate:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyCount: LegacyCount,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: newParam,
                            cubCount: CubCount
                        );

                default:
                    throw new Exception(string.Format("unhandled SorterCompPoolParameterType: {0}", sorterCompPoolParameterType));;
            }
                        
        }


    }
}