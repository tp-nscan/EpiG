using System.Collections.Generic;
using System.Linq;
using SorterGenome.CompPool;
using Sorting.Evals;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterPoolSummaryVm : ViewModelBase
    {
        public SorterPoolSummaryVm
            (
                int keyCount,
                IEnumerable<ISorterEval> sorterEvals,
                int generation,
                SorterCompPoolStageType sorterCompPoolStageType, 
                string name
            )
        {
            _generation = generation;
            _sorterCompPoolStageType = sorterCompPoolStageType;
            _name = name;
            _sorterEvals = sorterEvals.ToList();
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

        private SorterCompPoolStageType _sorterCompPoolStageType;
        public SorterCompPoolStageType SorterCompPoolStageType
        {
            get { return _sorterCompPoolStageType; }
            set
            {
                _sorterCompPoolStageType = value;
                OnPropertyChanged("SorterCompPoolStageType");
            }
        }

        private readonly string _name;

        public string Name
        {
            get { return _name; }
        }

        private List<ISorterEval> _sorterEvals;
        public IReadOnlyList<ISorterEval> SorterEvals
        {
            get { return _sorterEvals; }
        }
    }
}
