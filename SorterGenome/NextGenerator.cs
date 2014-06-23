using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.PhenotypeEvals;
using MathUtils.Rand;
using Sorting.Sorters;

namespace SorterGenome
{
    public static class NextGenerator
    {



    }

    public class NextGeneratorImpl<T>
        where T : ISorter
    {
        public NextGeneratorImpl(
            int keyCount, 
            int orgCount, 
            double mutationRate, 
            double multiplicationRate, 
            double cubRate
          )
        {
            _keyCount = keyCount;
            _orgCount = orgCount;
            _mutationRate = mutationRate;
            _multiplicationRate = multiplicationRate;
            _cubRate = cubRate;

            _nextGenerator = (eD, i) =>
            {

                var randy = Rando.Fast(i);


                var genomes = new List<IGenome>();
                return genomes.ToDictionary(g=>g.GenomeBuilder.Guid);
            };
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly int _orgCount;
        public int OrgCount
        {
            get { return _orgCount; }
        }

        private readonly double _mutationRate;
        public double MutationRate
        {
            get { return _mutationRate; }
        }

        private readonly double _multiplicationRate;
        public double MultiplicationRate
        {
            get { return _multiplicationRate; }
        }

        private readonly double _cubRate;
        public double CubRate
        {
            get { return _cubRate; }
        }

        private readonly 
            Func<
                    IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, 
                    int, 
                    IReadOnlyDictionary<Guid, IGenome>
                >
            _nextGenerator;
        public Func<
                     IReadOnlyDictionary<Guid, IPhenotypeEval<T>>,
                     int,
                     IReadOnlyDictionary<Guid, IGenome>
                    >
                    NextGenerator
        {
            get { return _nextGenerator; }
        }
    }

    //UpdateGenomesStepH
    //SorterPropigator
    //public static ILayer<TG> Multiply<TG>
}
