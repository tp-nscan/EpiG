using SorterControls.View;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignSorterVm : SorterVm
    {
        public DesignSorterVm()
            : base
            (
                Sorters.TestSorter(KeyCount, 1234, 50), 
                LineBrushFactory.GradedBlueBrushes(KeyCount)
            )
        {
        }

        private const int KeyCount = 16;
    }
}
