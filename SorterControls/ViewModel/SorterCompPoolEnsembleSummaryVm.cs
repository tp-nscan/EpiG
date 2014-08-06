using System.Collections.Generic;
using System.Linq;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterCompPoolEnsembleSummaryVm : ViewModelBase
    {
        public SorterCompPoolEnsembleSummaryVm
            (
                string run,
                int generation,
                int colonySize,
                int legacyCount,
                int cubCount,
                double mutationRate,
                int colonyCount,
                IList<int> bestValues
            )
        {
            Run = run;
            Generation = generation;
            ColonySize = colonySize;
            LegacyCount = legacyCount;
            CubCount = cubCount;
            MutationRate = mutationRate;
            ColonyCount = colonyCount;
            Average = bestValues.Average(t => t);
            Best = bestValues.Min(t => t);

            foreach (var countGroup in bestValues.GroupBy(t => t))
            {
                switch (countGroup.Key)
                {
                    case 29:
                        c29 = countGroup.Count();
                        break;
                    case 30:
                        c30 = countGroup.Count();
                        break;
                    case 31:
                        c31 = countGroup.Count();
                        break;
                    case 32:
                        c32 = countGroup.Count();
                        break;
                    case 33:
                        c33 = countGroup.Count();
                        break;
                    case 34:
                        c34 = countGroup.Count();
                        break;
                    default:
                        c35P += countGroup.Count();
                         break;
                }
            }

            
            //TopQuarter = bestValues
            //                .OrderBy(t=>t)
            //                .Take(bestValues.Count/4)
            //                .Average(t => t);
        }

        public string Run { get; set; }

        public double Generation { get; set; }

        public int ColonyCount { get; set; }

        public int ColonySize { get; set; }

        public int LegacyCount { get; set; }

        public int CubCount { get; set; }

        public double MutationRate { get; set; }

        public double Average { get; set; }

        public double Best { get; set; }

        public int c29 { get; set; }
        public int c30 { get; set; }
        public int c31 { get; set; }
        public int c32 { get; set; }
        public int c33 { get; set; }
        public int c34 { get; set; }
        public int c35P { get; set; }


    }
}
