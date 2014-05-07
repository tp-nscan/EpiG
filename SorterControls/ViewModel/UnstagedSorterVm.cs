using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Sorters;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public static class UnStagedSorterVm
    {
        public static ISorterVm ToSorterVm(this ISorter sorter)
        {
            return new UnstagedSorterVmImpl
                (
                    sorter: sorter,
                    lineBrushes: null,
                    width: 0
                );
        }
    }

    public class UnstagedSorterVmImpl: ViewModelBase, ISorterVm
    {
        public UnstagedSorterVmImpl
            (
                ISorter sorter,
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
                        keyPair,
                        Sorter.KeyCount,
                        lineBrushes,
                        width
                    ) { SwitchBrush = Brushes.Red }
                );
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

        public SorterVmType SorterVmType { get { return SorterVmType.Unstaged; } }
    }
}
