using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.CompetePools;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterEvalVm : ViewModelBase
    {
        public SorterEvalVm(ISorterEval sorterEval, List<Brush> lineBrushes, List<Brush> switchBrushes)
        {
            _sorterEval = sorterEval;

            //foreach (var keyPair in SorterEval.Sorter.KeyPairs)
            //{
            //    SwitchVms.Add(new SwitchVm(keyPair, SorterEval.Sorter.KeyCount, lineBrushes) { SwitchBrush = Brushes.Red} );
            //}

            for (var i = 0; i < sorterEval.Sorter.KeyPairCount; i++)
            {
                var keyPair = sorterEval.Sorter.KeyPair(i);
                var switchBrushIndex = (sorterEval.SwitchUseList[i] * switchBrushes.Count) / sorterEval.SwitchableGroupCount;

                SwitchVms.Add(new SwitchVm(keyPair, SorterEval.Sorter.KeyCount, lineBrushes)
                {
                    SwitchBrush = switchBrushes[(int) switchBrushIndex]
                });
            }
        }

        private readonly ISorterEval _sorterEval;
        ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }

        private ObservableCollection<SwitchVm> _switchVms = new ObservableCollection<SwitchVm>();
        public ObservableCollection<SwitchVm> SwitchVms
        {
            get { return _switchVms; }
            set { _switchVms = value; }
        }

        public string StringValue
        {
            get { return String.Empty; }
        }
    }
}
