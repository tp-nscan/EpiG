using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Sorting.CompetePools;
using WpfUtils;
using Xceed.Wpf.DataGrid.FilterCriteria;

namespace SorterControls.ViewModel
{
    public class SorterEvalVmOld : ViewModelBase
    {
        public SorterEvalVmOld(
            ISortResult sortResult, 
            List<Brush> lineBrushes,
            List<Brush> switchBrushes,
            int width,
            int height,
            bool showUnusedSwitches,
            bool showStages
         )
        {
            _sortResult = sortResult;
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
            for (var i = 0; i < SortResult.Sorter.KeyPairCount; i++)
            {
                if ((SortResult.SwitchUseList[i] == 0) && !ShowUnusedSwitches)
                {
                    continue;
                }

                var keyPair = SortResult.Sorter.KeyPair(i);
                var switchBrushIndex = Math.Ceiling(
                        (SortResult.SwitchUseList[i] * SwitchBrushes.Count)
                            /
                        SortResult.SwitchableGroupCount
                    );

                SwitchVms.Add(new SwitchVm(keyPair, SortResult.Sorter.KeyCount, LineBrushes, Width)
                {
                    SwitchBrush = SwitchBrushes[(int)switchBrushIndex]
                });
            }
        }

        void SetStagedSwitchVms()
        {
            //for (var i = 0; i < SorterEval.Reduce(); i++)
            //{
            //    if ((SorterEval.SwitchUseList[i] == 0) && !ShowUnusedSwitches)
            //    {
            //        continue;
            //    }

            //    var keyPair = SorterEval.Sorter.KeyPair(i);
            //    var switchBrushIndex = Math.Ceiling(
            //            (SorterEval.SwitchUseList[i] * SwitchBrushes.Count)
            //                /
            //            SorterEval.SwitchableGroupCount
            //        );

            //    SwitchVms.Add(new SwitchVm(keyPair, SorterEval.Sorter.KeyCount, LineBrushes, Width)
            //    {
            //        SwitchBrush = SwitchBrushes[(int)switchBrushIndex]
            //    });
            //}
        }

        bool ShowUnusedSwitches { get; set; }

        bool ShowStages { get; set; }

        private List<Brush> LineBrushes { get; set; }

        private List<Brush> SwitchBrushes { get; set; }

        public int SwitchesUsed
        {
            get { return SortResult.SwitchUseCount; }
        }

        public bool Success
        {
            get { return SortResult.Success; }
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
            get { return SortResult.Sorter.KeyCount; }
        }

        private readonly ISortResult _sortResult;
        ISortResult SortResult
        {
            get { return _sortResult; }
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
