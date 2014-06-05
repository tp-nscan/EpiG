using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Sorting.Evals;
using Utils.BackgroundWorkers;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class MakeRandomSortersVm : ViewModelBase
    {
        public MakeRandomSortersVm()
        {
            _sorterCount = 10;
            _seed = 1234;
            _keyPairCount = 100;
            _keyCount = 10;
            SorterGalleryVm = new SorterGalleryVm
                (
                    keyCount: KeyCount,
                    sorterEvals: SorterEvals,
                    displaySize: 3,
                    showStages: false,
                    showUnused: false,
                    sorterDisplayCount: 10
                );
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

        async void OnMakeSortersCommand()
        {
            IsBusy = true;
            SyncGui();
            await MakeSorterEvalsAsync();
            IsBusy = false;
        }

        bool CanMakeSortersCommand()
        {
            return !_isBusy && WasGuiChanged;
        }

        #endregion // MakeSortersCommand


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

        #region CancelMakeSortersCommand

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        RelayCommand _cancelMakeSortersCommand;
        public ICommand CancelMakeSortersCommand
        {
            get
            {
                return _cancelMakeSortersCommand ?? (_cancelMakeSortersCommand
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

        #endregion // CancelMakeSortersCommand


        async Task MakeSorterEvalsAsync()
        {
            IsBusy = true;
            Reset();

            await SorterEvalBackgroundWorker.Start(_cancellationTokenSource);
            IsBusy = false;
        }


        private IEnumerativeBackgroundWorker<int, ISorterEval> _sorterEvalBackgroundWorker;
        private IEnumerativeBackgroundWorker<int, ISorterEval> SorterEvalBackgroundWorker
        {
            get
            {
                if (_sorterEvalBackgroundWorker == null)
                {

                    _sorterEvalBackgroundWorker = EnumerativeBackgroundWorker.Make(
                            inputs: Enumerable.Range(0, SorterCount),
                            mapper: (i, c) => IterationResult.Make
                                (
                                    Sorting.TestData.SorterEvals
                                            .TestSorterEval(KeyCount, Seed + i, KeyPairCount), 
                                    ProgressStatus.StepComplete
                                )
                        );

                    _updateSubscription = _sorterEvalBackgroundWorker.OnIterationResult
                                            .Subscribe(UpdateSorterResults);
                }

                return _sorterEvalBackgroundWorker;
            }
        }

        void UpdateSorterResults(IIterationResult<ISorterEval> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)
            {
                _sorterEvals.Add(result.Data);
                _sorterGalleryVm.AddSorterEval(result.Data);
            }
        }

        private IDisposable _updateSubscription;
        void Reset()
        {
            if (_updateSubscription != null)
            {
                _updateSubscription.Dispose();
            }
            _sorterEvalBackgroundWorker = null;
            SorterEvals.Clear();
            MakeSorterGalleryVm();
            _cancellationTokenSource = new CancellationTokenSource();
        }


        private readonly List<ISorterEval> _sorterEvals = new List<ISorterEval>();
        private List<ISorterEval> SorterEvals
        {
            get { return _sorterEvals; }
        }


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

        private int _lastKeyPairCount;
        private int _keyPairCount;
        public int KeyPairCount
        {
            get { return _keyPairCount; }
            set
            {
                _keyPairCount = value;
                OnPropertyChanged("KeyPairCount");
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
                if (_lastKeyPairCount != _keyPairCount)
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
            _lastKeyPairCount = _keyPairCount;
            _lastKeyCount = _keyCount;
        }

        #endregion


        #region MakeSorterGallery related

        void MakeSorterGalleryVm()
        {
            SorterGalleryVm = new SorterGalleryVm
                (
                    keyCount: KeyCount,
                    sorterEvals: SorterEvals,
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
