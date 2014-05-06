using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Stages;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public static class StagedSorterVm
    {
        public static ISorterVm ToSorterVm(this IStagedSorter sorter)
        {
            return null;
        }
    }

    public class StagedSorterVmImpl : ViewModelBase
    {
        public StagedSorterVmImpl
            (
                IStagedSorter stagedSorter,
                List<Brush> lineBrushes,
                int width
            )
        {
            _stagedSorter = stagedSorter;
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

    }

}
