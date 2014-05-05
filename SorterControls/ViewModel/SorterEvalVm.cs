using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Sorting.Evals;
using Sorting.Stages;
using Sorting.StagesOld;

namespace SorterControls.ViewModel
{
    public class SorterEvalVm
    {
        public SorterEvalVm(
            ISorterEval sorterEval, 
            List<Brush> lineBrushes,
            List<Brush> switchBrushes,
            int width,
            int height,
            bool showUnusedSwitches,
            bool showStages
         )
        {
            _sorterEval = sorterEval;
            LineBrushes = lineBrushes;
            SwitchBrushes = switchBrushes;
            _height = height;
            _width = width;
            ShowUnusedSwitches = showUnusedSwitches;
            ShowStages = showStages;
            SetSwitchVms();
        }

        void SetSwitchVms()
        {
            _switchVms.Clear();

            if (ShowStages)
            {
                SetStagedSwitchVms();
                return;
            }

            SetUnstagedSwitchVms();
        }

        void SetUnstagedSwitchVms()
        {
            for (var i = 0; i < SorterEval.KeyPairCount; i++)
            {
                if ((SorterEval.SwitchEvals[i].UseCount == 0) && !ShowUnusedSwitches)
                {
                    continue;
                }

                var keyPair = SorterEval.KeyPair(i);
                var switchBrushIndex = Math.Ceiling(
                        (SorterEval.SwitchEvals[i].UseCount * SwitchBrushes.Count)
                            /
                        SorterEval.SwitchableGroupCount
                    );

                SwitchVms.Add(new SwitchVm(keyPair, SorterEval.KeyCount, LineBrushes, Width)
                {
                    SwitchBrush = SwitchBrushes[(int)switchBrushIndex]
                });
            }
        }

        void SetStagedSwitchVms()
        {
            var stagedKeyPairs = SorterEval.SwitchEvals
                                   .ToSorterStages(SorterEval.KeyPairCount)
                                   .SelectMany(st => st.KeyPairs)
                                   .Cast<ISwitchEval>();

            foreach (var stagedKeyPair in stagedKeyPairs)
            {
                var switchBrushIndex = Math.Ceiling(
                        (stagedKeyPair.UseCount * SwitchBrushes.Count)
                            /
                        SorterEval.SwitchableGroupCount
                    );

                SwitchVms.Add(new SwitchVm(stagedKeyPair, SorterEval.KeyCount, LineBrushes, Width)
                {
                    SwitchBrush = SwitchBrushes[(int)switchBrushIndex]
                });
            }
        }

        bool ShowUnusedSwitches { get; set; }

        bool ShowStages { get; set; }

        private List<Brush> LineBrushes { get; set; }

        private List<Brush> SwitchBrushes { get; set; }

        public int SwitchesUsed
        {
            get { return SorterEval.SwitchUseCount; }
        }

        public bool Success
        {
            get { return SorterEval.Success; }
        }

        private readonly int _height;
        public int Height
        {
            get { return _height; }
        }

        private readonly int _width;
        public int Width
        {
            get { return _width; }
        }

        public int KeyCount
        {
            get { return SorterEval.KeyCount; }
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
