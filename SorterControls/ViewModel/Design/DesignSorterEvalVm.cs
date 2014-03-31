using System.Collections.ObjectModel;
using System.Linq;
using SorterControls.View;
using Sorting.CompetePools;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignSorterEvalVm : SorterEvalVmOld
    {
        public DesignSorterEvalVm()
            : base(
                        sortResult: DesignSorterEval(),
                        lineBrushes: LineBrushFactory.GradedBlueBrushes(keyCount),
                        switchBrushes: LineBrushFactory.GradedRedBrushes(keyCount),
                        width: 8,
                        height: 150,
                        showUnusedSwitches: false,
                        showStages: false
                 )
        {
        }

        private const int keyCount = 16;
        private static ISortResult DesignSorterEval()
        {
            return SorterEvals.TestSorterEval(keyCount, 123, 800);
        }
    }

    public class DesignSorterEvalVms
    {
        public DesignSorterEvalVms()
        {
            for (var i = 0; i < 200; i++)
            {
                _sorterEvalVms.Add(
                    new SorterEvalVmOld
                        (
                            sortResult: SorterEvals.TestSorterEval(KeyCount, 1323 + i, 700),
                            lineBrushes: LineBrushFactory.GradedBlueBrushes(KeyCount),
                            switchBrushes: LineBrushFactory.GradedRedBrushes(KeyCount),
                            width: 8,
                            height: 150,
                            showUnusedSwitches: false,
                            showStages: false
                        )
                    );
            }

            _sorterEvalVms = new ObservableCollection<SorterEvalVmOld>(_sorterEvalVms.OrderBy(e => e.SwitchesUsed));
        }

        private const int KeyCount = 8;

        private ObservableCollection<SorterEvalVmOld> _sorterEvalVms
                = new ObservableCollection<SorterEvalVmOld>();
        public ObservableCollection<SorterEvalVmOld> SorterEvalVms
        {
            get { return _sorterEvalVms; }
            set { _sorterEvalVms = value; }
        }
    }
}
