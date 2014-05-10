using SorterControls.View;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignSorterVm : SorterVmImpl
    {
        public DesignSorterVm()
            : base
            (
                sorterEval: SorterEvals.TestSorterEval(__keyCount, 123, 800),
                lineBrushes: LineBrushFactory.GradedBlueBrushes(__keyCount),
                switchBrushes: LineBrushFactory.GradedRedBrushes(__keyCount),
                width: 8,
                height:150,
                showUnusedSwitches: true
            )
        { }

        private const int __keyCount = 16;
    }
}
