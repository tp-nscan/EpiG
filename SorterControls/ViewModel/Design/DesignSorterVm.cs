using SorterControls.View;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignSorterVm : SorterVm
    {
        public DesignSorterVm()
            : base
            (
                sorter: Sorters.TestSorter(keyCount, 1234, 50),
                lineBrushes: LineBrushFactory.GradedBlueBrushes(keyCount),
                width: 8,
                height: 150
            )
        { }

        private const int keyCount = 16;
    }
}
