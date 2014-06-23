using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Genomic.Genomes;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils.Rand;
using SorterGenome;
using Sorting.Evals;
using System.Linq;
using Sorting.Sorters;
using Utils.BackgroundWorkers;
using Workflows;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterCompPoolVm : ViewModelBase
    {
        public SorterCompPoolVm()
        {
            _multiplicationRate = 2.0;
            _mutationRate = 0.03;
            _cubRate = 0.25;

            _sorterCount = 10;
            _seed = 1234;
            _keyPairCount = 100;
            _keyCount = 10;

            SorterPoolVm = new SorterPoolVm
                (
                    keyCount: KeyCount,
                    sorterEvals: Enumerable.Empty<ISorterEval>(),
                    displaySize: 3,
                    showStages: false,
                    showUnused: false,
                    generation: 0
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
            await DoSorterCompPoolAsync();
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


        async Task DoSorterCompPoolAsync()
        {
            IsBusy = true;
            NextRound();
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                NextRound();
                await SorterCompPoolBackgroundWorker.Start(_cancellationTokenSource);
            }
            IsBusy = false;
        }

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool<ISorter>>, int> _sorterCompPoolBackgroundWorker;
        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool<ISorter>>, int> SorterCompPoolBackgroundWorker
        {
            get
            {
                return _sorterCompPoolBackgroundWorker;
            }
        }

        private IRecursiveWorkflow<ISorterCompPool<ISorter>> _initialState;
        private IRecursiveWorkflow<ISorterCompPool<ISorter>> InitialState
        {
            get
            {
                return _initialState ??
                    (
                        _initialState = SorterCompPool.InitStandardFromSeed
                        (
                            seed: Seed,
                            orgCount: SorterCount,
                            seqenceLength: KeyPairCount,
                            keyCount: KeyCount,
                            mutationRate: MutationRate,
                            multiplicationRate: MultiplicationRate,
                            cubRate: CubRate
                        ).ToPassThroughWorkflow(Guid.NewGuid())
                         .ToRecursiveWorkflowRndWlk()
                    );
            }
        }


        void Reset()
        {
            _initialState = null;
        }

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool<ISorter>>, int>
                        MakeSorterEvalBackgroundWorker()
        {
            return
                    _sorterCompPoolBackgroundWorker = RecursiveParamBackgroundWorker.Make(
                            parameters: _rando.ToIntEnumerator().Take(1).ToList(),
                            recursion: (w, i, c) => IterationResult.Make
                                (
                                    w.Update(i),
                                    ProgressStatus.StepComplete
                                ),
                            initialState: InitialState
                        );
        }

        void UpdateResults(IIterationResult<IRecursiveWorkflow<ISorterCompPool<ISorter>>> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)
            {
                SorterPoolVm.Generation = result.Data.Result.Generation;
                _initialState = result.Data;
            }
        }

        private IDisposable _updateSubscription;
        void NextRound()
        {
            if (_updateSubscription != null)
            {
                _updateSubscription.Dispose();
            }
            if (
                    (_sorterCompPoolBackgroundWorker == null)
                    ||
                    (_sorterCompPoolBackgroundWorker != null && _sorterCompPoolBackgroundWorker.IsComplete)

                )
            {
                _sorterCompPoolBackgroundWorker = MakeSorterEvalBackgroundWorker();
            }

            _updateSubscription = _sorterCompPoolBackgroundWorker.OnIterationResult.Subscribe(UpdateResults);

            //_sorterEvals.Clear();
            //_sorterVms.Clear();
            _cancellationTokenSource = new CancellationTokenSource();
        }


        #region Gui Binders

        private int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
            set
            {
                _keyCount = value;
                Reset();
                OnPropertyChanged("KeyCount");
            }
        }

        private int _keyPairCount;
        public int KeyPairCount
        {
            get { return _keyPairCount; }
            set
            {
                _keyPairCount = value;
                Reset();
                OnPropertyChanged("KeyPairCount");
            }
        }

        private double _multiplicationRate;
        public double MultiplicationRate
        {
            get { return _multiplicationRate; }
            set
            {
                _multiplicationRate = value;
                Reset();
                OnPropertyChanged("MultiplicationRate");
            }
        }

        private double _mutationRate;
        public double MutationRate
        {
            get { return _mutationRate; }
            set
            {
                _mutationRate = value;
                Reset();
                OnPropertyChanged("MutationRate");
            }
        }

        private double _cubRate;
        public double CubRate
        {
            get { return _cubRate; }
            set
            {
                _cubRate = value;
                Reset();
                OnPropertyChanged("CubRate");
            }
        }

        private IRando _rando = Rando.Fast(123);
        private int _seed;
        public int Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                Reset();
                _rando = Rando.Fast(_seed);
                OnPropertyChanged("Seed");
            }
        }

        private int _sorterCount;
        public int SorterCount
        {
            get { return _sorterCount; }
            set
            {
                _sorterCount = value;
                Reset();
                OnPropertyChanged("SorterCount");
            }
        }

        #endregion


        #region MakeSorterPoolVm related

        void MakeSorterPoolVm(int generation)
        {
            SorterPoolVm = new SorterPoolVm
                (
                    keyCount: KeyCount,
                    sorterEvals: Enumerable.Empty<ISorterEval>(),
                    displaySize: DisplaySize,
                    showStages: ShowStages,
                    showUnused: ShowUnused,
                    generation: generation
                );
        }

        private SorterPoolVm _sorterPoolVm;
        public SorterPoolVm SorterPoolVm
        {
            get { return _sorterPoolVm; }
            set
            {
                _sorterPoolVm = value;
                OnPropertyChanged("SorterPoolVm");
            }
        }

        public int DisplaySize
        {
            get { return SorterPoolVm.SorterGalleryVm.DisplaySize; }
            set
            {
                SorterPoolVm.SorterGalleryVm.DisplaySize = value;
            }
        }

        public bool ShowStages
        {
            get { return SorterPoolVm.SorterGalleryVm.ShowStages; }
            set
            {
                SorterPoolVm.SorterGalleryVm.ShowStages = value;
            }
        }

        public bool ShowUnused
        {
            get { return SorterPoolVm.SorterGalleryVm.ShowUnused; }
            set
            {
                SorterPoolVm.SorterGalleryVm.ShowUnused = value;
            }
        }

        public int Generation
        {
            get { return SorterPoolVm.Generation; }
            //set
            //{
            //    SorterPoolVm.Generation = value;
            //}
        }

        #endregion


    }
}


