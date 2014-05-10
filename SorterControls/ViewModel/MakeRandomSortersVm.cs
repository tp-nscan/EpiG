using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SorterControls.View;
using Sorting.Evals;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class MakeRandomSortersVm : ViewModelBase
    {
        public MakeRandomSortersVm()
        {
            _sorterCount = 10;
            _showUnused = false;
            _showStages = false;
            _seed = 1234;
            _keyPairCount = 100;
            _keyCount = 10;
            _displaySize = 3;
        }

        #region MakeSortersCommand

        RelayCommand _makeSortersCommand;
        public ICommand MakeSortersCommand
        {
            get
            {
                return _makeSortersCommand ?? (_makeSortersCommand
                    = new RelayCommand
                        (
                            param => OnMakeSortersCommand(),
                            param => CanMakeSortersCommand()
                        ));
            }
        }

        protected void OnMakeSortersCommand()
        {
            MakeSorterEvals();
        }

        bool CanMakeSortersCommand()
        {
            return true;
        }

        private List<ISorterEval> _sorterEvals = new List<ISorterEval>();
        private List<ISorterEval> SorterEvals
        {
            get { return _sorterEvals; }
            set { _sorterEvals = value; }
        }

        void MakeSorterEvals()
        {
            _sorterEvals.Clear();
            _sorterVms.Clear();

            for (var i = 0; i < SorterCount; i++)
            {
                var newSorterEval = Sorting.TestData.SorterEvals.TestSorterEval(KeyCount, Seed + i, KeyPairCount);

                _sorterEvals.Add(newSorterEval);

                _sorterVms.InsertWhen(
                        MakeSorterEvalVm(newSorterEval), ev => ev.SwitchesUsed > newSorterEval.SwitchUseCount
                    );
            }
        }

        void MakeSorterEvalVms()
        {
            _sorterVms.Clear();

            foreach (var sorterEval in SorterEvals.OrderBy(e=>e.SwitchUseCount))
            {
                SorterVms.Add(
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
        public ObservableCollection<ISorterVm> SorterVms
        {
            get { return _sorterVms; }
            set
            {
                _sorterVms = value;
                OnPropertyChanged("SorterEvalVms");
            }
        }

        #endregion // MakeSortersCommand

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

        private int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
            set
            {
                _keyCount = value;
                OnPropertyChanged("KeyCount");
                MakeSorterEvals();
            }
        }

        private int _keyPairCount;
        public int KeyPairCount
        {
            get { return _keyPairCount; }
            set
            {
                _keyPairCount = value;
                OnPropertyChanged("KeyPairCount");
                MakeSorterEvals();
            }
        }

        private int _seed;
        public int Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                OnPropertyChanged("Seed");
                MakeSorterEvals();
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

        private int _sorterCount;
        public int SorterCount
        {
            get { return _sorterCount; }
            set
            {
                _sorterCount = value;
                OnPropertyChanged("SorterCount");
                MakeSorterEvals();
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
