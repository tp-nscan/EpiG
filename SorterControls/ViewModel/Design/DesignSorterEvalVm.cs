using System.Collections.ObjectModel;
using System.Linq;
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
            return SorterEvals.TestSorterEval(KeyCount, 123, 800);
        }
    }

    public class DesignSorterEvalVms
    {
        public DesignSorterEvalVms()
        {
                    //            DesignSorterEval(),
                    //LineBrushFactory.GradedBlueBrushes(KeyCount),
                    //LineBrushFactory.GradedRedBrushes(KeyCount)

            for (var i = 0; i < 200; i++)
            {
                _sorterEvalVms.Add(
                    new SorterEvalVm(
                            SorterEvals.TestSorterEval(KeyCount, 1323 + i, 700),
                            LineBrushFactory.GradedBlueBrushes(KeyCount),
                            LineBrushFactory.GradedRedBrushes(KeyCount)
                        )
                    );
            }

            _sorterEvalVms = new ObservableCollection<SorterEvalVm>(_sorterEvalVms.OrderBy(e => e.SwitchesUsed));
        }

        private const int KeyCount = 8;

        private ObservableCollection<SorterEvalVm> _sorterEvalVms
                = new ObservableCollection<SorterEvalVm>();
        public ObservableCollection<SorterEvalVm> SorterEvalVms
        {
            get { return _sorterEvalVms; }
            set { _sorterEvalVms = value; }
        }
    }
}
