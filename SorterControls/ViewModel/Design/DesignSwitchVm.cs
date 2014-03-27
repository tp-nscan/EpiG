using System.Collections.Generic;
using System.Windows.Media;
using MathUtils.Rand;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.Design
{
    public class DesignSwitchVm : SwitchVm
    {
        public DesignSwitchVm() : base(
                keyPair: KeyPairRepository.AtIndex(5), 
                keyCount: 16,
                lineBrushes: SolidColorBrushes()
            )
        {
            
        }

        static List<Brush> SolidColorBrushes()
        {
            var _solidColorBrushes = new List<Brush>();
            var randy = Rando.Fast(333);
            for (int i = 0; i < 16; i++)
            {
                var scb = new SolidColorBrush(
                    new Color
                    {
                        ScA = (float)1.0,
                        ScB = (float)randy.NextDouble(),
                        ScG = (float)randy.NextDouble(),
                        ScR = (float)randy.NextDouble()
                    });

                scb.Freeze();
                _solidColorBrushes.Add(scb);
            }

            return _solidColorBrushes;
        }
    }
}
