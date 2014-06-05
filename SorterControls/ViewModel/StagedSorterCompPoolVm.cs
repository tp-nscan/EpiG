using System.Linq;
using System.Threading;
using System.Windows.Input;
using Sorting.Evals;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class StagedSorterCompPoolVm : ViewModelBase
    {
        public StagedSorterCompPoolVm()
        {
            _sorterCount = 10;
            _seed = 1234;
            _stageCount = 10;
            _keyCount = 10;
            SorterGalleryVm = new SorterGalleryVm
                (
                    keyCount: 100,
                    sorterEvals: Enumerable.Empty<ISorterEval>(),
                    displaySize: 3,
                    showStages: false,
                    showUnused: false,
                    sorterDisplayCount: 10
                );
        }

        private int _generationCount;
        public int GenerationCount
        {
            get { return _generationCount; }
            set
            {
                _generationCount = value;
                OnPropertyChanged("GenerationCount");
            }
        }

        #region StartSimulationCommand

        RelayCommand _startSimulationCommand;
        public ICommand StartSimulationCommand
        {
            get
            {
                return _startSimulationCommand ?? (_startSimulationCommand
                    = new RelayCommand
                        (
                            param => OnMakeSortersCommand(),
                            param => CanMakeSortersCommand()
                        ));
            }
        }

        async void OnMakeSortersCommand()
        {
            IsBusy = true;
            //await MakeSorterEvals();
            IsBusy = false;
        }

        bool CanMakeSortersCommand()
        {
            return !_isBusy;
        }

        #endregion // StartSimulationCommand

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        #region CancelSimulationCommand

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        RelayCommand _cancelSimulationCommand;
        public ICommand CancelSimulationCommand
        {
            get
            {
                return _cancelSimulationCommand ?? (_cancelSimulationCommand
                    = new RelayCommand
                        (
                            param => OnCancelMakeSortersCommand(),
                            param => CanCancelMakeSortersCommand()
                        ));
            }
        }

        void OnCancelMakeSortersCommand()
        {
            _cancellationTokenSource.Cancel();
        }

        bool CanCancelMakeSortersCommand()
        {
            return true;
        }

        #endregion // CancelSimulationCommand



        #region Gui Binders

        private int _lastKeyCount;
        private int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
            set
            {
                _keyCount = value;
                OnPropertyChanged("KeyCount");
            }
        }

        private int _lastStageCount;
        private int _stageCount;
        public int StageCount
        {
            get { return _stageCount; }
            set
            {
                _stageCount = value;
                OnPropertyChanged("StageCount");
            }
        }

        private int _lastSeed;
        private int _seed;
        public int Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                OnPropertyChanged("Seed");
            }
        }

        private int _lastSorterCount;
        private int _sorterCount;
        public int SorterCount
        {
            get { return _sorterCount; }
            set
            {
                _sorterCount = value;
                OnPropertyChanged("SorterCount");
            }
        }

        bool WasGuiChanged
        {
            get
            {
                if (_lastSorterCount != _sorterCount)
                {
                    return true;
                }
                if (_lastSeed != _seed)
                {
                    return true;
                }
                if (_lastStageCount != _stageCount)
                {
                    return true;
                }
                if (_lastKeyCount != _keyCount)
                {
                    return true;
                }
                return false;
            }
        }

        void SyncGui()
        {
            _lastSorterCount = _sorterCount;
            _lastSeed = _seed;
            _lastStageCount = _stageCount;
            _lastKeyCount = _keyCount;
        }

        #endregion


        #region MakeSorterGallery related

        void MakeSorterGalleryVm()
        {
            SorterGalleryVm = new SorterGalleryVm
                (
                    keyCount: KeyCount,
                    sorterEvals: null,
                    displaySize: DisplaySize,
                    showStages: ShowStages,
                    showUnused: ShowUnused,
                    sorterDisplayCount: SorterDisplayCount
                );
        }

        private SorterGalleryVm _sorterGalleryVm;
        public SorterGalleryVm SorterGalleryVm
        {
            get { return _sorterGalleryVm; }
            set
            {
                _sorterGalleryVm = value;
                OnPropertyChanged("SorterGalleryVm");
            }
        }

        public int DisplaySize
        {
            get { return SorterGalleryVm.DisplaySize; }
            set
            {
                SorterGalleryVm.DisplaySize = value;
            }
        }

        public bool ShowStages
        {
            get { return SorterGalleryVm.ShowStages; }
            set { SorterGalleryVm.ShowStages = value; }
        }

        public bool ShowUnused
        {
            get { return SorterGalleryVm.ShowUnused; }
            set { SorterGalleryVm.ShowUnused = value; }
        }

        public int SorterDisplayCount
        {
            get { return SorterGalleryVm.SorterDisplayCount; }
            set
            {
                SorterGalleryVm.SorterDisplayCount = value;
            }
        }

        #endregion


    }
}
