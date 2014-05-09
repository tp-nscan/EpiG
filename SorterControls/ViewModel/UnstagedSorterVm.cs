using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Evals;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public static class UnStagedSorterVm
    {
        public static ISorterVm ToSorterVm
            (
                this ISorterEval sorterEval,
                List<Brush> lineBrushes,
                List<Brush> switchBrushes,
                int width,
                int height,
                bool showUnusedSwitches
            )
        {
            return new UnstagedSorterVmImpl
                (
                    sorterEval: sorterEval,
                    lineBrushes: lineBrushes,
                    switchBrushes: switchBrushes,
                    width: width,
                    height: height,
                    showUnusedSwitches: showUnusedSwitches
                );
        }
    }

    public class UnstagedSorterVmImpl: ViewModelBase, ISorterVm
    {
        public UnstagedSorterVmImpl
            (
                ISorterEval sorterEval,
                List<Brush> lineBrushes,
                List<Brush> switchBrushes,
                int width,
                int height,
                bool showUnusedSwitches
            )
        {
            _sorterEval = sorterEval;
            foreach (var keyPair in SorterEval.KeyPairs)
            {
                SwitchVms.Add
                (
                    new SwitchVm
                    (
                        keyPair,
                        SorterEval.KeyCount,
                        lineBrushes,
                        width
                    ) { SwitchBrush = Brushes.Red }
                );
            }
        }

        private readonly ISorterEval _sorterEval;
        ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }

        private ObservableCollection<SwitchVm> _switchVms = new ObservableCollection<SwitchVm>();
        public ObservableCollection<SwitchVm> SwitchVms
        {
            get { return _switchVms; }
            set { _switchVms = value; }
        }

        public SorterVmType SorterVmType { get { return SorterVmType.Unstaged; } }

        public int SwitchesUsed
        {
            get { return SorterEval.SwitchUseCount; }
        }

        public bool Success
        {
            get { return SorterEval.Success; }
        }
    }
}
