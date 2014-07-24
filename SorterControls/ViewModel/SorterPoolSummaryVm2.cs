using System.Collections.Generic;
using System.Linq;
using Sorting.Evals;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterPoolSummaryVm2 : ViewModelBase
    {
        public SorterPoolSummaryVm2
            (
                IEnumerable<ISorterEval> sorterEvals,
                int generation,
                string name,
                double tweak
            )
        {
            _generation = generation;
            _name = name;

            var goodEvals = sorterEvals.Where(ev=>ev.Success).ToList();

            Best = goodEvals.Min(ev => ev.SwitchUseCount) + tweak;
            Average = goodEvals.Average(ev => ev.SwitchUseCount);

        }



        private int _generation;
        public int Generation
        {
            get { return _generation; }
            set
            {
                _generation = value;
                OnPropertyChanged("Generation");
            }
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        public double Best { get; set; }

        public double Average { get; set; }

    }
}
