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
            _keyCount = keyCount;
            _generation = generation;
            _sorterCompPoolStageType = sorterCompPoolStageType;
            _name = name;
            _sorterEvals = sorterEvals.GroupBy(ev => ev.SwitchUseCount).ToDictionary(g => g.Key, g => g.ToList());
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

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        private readonly IReadOnlyDictionary<int, List<ISorterEval>> _sorterEvals;
        public IReadOnlyDictionary<int, List<ISorterEval>> SorterEvals
        {
            get { return _sorterEvals; }
        }


        public int S38
        {
            get
            {
                return (_sorterEvals.ContainsKey(38)) ? _sorterEvals[38].Count : 0;
            }
        }

        public int S39
        {
            get
            {
                return (_sorterEvals.ContainsKey(39)) ? _sorterEvals[39].Count : 0;
            }
        }

        public int S40
        {
            get
            {
                return (_sorterEvals.ContainsKey(40)) ? _sorterEvals[40].Count : 0;
            }
        }

        public int S41
        {
            get
            {
                return (_sorterEvals.ContainsKey(41)) ? _sorterEvals[41].Count : 0;
            }
        }

        public int S42
        {
            get
            {
                return (_sorterEvals.ContainsKey(42)) ? _sorterEvals[42].Count : 0;
            }
        }

        public int S43
        {
            get
            {
                return (_sorterEvals.ContainsKey(43)) ? _sorterEvals[43].Count : 0;
            }
        }

        public int S44
        {
            get
            {
                return (_sorterEvals.ContainsKey(44)) ? _sorterEvals[44].Count : 0;
            }
        }

        public int S45
        {
            get
            {
                return (_sorterEvals.ContainsKey(45)) ? _sorterEvals[45].Count : 0;
            }
        }
        public int S46
        {
            get
            {
                return (_sorterEvals.ContainsKey(46)) ? _sorterEvals[46].Count : 0;
            }
        }

        public int S47
        {
            get
            {
                return (_sorterEvals.ContainsKey(47)) ? _sorterEvals[47].Count : 0;
            }
        }

        public int S48
        {
            get
            {
                return (_sorterEvals.ContainsKey(48)) ? _sorterEvals[48].Count : 0;
            }
        }

        public int S49
        {
            get
            {
                return (_sorterEvals.ContainsKey(49)) ? _sorterEvals[49].Count : 0;
            }
        }
        public int S50
        {
            get
            {
                return (_sorterEvals.ContainsKey(50)) ? _sorterEvals[50].Count : 0;
            }
        }

        public int S51
        {
            get
            {
                return (_sorterEvals.ContainsKey(51)) ? _sorterEvals[51].Count : 0;
            }
        }

        public int S52
        {
            get
            {
                return (_sorterEvals.ContainsKey(52)) ? _sorterEvals[52].Count : 0;
            }
        }
    }
}
