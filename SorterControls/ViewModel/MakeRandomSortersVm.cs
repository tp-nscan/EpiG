﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MathUtils;
using SorterControls.View;
using Sorting.CompetePools;
using Sorting.Evals;
using Sorting.Sorters;
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
            _sorterEvalVms.Clear();

            for (var i = 0; i < SorterCount; i++)
            {
                var newSorterEval = Sorting.TestData.SorterEvals.TestSorterEval(KeyCount, Seed + i, KeyPairCount);

                _sorterEvals.Add(newSorterEval);

                _sorterEvalVms.InsertWhen(
                        MakeSorterEvalVm(newSorterEval), ev => ev.SwitchesUsed > newSorterEval.SwitchUseCount
                    );
            }
        }

        void MakeSorterEvalVms()
        {
            _sorterEvalVms.Clear();

            foreach (var sorterEval in SorterEvals.OrderBy(e=>e.SwitchUseCount))
            {
                SorterEvalVms.Add(
                        MakeSorterEvalVm(sorterEval)
                    );
            }
        }

        SorterEvalVm MakeSorterEvalVm(ISorterEval sorterEval)
        {
            return new SorterEvalVm
            (
                sorterEval: sorterEval,
                lineBrushes: LineBrushFactory.GradedBlueBrushes(KeyCount),
                switchBrushes: LineBrushFactory.GradedRedBrushes(KeyCount),
                width: DisplaySizeToSwitchWith(DisplaySize),
                height: DisplaySizeToHeight(DisplaySize),
                showUnusedSwitches: ShowUnused,
                showStages: ShowStages
            );
        }

        private ObservableCollection<SorterEvalVm> _sorterEvalVms
                    = new ObservableCollection<SorterEvalVm>();
        public ObservableCollection<SorterEvalVm> SorterEvalVms
        {
            get { return _sorterEvalVms; }
            set
            {
                _sorterEvalVms = value;
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
                if (value)
                {
                    _showUnused = false;
                    OnPropertyChanged("ShowUnused");
                }
                _showStages = value;
                OnPropertyChanged("ShowStages");
                MakeSorterEvalVms();
            }
        }

        private bool _showUnused;
        public bool ShowUnused
        {
            get { return _showUnused; }
            set
            {
                if (value)
                {
                    _showStages = false;
                    OnPropertyChanged("ShowStages");
                }
                _showUnused = value;
                OnPropertyChanged("ShowUnused");
                MakeSorterEvalVms();
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
                return 6;
            }
            if (displaySize == 4)
            {
                return 8;
            }
            if (displaySize == 5)
            {
                return 10;
            }
            return 12;
        }

        static int DisplaySizeToHeight(int displaySize)
        {
            if (displaySize == 1)
            {
                return 40;
            }
            if (displaySize == 2)
            {
                return 80;
            }
            if (displaySize == 3)
            {
                return 120;
            }
            if (displaySize == 4)
            {
                return 160;
            }
            if (displaySize == 5)
            {
                return 200;
            }
            return 240;
        }
    }
}
