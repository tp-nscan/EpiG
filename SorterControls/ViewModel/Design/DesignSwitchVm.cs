using System.Windows.Media;
using SorterControls.View;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.Design
{
    public class DesignSwitchVm : SwitchVm
    {
        public DesignSwitchVm() : base(
                keyPair: KeyPairRepository.AtIndex(5),
                keyCount: KeyCount,
                lineBrushes: LineBrushFactory.GradedBlueBrushes(KeyCount)
            )
        {
            SwitchBrush = Brushes.Red;
        }

        const int KeyCount = 16;

    }
}
