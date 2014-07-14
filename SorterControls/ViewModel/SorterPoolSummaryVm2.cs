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

            //IReadOnlyDictionary<int, List<ISorterEval>>  evals 
            //    = sorterEvals.GroupBy(ev => ev.SwitchUseCount)
            //                 .ToDictionary(g => g.Key, g => g.ToList());

            //S38 = (evals.ContainsKey(38)) ? evals[38].Count : 0;
            //S39 = (evals.ContainsKey(39)) ? evals[39].Count : 0;
            //S40 = (evals.ContainsKey(40)) ? evals[40].Count : 0;
            //S41 = (evals.ContainsKey(41)) ? evals[41].Count : 0;
            //S42 = (evals.ContainsKey(42)) ? evals[42].Count : 0;
            //S43 = (evals.ContainsKey(43)) ? evals[43].Count : 0;
            //S44 = (evals.ContainsKey(44)) ? evals[44].Count : 0;
            //S45 = (evals.ContainsKey(45)) ? evals[45].Count : 0;
            //S46 = (evals.ContainsKey(46)) ? evals[46].Count : 0;
            //S47 = (evals.ContainsKey(47)) ? evals[47].Count : 0;
            //S48 = (evals.ContainsKey(48)) ? evals[48].Count : 0;
            //S49 = (evals.ContainsKey(49)) ? evals[49].Count : 0;
            //S50 = (evals.ContainsKey(50)) ? evals[50].Count : 0;
            //S51 = (evals.ContainsKey(51)) ? evals[51].Count : 0;
            //S52 = (evals.ContainsKey(52)) ? evals[52].Count : 0;
            //S53 = (evals.ContainsKey(53)) ? evals[53].Count : 0;
            //S54 = (evals.ContainsKey(54)) ? evals[54].Count : 0;
            //S55 = (evals.ContainsKey(55)) ? evals[55].Count : 0;
            //S56 = (evals.ContainsKey(56)) ? evals[56].Count : 0;
            //S57 = (evals.ContainsKey(57)) ? evals[57].Count : 0;
            //S58 = (evals.ContainsKey(58)) ? evals[58].Count : 0;
            //S59 = (evals.ContainsKey(59)) ? evals[59].Count : 0;

            //Best = evals.Where(kvp => kvp.Value.Count > 0).Min(p => p.Key);

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

        //public int S38 { get; set; }
        //public int S39 { get; set; }
        //public int S40 { get; set; }
        //public int S41 { get; set; }
        //public int S42 { get; set; }
        //public int S43 { get; set; }
        //public int S44 { get; set; }
        //public int S45 { get; set; }
        //public int S46 { get; set; }
        //public int S47 { get; set; }
        //public int S48 { get; set; }
        //public int S49 { get; set; }
        //public int S50 { get; set; }
        //public int S51 { get; set; }
        //public int S52 { get; set; }
        //public int S53 { get; set; }
        //public int S54 { get; set; }
        //public int S55 { get; set; }
        //public int S56 { get; set; }
        //public int S57 { get; set; }
        //public int S58 { get; set; }
        //public int S59 { get; set; }
    }
}
