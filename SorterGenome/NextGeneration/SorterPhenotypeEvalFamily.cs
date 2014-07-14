using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using SorterGenome.PhenotypeEvals;

namespace SorterGenome.NextGeneration
{
    public class SorterPhenotypeEvalFamily : IComparable
    {
        public SorterPhenotypeEvalFamily(ISorterPhenotypeEval sorterPhenotypeEval)
        {
            _sorterPhenotypeEvals.Add(sorterPhenotypeEval);
            _sourceGenome = sorterPhenotypeEval.SorterPhenotypeEvalBuilder.SorterPhenotype.SorterPhenotypeBuilder.Genome;
        }

        private List<ISorterPhenotypeEval> _sorterPhenotypeEvals = new List<ISorterPhenotypeEval>() ;
        public List<ISorterPhenotypeEval> SorterPhenotypeEvals
        {
            get { return _sorterPhenotypeEvals; }
            set { _sorterPhenotypeEvals = value; }
        }

        public void AddSorterPhenotypeEval(ISorterPhenotypeEval sorterPhenotypeEval)
        {
            _sorterPhenotypeEvals.Add(sorterPhenotypeEval);
            _averageScore = null;
            _bestScore = null;
            _secondBestScore = null;
            _thirdBestScore = null;
        }

        private readonly IGenome _sourceGenome;
        public IGenome SourceGenome
        {
            get { return _sourceGenome; }
        }


        private double? _averageScore;
        public double AverageScore
        {
            get
            {
                if (!_averageScore.HasValue)
                {
                    _averageScore = _sorterPhenotypeEvals.Average(ev => ev.SorterEval.SwitchUseCount);
                }

                return _averageScore.Value;
            }
        }

        private double? _bestScore;
        public double BestScore
        {
            get
            {
                if (! _bestScore.HasValue)
                {
                    GetScores();
                }

                return _bestScore.Value;
            }
        }

        void GetScores()
        {
            _averageScore = _sorterPhenotypeEvals.Average(ev => ev.SorterEval.SwitchUseCount);
            var scores = _sorterPhenotypeEvals.Select(ev => ev.SorterEval.SwitchUseCount).OrderBy(uc => uc).ToList();

            _bestScore = scores[0];
            _secondBestScore = (scores.Count > 1) ? scores[1] : Double.MaxValue;
            _thirdBestScore = (scores.Count > 2) ? scores[2] : Double.MaxValue;

        }

        private double? _secondBestScore;
        public double SecondBestScore
        {
            get
            {
                if (!_bestScore.HasValue)
                {
                    GetScores();
                }

                return _secondBestScore.Value;
            }
        }

        private double? _thirdBestScore;
        public double ThirdBestScore
        {
            get
            {
                if (!_bestScore.HasValue)
                {
                    GetScores();
                }

                return _thirdBestScore.Value;
            }
        }

        public int CompareTo(object obj)
        {
            var c1 = (SorterPhenotypeEvalFamily)obj;


            if (c1.BestScore > BestScore)
            {
                return -1;
            }

            if (c1.BestScore < BestScore)
            {
                return 1;
            }

            if (c1.SecondBestScore > SecondBestScore)
            {
                return -1;
            }

            if (c1.SecondBestScore < SecondBestScore)
            {
                return 1;
            }

            if (c1.ThirdBestScore > ThirdBestScore)
            {
                return -1;
            }

            if (c1.ThirdBestScore < ThirdBestScore)
            {
                return 1;
            }

            if (c1.AverageScore > AverageScore)
            {
                return -1;
            }

            if (c1.AverageScore < AverageScore)
            {
                return 1;
            }

            return 0;
        }

    }
}
