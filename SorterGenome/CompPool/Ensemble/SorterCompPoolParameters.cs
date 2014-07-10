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
                int replicas
        )
        {
            var randy = Rando.Fast(seed);

            SorterCompPoolParameters sorterCompPoolParameters;

            switch (sorterCompPoolParameterType)
            {
                case SorterCompPoolParameterType.Seed:
                    sorterCompPoolParameters = new SorterCompPoolParameters
                        (
                            name: startingValue.ToString("0.0000"),
                            seed: startingValue,
                            orgCount: orgCount,
                            legacyRate: legacyRate,
                            deletionRate: deletionRate,
                            insertionRate: insertionRate,
                            mutationRate: mutationRate,
                            cubRate: cubRate
                        );
                    break;
                case SorterCompPoolParameterType.OrgCount:
                    sorterCompPoolParameters = new SorterCompPoolParameters
                        (
                            name: startingValue.ToString("0.0000"),
                            seed: randy.NextInt(),
                            orgCount: startingValue,
                            legacyRate: legacyRate,
                            deletionRate: deletionRate,
                            insertionRate: insertionRate,
                            mutationRate: mutationRate,
                            cubRate: cubRate
                        );
                    break;
                case SorterCompPoolParameterType.LegacyRate:
                    sorterCompPoolParameters = new SorterCompPoolParameters
                        (
                            name: startingValue.ToString("0.0000"),
                            seed: randy.NextInt(),
                            orgCount: orgCount,
                            legacyRate: startingValue,
                            deletionRate: deletionRate,
                            insertionRate: insertionRate,
                            mutationRate: mutationRate,
                            cubRate: cubRate
                        );
                    break;
                case SorterCompPoolParameterType.CubRate:
                    sorterCompPoolParameters = new SorterCompPoolParameters
                        (
                            name: startingValue.ToString("0.0000"),
                            seed: randy.NextInt(),
                            orgCount: orgCount,
                            legacyRate: legacyRate,
                            deletionRate: deletionRate,
                            insertionRate: insertionRate,
                            mutationRate: mutationRate,
                            cubRate: startingValue
                        );
                    break;
                case SorterCompPoolParameterType.DeletionRate:
                    sorterCompPoolParameters = new SorterCompPoolParameters
                        (
                            name: startingValue.ToString("0.0000"),
                            seed: randy.NextInt(),
                            orgCount: orgCount,
                            legacyRate: legacyRate,
                            deletionRate: startingValue,
                            insertionRate: insertionRate,
                            mutationRate: mutationRate,
                            cubRate: cubRate
                        );
                    break;
                case SorterCompPoolParameterType.InsertionRate:
                    sorterCompPoolParameters = new SorterCompPoolParameters
                        (
                            name: startingValue.ToString("0.0000"),
                            seed: randy.NextInt(),
                            orgCount: orgCount,
                            legacyRate: legacyRate,
                            deletionRate: deletionRate,
                            insertionRate: startingValue,
                            mutationRate: mutationRate,
                            cubRate: cubRate
                        );
                    break;
                case SorterCompPoolParameterType.MutationRate:
                    sorterCompPoolParameters = new SorterCompPoolParameters
                        (
                            name: startingValue.ToString("0.0000"),
                            seed: randy.NextInt(),
                            orgCount: orgCount,
                            legacyRate: legacyRate,
                            deletionRate: deletionRate,
                            insertionRate: insertionRate,
                            mutationRate: startingValue,
                            cubRate: cubRate
                        );
                    break;
                default:
                    throw new Exception(String.Format("sorterCompPoolParameterType: {0} not handled", sorterCompPoolParameterType));
            }

            return sorterCompPoolParameters.GetRange
                (
                    sorterCompPoolParameterType: sorterCompPoolParameterType,
                    incrementValue: increment
                ).Take(replicas).ToList();
        }

        public static IEnumerable<SorterCompPoolParameters> GetRange
            (
                this SorterCompPoolParameters initial,
                SorterCompPoolParameterType sorterCompPoolParameterType,
                Func<double, double> mutator
            )
        {
            var current = initial;
            while (true)
            {
                yield return current;
                current = current.Modify(sorterCompPoolParameterType, mutator);
            }
        }

        public static IEnumerable<SorterCompPoolParameters> GetRange
            (
                this SorterCompPoolParameters initial,
                SorterCompPoolParameterType sorterCompPoolParameterType,
                double incrementValue
            )
        {
            Func<double, double> mutator = d => d + incrementValue;
            var current = initial;
            while (true)
            {
                yield return current;
                current = current.Modify(sorterCompPoolParameterType, mutator);
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
            Func<double, double> mutator 
            )
        {
            switch (sorterCompPoolParameterType)
            {
                case SorterCompPoolParameterType.Seed:
                    return new SorterCompPoolParameters
                        (
                            name: mutator(Seed).ToString("0.0000"),
                            seed: mutator(Seed),
                            orgCount:OrgCount, 
                            legacyRate: LegacyRate, 
                            deletionRate: DeletionRate, 
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate, 
                            cubRate: CubRate
                        );
                case SorterCompPoolParameterType.OrgCount:
                    return new SorterCompPoolParameters
                        (
                            name: mutator(OrgCount).ToString("0.0000"),
                            seed: Seed,
                            orgCount: mutator(OrgCount),
                            legacyRate: LegacyRate,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubRate: CubRate
                        );
                case SorterCompPoolParameterType.LegacyRate:
                    return new SorterCompPoolParameters
                        (
                            name: mutator(LegacyRate).ToString("0.0000"),
                            seed: Seed,
                            orgCount: OrgCount,
                            legacyRate: mutator(LegacyRate),
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubRate: CubRate
                        );
                case SorterCompPoolParameterType.CubRate:
                    return new SorterCompPoolParameters
                        (
                            name: mutator(CubRate).ToString("0.0000"),
                            seed: Seed,
                            orgCount: OrgCount,
                            legacyRate: LegacyRate,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubRate: mutator(CubRate)
                        );
                case SorterCompPoolParameterType.DeletionRate:
                    return new SorterCompPoolParameters
                        (
                            name: mutator(DeletionRate).ToString("0.0000"),
                            seed: Seed,
                            orgCount: OrgCount,
                            legacyRate: LegacyRate,
                            deletionRate: mutator(DeletionRate),
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            cubRate: CubRate
                        );
                case SorterCompPoolParameterType.InsertionRate:
                    return new SorterCompPoolParameters
                        (
                            name: mutator(InsertionRate).ToString("0.0000"),
                            seed: Seed,
                            orgCount: OrgCount,
                            legacyRate: LegacyRate,
                            deletionRate: DeletionRate,
                            insertionRate: mutator(InsertionRate),
                            mutationRate: MutationRate,
                            cubRate: CubRate
                        );
                case SorterCompPoolParameterType.MutationRate:
                    return new SorterCompPoolParameters
                        (
                            name: mutator(MutationRate).ToString("0.0000"),
                            seed: Seed,
                            orgCount: OrgCount,
                            legacyRate: LegacyRate,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: mutator(MutationRate),
                            cubRate: CubRate
                        );
                default:
                    throw new Exception(string.Format("unhandled SorterCompPoolParameterType: {0}", sorterCompPoolParameterType));;
            }
                        
        }


    }
}