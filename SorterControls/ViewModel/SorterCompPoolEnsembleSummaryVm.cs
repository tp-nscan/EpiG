using System.Collections.Generic;
using System.Linq;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterCompPoolEnsembleSummaryVm : ViewModelBase
    {
        public SorterCompPoolEnsembleSummaryVm
            (
                string name,
                int generation,
                IList<double> bestValues
            )
        {
            Generation = generation;
            Name = name;
            Average = bestValues.Average(t => t);
            Best = bestValues.Min(t => t);
            TopQuarter = bestValues
                            .OrderBy(t=>t)
                            .Take(bestValues.Count/4)
                            .Average(t => t);
        }

        public double Generation { get; set; }

        public double TopQuarter { get; set; }

        public double Average { get; set; }

        public double Best { get; set; }


        public string Name { get; set; }

    }
}
