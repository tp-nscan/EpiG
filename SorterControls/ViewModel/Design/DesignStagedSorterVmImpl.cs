using SorterControls.View;
using Sorting.Stages;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignStagedSorterVmImpl : StagedSorterVmImpl
    {
        public DesignStagedSorterVmImpl() 
            : base
            (
                stagedSorter: Sorters.TestSorter(KeyCount, 1234, 50).ToStagedSorter(),
                lineBrushes: LineBrushFactory.GradedBlueBrushes(KeyCount),
                width: 8
            )
        {
        }
        private const int KeyCount = 16;
    }
}
