using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SorterControls.View;
using Sorting.Evals;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterGalleryVm : ViewModelBase
    {
        public SorterGalleryVm
            (
                int keyCount, 
                IEnumerable<ISorterEval> sorterEvals, 
                int displaySize, 
                bool showStages, 
                bool showUnused
            )
        {
            _keyCount = keyCount;
            _displaySize = displaySize;
            _showStages = showStages;
            _showUnused = showUnused;
            _sorterEvals = sorterEvals.ToList();

            MakeSorterEvalVms();
        }

        public void AddSorterEval(ISorterEval sorterEval)
        {
            _sorterEvals.Add(sorterEval);
            _sorterVms.InsertWhen(
                MakeSorterEvalVm(sorterEval), ev => ev.SwitchesUsed > sorterEval.SwitchUseCount
            );
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly List<ISorterEval> _sorterEvals;
        private IEnumerable<ISorterEval> SorterEvals
        {
            get { return _sorterEvals; }
        }

        void MakeSorterEvalVms()
        {
            SorterEvalVms.Clear();
            foreach (var sorterEval in SorterEvals.OrderBy(e => e.SwitchUseCount))
            {
                SorterEvalVms.Add(
                        MakeSorterEvalVm(sorterEval)
                    );
            }
        }

        ISorterVm MakeSorterEvalVm(ISorterEval sorterEval)
        {
            if (ShowStages)
            {
                return sorterEval.ToStagedSorterVm
                (
                    lineBrushes: LineBrushFactory.GradedBlueBrushes(KeyCount),
                    switchBrushes: LineBrushFactory.GradedRedBrushes(KeyCount),
                    width: DisplaySizeToSwitchWith(DisplaySize),
                    height: DisplaySizeToHeight(DisplaySize),
                    showUnusedSwitches: ShowUnused
                );
            }

            return sorterEval.ToUnStagedSorterVm
            (
                    lineBrushes: LineBrushFactory.GradedBlueBrushes(KeyCount),
                    switchBrushes: LineBrushFactory.GradedRedBrushes(KeyCount),
                    width: DisplaySizeToSwitchWith(DisplaySize),
                    height: DisplaySizeToHeight(DisplaySize),
                    showUnusedSwitches: ShowUnused
            );

        }

        private ObservableCollection<ISorterVm> _sorterVms = new ObservableCollection<ISorterVm>();
        public ObservableCollection<ISorterVm> SorterEvalVms
        {
            get { return _sorterVms; }
            set
            {
                _sorterVms = value;
                OnPropertyChanged("SorterEvalVms");
            }
        }


        private int _displaySize;
        public int DisplaySize
        {
            get { return _displaySize; }
            set
            {
                _displaySize = value;
                OnPropertyChanged("DisplaySize");
                MakeSorterEvalVms();
            }
        }

        private bool _showStages;
        public bool ShowStages
        {
            get { return _showStages; }
            set
            {
                if (_showStages == value)
                {
                    return;
                }
                _showStages = value;
                MakeSorterEvalVms();
                OnPropertyChanged("ShowStages");
            }
        }

        private bool _showUnused;
        public bool ShowUnused
        {
            get { return _showUnused; }
            set
            {
                if (_showUnused == value)
                {
                    return;
                }
                _showUnused = value;
                MakeSorterEvalVms();
                OnPropertyChanged("ShowUnused");
            }
        }

        static int DisplaySizeToSwitchWith(int displaySize)
        {
            if (displaySize == 1)
            {
                return 2;
            }
            if (displaySize == 2)
            {
                return 4;
            }
            if (displaySize == 3)
            {
                return 5;
            }
            if (displaySize == 4)
            {
                return 7;
            }
            if (displaySize == 5)
            {
                return 9;
            }
            return 11;
        }

        static int DisplaySizeToHeight(int displaySize)
        {
            if (displaySize == 1)
            {
                return 80;
            }
            if (displaySize == 2)
            {
                return 120;
            }
            if (displaySize == 3)
            {
                return 160;
            }
            if (displaySize == 4)
            {
                return 200;
            }
            if (displaySize == 5)
            {
                return 240;
            }
            return 280;
        }

    }
}
