﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Sorting.Evals;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public static class UnStagedSorterVm
    {
        public static ISorterVm ToUnStagedSorterVm
            (
                this ISorterEval sorterEval,
                List<Brush> lineBrushes,
                List<Brush> switchBrushes,
                int width,
                int height,
                bool showUnusedSwitches
            )
        {
            return new UnstagedSorterVmImpl
                (
                    sorterEval: sorterEval,
                    lineBrushes: lineBrushes,
                    switchBrushes: switchBrushes,
                    width: width,
                    height: height,
                    showUnusedSwitches: showUnusedSwitches
                );
        }
    }

    public class UnstagedSorterVmImpl: ViewModelBase, ISorterVm
    {
        public UnstagedSorterVmImpl
            (
                ISorterEval sorterEval,
                List<Brush> lineBrushes,
                List<Brush> switchBrushes,
                int width,
                int height,
                bool showUnusedSwitches
            )
        {
            _sorterEval = sorterEval;
            _lineBrushes = lineBrushes;
            _switchBrushes = switchBrushes;
            _showUnusedSwitches = showUnusedSwitches;
            _height = height;
            _width = width;

            for (var i = 0; i < SorterEval.KeyPairCount; i++)
            {
                if ((SorterEval.SwitchEvals[i].UseCount <1) && !ShowUnusedSwitches)
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

        private readonly List<Brush> _lineBrushes;
        private List<Brush> LineBrushes
        {
            get { return _lineBrushes; }
        }

        private readonly int _height;
        public int Height
        {
            get { return _height; }
        }

        private readonly bool _showUnusedSwitches;
        public bool ShowUnusedSwitches
        {
            get { return _showUnusedSwitches; }
        }

        private readonly ISorterEval _sorterEval;
        ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }

        private readonly List<Brush> _switchBrushes;
        private List<Brush> SwitchBrushes
        {
            get { return _switchBrushes; }
        }

        private ObservableCollection<SwitchVm> _switchVms = new ObservableCollection<SwitchVm>();
        public ObservableCollection<SwitchVm> SwitchVms
        {
            get { return _switchVms; }
            set { _switchVms = value; }
        }

        public SorterVmType SorterVmType { get { return SorterVmType.Unstaged; } }

        public int SwitchesUsed
        {
            get { return SorterEval.SwitchUseCount; }
        }

        public bool Success
        {
            get { return SorterEval.Success; }
        }

        private readonly int _width;
        public int Width
        {
            get { return _width; }
        }
    }
}
