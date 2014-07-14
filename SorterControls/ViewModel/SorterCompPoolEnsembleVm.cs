using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MathUtils.Rand;
using SorterGenome.CompPool;
using SorterGenome.CompPool.Ensemble;
using System.Linq;
using Utils.BackgroundWorkers;
using Workflows;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterCompPoolEnsembleVm : ViewModelBase
    {
        public SorterCompPoolEnsembleVm()
        {
            _legacyRate = 0.04;
            _mutationRate = 0.02;
            _cubRate = 0.24;

            _sorterCount = 50;
            _seed = 1234;
            _keyPairCount = 1250;
            _keyCount = 14;

            _sorterCompPoolParameterType = SorterCompPoolParameterType.MutationRate;
            _replicas = 10;
            _increment = 0.002;
            _startingValue = 0.02;
            _deletionRate = 0.005;
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

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPoolEnsemble>, int> _sorterCompPoolBackgroundWorker;
        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPoolEnsemble>, int> SorterCompPoolBackgroundWorker
        {
            get
            {
                return _sorterCompPoolBackgroundWorker;
            }
        }

        private IRecursiveWorkflow<ISorterCompPoolEnsemble> _initialState;
        private IRecursiveWorkflow<ISorterCompPoolEnsemble> InitialState
        {
            get
            {
                return _initialState ??
                    (
                    _initialState = PermutationStyle ? MakePermutation() : MakeStandard()
                    );
            }
        }

        IRecursiveWorkflow<ISorterCompPoolEnsemble> MakeStandard()
        {
            return SorterCompPoolEnsemble.InitStandardFromSeed
                (
                    seed: Seed,
                    orgCount: SorterCount,
                    seqenceLength: KeyPairCount,
                    keyCount: KeyCount,
                    deletionRate: DeletionRate,
                    insertionRate: InsertionRate,
                    mutationRate: MutationRate,
                    legacyRate: LegacyRate,
                    cubRate: CubRate,
                    startingValue: StartingValue,
                    increment: Increment,
                    replicas: Replicas,
                    sorterCompPoolParameterType: SorterCompPoolParameterType
                
                ).ToPassThroughWorkflow(_rando.NextGuid())
                 .ToRecursiveWorkflowRndWlk(_rando.NextGuid());

        }

        IRecursiveWorkflow<ISorterCompPoolEnsemble> MakePermutation()
        {
            return SorterCompPoolEnsemble.InitPermuterFromSeed
            (
                seed: Seed,
                orgCount: SorterCount,
                permutationCount: (KeyPairCount * 2) / KeyCount,
                degree: KeyCount,
                deletionRate: DeletionRate,
                insertionRate: InsertionRate,
                mutationRate: MutationRate,
                legacyRate: LegacyRate,
                cubRate: CubRate,
                startingValue: StartingValue,
                increment: Increment,
                replicas: Replicas,
                sorterCompPoolParameterType: SorterCompPoolParameterType

            ).ToPassThroughWorkflow(_rando.NextGuid())
             .ToRecursiveWorkflowRndWlk(_rando.NextGuid());
        }


        void Reset()
        {
            _initialState = null;
        }

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPoolEnsemble>, int>
                        MakeSorterEvalBackgroundWorker()
        {
            return
                _sorterCompPoolBackgroundWorker = RecursiveParamBackgroundWorker.Make(
                        parameters: _rando.ToIntEnumerator().Take(1).ToList(),
                        recursion: (w, i, c) => IterationResult.Make
                            (
                                w.Update(_rando.NextGuid(), i),
                                ProgressStatus.StepComplete
                            ),
                        initialState: InitialState
                    );
        }

        void UpdateResults(IIterationResult<IRecursiveWorkflow<ISorterCompPoolEnsemble>> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)


                if (result.Data.Result.SorterCompPoolStageType == SorterCompPoolStageType.MakeNextGeneration)
                {
                    var scps = result.Data.Result.SorterCompPools.ToList();
                    if (scps[0].Generation % 1 == 0)
                    {
                        foreach (var sorterCompPool in scps)
                        {
                            var sorterEvals =
                                sorterCompPool.PhenotypeEvals.Select(ev => ev.Value.SorterEval).ToList();

                            SorterPoolSummaryVms.Add
                            (
                                new SorterPoolSummaryVm2
                                (
                                    sorterEvals: sorterEvals,
                                    generation: sorterCompPool.Generation,
                                    name: sorterCompPool.Name,
                                    tweak: Double.Parse(sorterCompPool.Name) * 10 - 0.2
                                )
                            );
                        }
                    }
                }

            _initialState = result.Data;
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

        private int _replicas;
        public int Replicas
        {
            get { return _replicas; }
            set
            {
                _replicas = value;
                Reset();
                OnPropertyChanged("Replicas");
            }
        }

        private double _startingValue;
        public double StartingValue
        {
            get { return _startingValue; }
            set
            {
                _startingValue = value;
                OnPropertyChanged("StartingValue");
            }
        }

        private double _increment;
        public double Increment
        {
            get { return _increment; }
            set
            {
                _increment = value;
                Reset();
                OnPropertyChanged("Increment");
            }
        }

        private SorterCompPoolParameterType _sorterCompPoolParameterType;
        public SorterCompPoolParameterType SorterCompPoolParameterType
        {
            get { return _sorterCompPoolParameterType; }
            set
            {
                _sorterCompPoolParameterType = value;
                Reset();
                OnPropertyChanged("SorterCompPoolParameterType");
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

        private double _legacyRate;
        public double LegacyRate
        {
            get { return _legacyRate; }
            set
            {
                _legacyRate = value;
                Reset();
                OnPropertyChanged("LegacyRate");
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

        private double _deletionRate;
        public double DeletionRate
        {
            get { return _deletionRate; }
            set
            {
                _deletionRate = value;
                Reset();
                OnPropertyChanged("DeletionRate");
            }
        }

        private double _insertionRate;
        public double InsertionRate
        {
            get { return _insertionRate; }
            set
            {
                _insertionRate = value;
                Reset();
                OnPropertyChanged("InsertionRate");
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

        private bool _permutationStyle;
        public bool PermutationStyle
        {
            get { return _permutationStyle; }
            set
            {
                _permutationStyle = value; 
                Reset();
                OnPropertyChanged("PermutationStyle");
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

        private ObservableCollection<SorterPoolSummaryVm2> _sorterPoolVms
            = new ObservableCollection<SorterPoolSummaryVm2>();
        public ObservableCollection<SorterPoolSummaryVm2> SorterPoolSummaryVms
        {
            get { return _sorterPoolVms; }
            set
            {
                _sorterPoolVms = value;
                OnPropertyChanged("SorterPoolSummaryVms");
            }
        }

    }
}
