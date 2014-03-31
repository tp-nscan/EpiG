using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Sorters;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterVm : ViewModelBase
    {
        public SorterVm
            (
                ISorter sorter,
                List<Brush> lineBrushes,
                int width,
                int height
            )
        {
            _sorter = sorter;
            foreach (var keyPair in Sorter.KeyPairs)
            {
                SwitchVms.Add
                    (
                        new SwitchVm(
                            keyPair, 
                            sorter.KeyCount, 
                            lineBrushes,
                            width
                        ) 
                        { SwitchBrush = Brushes.Red} 
                    );
            }
        }

        private readonly ISorter _sorter;
        ISorter Sorter
        {
            get { return _sorter; }
        }

        public int KeyCount
        {
            get { return Sorter.KeyCount; }
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
