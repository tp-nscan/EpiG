using System;
using System.Collections.Generic;

namespace SorterGenome.CompPool.Ensemble
{
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
                        name: mutator(Seed).ToString("0.000"),
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
                        name: mutator(Seed).ToString("0.000"),
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
                        name: mutator(Seed).ToString("0.000"),
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
                        name: mutator(Seed).ToString("0.000"),
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
                        name: mutator(Seed).ToString("0.000"),
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
                        name: mutator(Seed).ToString("0.000"),
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
                        name: mutator(Seed).ToString("0.000"),
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

        public IEnumerable<SorterCompPoolParameters> GetRange(
            SorterCompPoolParameters initial,
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

        public IEnumerable<SorterCompPoolParameters> GetRange(
            SorterCompPoolParameters initial,
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
}