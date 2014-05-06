using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Stages;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterStageVm : ViewModelBase
    {
        public SorterStageVm
            (
                ISorterStage sorterStage,
                List<Brush> lineBrushes,
                int width
            )
        {
            _sorterStage = sorterStage;
            foreach (var keyPair in SorterStage.KeyPairs)
            {
                SwitchVms.Add
                    (
                        new SwitchVm(
                            keyPair,
                            SorterStage.KeyCount,
                            lineBrushes,
                            width
                        ) { SwitchBrush = Brushes.Red }
                    );
            }

        }

        private readonly ISorterStage _sorterStage;
        ISorterStage SorterStage
        {
            get { return _sorterStage; }
        }

        private ObservableCollection<SwitchVm> _switchVms = new ObservableCollection<SwitchVm>();
        public ObservableCollection<SwitchVm> SwitchVms
        {
            get { return _switchVms; }
            set { _switchVms = value; }
        }

    }
}
