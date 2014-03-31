using System.Windows.Media;
using SorterControls.View;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.Design
{
    public class DesignSwitchVm : SwitchVm
    {
        public DesignSwitchVm() : base(
                keyPair: KeyPairRepository.AtIndex(5),
                keyCount: keyCount,
                lineBrushes: LineBrushFactory.GradedBlueBrushes(keyCount),
                width: 8
            )
        {
            SwitchBrush = Brushes.Red;
        }

        const int keyCount = 16;

    }
}
