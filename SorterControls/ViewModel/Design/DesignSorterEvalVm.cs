using SorterControls.View;
using Sorting.CompetePools;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignSorterEvalVm : SorterEvalVm
    {
        public DesignSorterEvalVm()
            : base(
                    DesignSorterEval(), 
                    LineBrushFactory.GradedBlueBrushes(KeyCount),
                    LineBrushFactory.GradedRedBrushes(KeyCount)
            )
        {
        }

        private const int KeyCount = 16;
        private static ISorterEval DesignSorterEval()
        {
            return SorterEvals.TestSorterEval(KeyCount, 200);
        }
    }
}
