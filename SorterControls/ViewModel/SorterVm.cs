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
        int SwitchesUsed { get; }
    }

    public static class SorterVm
    {
        public static ISorterVm ToSorterVm(this ISorter sorter)
        {
            return null;
        }
    }

    public class SorterVmImpl : ViewModelBase, ISorterVm
    {
        public SorterVmImpl
            (
                ISorterEval sorterEval,
                List<Brush> lineBrushes,
                int width
            )
        {
            _sorterEval = sorterEval;
            foreach (var keyPair in SorterEval.KeyPairs)
            {
                SwitchVms.Add
                    (
                        new SwitchVm
                        (
                            keyPair: keyPair,
                            keyCount: sorterEval.KeyCount,
                            lineBrushes: lineBrushes,
                            width: width
                        ) 
                        { SwitchBrush = Brushes.Red} 
                    );
            }
        }

        private readonly ISorterEval _sorterEval;
        ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }

        public int KeyCount
        {
            get { return SorterEval.KeyCount; }
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
