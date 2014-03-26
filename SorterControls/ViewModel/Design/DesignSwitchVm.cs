using System.Collections.Generic;
using System.Windows.Media;
using MathUtils.Rand;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.Design
{
    public class DesignSwitchVm
    {
        public static readonly List<Brush> SolidColorBrushes = new List<Brush>();

        static DesignSwitchVm()
        {
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
                SolidColorBrushes.Add(scb);
            }
        }

        public IKeyPair KeyPair
        {
            get { return KeyPairRepository.AtIndex(5); }
        }

        public int KeyCount { get { return 9; } }

        public List<Brush> LineBrushes
        {
            get { return SolidColorBrushes; }
        }
    }
}
