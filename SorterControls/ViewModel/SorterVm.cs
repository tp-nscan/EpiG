using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Evals;
using Sorting.Sorters;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public interface ISorterVm
    {
        SorterVmType SorterVmType { get; }
    }

    public static class SorterVm
    {
        public static ISorterVm ToSorterVm
            (
                this ISorterEval sorterEval,
                List<Brush> lineBrushes,
                int width
            )
        {
            return new SorterVmImpl
                (
                    sorter: sorterEval,
                    lineBrushes: lineBrushes,
                    width: width
                );
        }
    }

    public class SorterVmImpl : ViewModelBase, ISorterVm
    {
        public SorterVmImpl
            (
                ISorterEval sorter,
                List<Brush> lineBrushes,
                int width
            )
        {
            _sorter = sorter;
            foreach (var keyPair in Sorter.KeyPairs)
            {
                SwitchVms.Add
                    (
                        new SwitchVm
                        (
                            keyPair: keyPair,
                            keyCount: sorter.KeyCount,
                            lineBrushes: lineBrushes,
                            width: width
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

        public SorterVmType SorterVmType
        {
            get { return SorterVmType.Unstaged; }
        }
    }
}
