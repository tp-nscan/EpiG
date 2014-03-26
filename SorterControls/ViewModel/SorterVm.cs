using System;
using System.Collections.ObjectModel;
using Sorting.Sorters;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterVm : ViewModelBase
    {
        public SorterVm(ISorter sorter)
        {
            _sorter = sorter;
            foreach (var @switch in Sorter.KeyPairs)
            {
                SwitchVms.Add(new SwitchVm(@switch));
            }
        }

        private readonly ISorter _sorter;
        ISorter Sorter
        {
            get { return _sorter; }
        }

        private ObservableCollection<SwitchVm> _switchVms = new ObservableCollection<SwitchVm>();
        public ObservableCollection<SwitchVm> SwitchVms
        {
            get { return _switchVms; }
            set { _switchVms = value; }
        }

        public string StringValue
        {
            get { return String.Empty; }
        }
    }
}
