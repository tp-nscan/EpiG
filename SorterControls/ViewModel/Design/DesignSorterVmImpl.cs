using SorterControls.View;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignSorterVmImpl : SorterVmImpl
    {
        public DesignSorterVmImpl()
            : base
            (
                sorterEval: SorterEvals.TestSorterEval(__keyCount, 123, 800),
                lineBrushes: LineBrushFactory.GradedBlueBrushes(__keyCount),
                width: 8
            )
        { }

        private const int __keyCount = 16;
    }
}
