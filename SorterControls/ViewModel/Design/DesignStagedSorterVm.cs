using SorterControls.View;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignStagedSorterVm : StagedSorterVmImpl
    {
        public DesignStagedSorterVm() 
            : base
            (
                sorterEval: SorterEvals.TestSorterEval(__keyCount, 123, 800),
                lineBrushes: LineBrushFactory.GradedBlueBrushes(__keyCount),
                width: 8
            )
        {
        }

        private const int __keyCount = 16;
    }
}
