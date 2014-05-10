using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Evals;
using Sorting.Stages;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterStageVm : ViewModelBase
    {
        public SorterStageVm
            (
                ISorterStage<ISwitchEval> sorterStage,
                List<Brush> lineBrushes,
                List<Brush> switchBrushes,
                int width,
                bool showUnusedSwitches,
                int switchableGroupCount
            )
        {
            _sorterStage = sorterStage;
            _lineBrushes = lineBrushes;
            _switchBrushes = switchBrushes;
            _showUnusedSwitches = showUnusedSwitches;
            _width = width;
            _switchableGroupCount = switchableGroupCount;


            for (var i = 0; i < SorterStage.KeyPairCount; i++)
            {
                var keyPair = SorterStage.KeyPair(i);
                if ((keyPair.UseCount < 1) && !ShowUnusedSwitches)
                {
                    continue;
                }

                var switchBrushIndex = Math.Ceiling(
                        (keyPair.UseCount * SwitchBrushes.Count)
                            /
                        SwitchableGroupCount
                    );

                SwitchVms.Add(new SwitchVm(keyPair, SorterStage.KeyCount, LineBrushes, Width)
                {
                    SwitchBrush = SwitchBrushes[(int)switchBrushIndex]
                });
            }

        }

        private readonly ISorterStage<ISwitchEval> _sorterStage;
        ISorterStage<ISwitchEval> SorterStage
        {
            get { return _sorterStage; }
        }

        private ObservableCollection<SwitchVm> _switchVms = new ObservableCollection<SwitchVm>();
        public ObservableCollection<SwitchVm> SwitchVms
        {
            get { return _switchVms; }
            set { _switchVms = value; }
        }

        private readonly List<Brush> _lineBrushes;
        private List<Brush> LineBrushes
        {
            get { return _lineBrushes; }
        }

        private readonly bool _showUnusedSwitches;
        public bool ShowUnusedSwitches
        {
            get { return _showUnusedSwitches; }
        }

        private readonly List<Brush> _switchBrushes;
        private List<Brush> SwitchBrushes
        {
            get { return _switchBrushes; }
        }


        private readonly int _switchableGroupCount;
        public int SwitchableGroupCount
        {
            get { return _switchableGroupCount; }
        }

        private readonly int _width;
        public int Width
        {
            get { return _width; }
        }
    }
}
