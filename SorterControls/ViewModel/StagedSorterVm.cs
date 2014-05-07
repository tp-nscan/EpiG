using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Evals;
using Sorting.Stages;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public static class StagedSorterVm
    {
        public static ISorterVm ToSorterVm
            (
                this ISorterEval sorterEval,
                List<Brush> lineBrushes,
                int width
            )
        {
            return new StagedSorterVmImpl
                (
                    sorterEval: sorterEval,
                    lineBrushes: lineBrushes,
                    width: width
                );
        }
    }

    public class StagedSorterVmImpl : ViewModelBase, ISorterVm
    {
        public StagedSorterVmImpl
            (
                ISorterEval sorterEval,
                List<Brush> lineBrushes,
                int width
            )
        {
            _sorterEval = sorterEval;

            _stagedSorter = sorterEval.ToStagedSorter();

            foreach (var sorterStage in StagedSorter.SorterStages)
            {
                SorterStageVms.Add
                    (
                        new SorterStageVm(
                            sorterStage,
                            lineBrushes,
                            width
                        )
                    );
            }
        }

        private readonly IStagedSorter _stagedSorter;
        IStagedSorter StagedSorter
        {
            get { return _stagedSorter; }
        }

        private ObservableCollection<SorterStageVm> _sorterStageVms = new ObservableCollection<SorterStageVm>();
        public ObservableCollection<SorterStageVm> SorterStageVms
        {
            get { return _sorterStageVms; }
            set { _sorterStageVms = value; }
        }

        public SorterVmType SorterVmType
        {
            get { return SorterVmType.Staged;}
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
