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
                double legacyRate,
                double cubRate,
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
                            legacyRate: legacyRate,
                            deletionRate: deletionRate,
                            insertionRate: insertionRate,
                            mutationRate: mutationRate,
                            cubRate: cubRate
                        );

            return sorterCompPoolParameters.GetRange
                (
                    sorterCompPoolParameterType: sorterCompPoolParameterType,
                    seed:seed,
                    reps:reps,
                    startingValue:startingValue,
                    stepCount: stepCount,
                    increment: increment
                ).Take(reps).ToList();
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
                double legacyRate, 
                double deletionRate, 
                double insertionRate,
                double mutationRate, 
                double cubRate
            )
        {
            _name = name;
            _seed = (int) seed;
            _orgCount = (int) orgCount;
            _mutationRate = mutationRate;
            _cubRate = cubRate;
            _legacyRate = legacyRate;
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

        private readonly double _legacyRate;
        public double LegacyRate
        {
            get { return _legacyRate; }
        }

        private readonly double _cubRate;
        public double CubRate
        {
            get { return _cubRate; }
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
                            legacyRate: LegacyRate,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubRate: CubRate
                        );

                case SorterCompPoolParameterType.LegacyRate:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyRate: newParam,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubRate: CubRate
                        );

                case SorterCompPoolParameterType.CubRate:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyRate: LegacyRate,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubRate: newParam
                        );

                case SorterCompPoolParameterType.DeletionRate:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyRate: LegacyRate,
                            deletionRate: newParam,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubRate: CubRate
                        );

                case SorterCompPoolParameterType.InsertionRate:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyRate: LegacyRate,
                            deletionRate: DeletionRate,
                            insertionRate: newParam,
                            mutationRate: MutationRate,
                            cubRate: CubRate
                        );

                case SorterCompPoolParameterType.MutationRate:
                    return new SorterCompPoolParameters
                        (
                            name: name,
                            seed: seed,
                            orgCount: OrgCount,
                            legacyRate: LegacyRate,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: newParam,
                            cubRate: CubRate
                        );

                default:
                    throw new Exception(string.Format("unhandled SorterCompPoolParameterType: {0}", sorterCompPoolParameterType));;
            }
                        
        }


    }
}