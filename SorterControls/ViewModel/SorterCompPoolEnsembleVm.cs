using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
            _legacyCount = 1;
            _mutationRate = 0.03;
            _cubCount = 4;

            _colonySize = 16;
            _seed = (ushort) DateTime.Now.Ticks;
            _runName = Seed.ToString();
            _keyPairCount = 400;
            _keyCount = 10;

            _sorterCompPoolParameterType = SorterCompPoolParameterType.CubCount;
            _colonyCount = 64;
            _increment = 1;
            _startingValue = 6;
            _paramSteps = 1;
            _permutationStyle = true;
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


        #region CopySettingsCommand

        RelayCommand _copySettingsCommand;
        public ICommand CopySettingsCommand
        {
            get
            {
                return _copySettingsCommand ?? (_copySettingsCommand
                    = new RelayCommand
                        (
                            param => OnCopySettingsCommand(),
                            param => CanCopySettingsCommand()
                        ));
            }
        }

        void OnCopySettingsCommand()
        {
            string hdrs =
                "Seed\tKeys\tSwitches\tColonyCount\tMutation\tInsertion\tDeletion\tLegacy\tCub\tPermute\tReps\tVarParam\tVarStart\tVarDelta\tPhenotyper\tPhenoParam\tNextGen";

            string vals = string.Format
                (
                    "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t",
                    Seed,
                    KeyCount,
                    KeyPairCount,
                    ColonySize,
                    MutationRate,
                    InsertionRate,
                    DeletionRate,
                    LegacyCount,
                    CubCount,
                    PermutationStyle,
                    ColonyCount,
                    SorterCompPoolParameterType,
                    StartingValue,
                    Increment
                );

            Clipboard.SetText(string.Format("{0}\n{1}", hdrs, vals));
        }

        bool CanCopySettingsCommand()
        {
            return true;
        }

        #endregion // CopySettingsCommand


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
                    orgCount: ColonySize,
                    seqenceLength: KeyPairCount,
                    keyCount: KeyCount,
                    deletionRate: DeletionRate,
                    insertionRate: InsertionRate,
                    mutationRate: MutationRate,
                    legacyCount: LegacyCount,
                    cubCount: CubCount,
                    stepCount: ParamSteps,
                    startingValue: StartingValue,
                    increment: Increment,
                    reps: ColonyCount,
                    sorterCompPoolParameterType: SorterCompPoolParameterType
                
                ).ToPassThroughWorkflow(_rando.NextGuid())
                 .ToRecursiveWorkflowRndWlk(_rando.NextGuid());

        }

        IRecursiveWorkflow<ISorterCompPoolEnsemble> MakePermutation()
        {
            return SorterCompPoolEnsemble.InitPermuterFromSeed
            (
                seed: Seed,
                orgCount: ColonySize,
                permutationCount: (KeyPairCount * 2) / KeyCount,
                degree: KeyCount,
                deletionRate: DeletionRate,
                insertionRate: InsertionRate,
                mutationRate: MutationRate,
                legacyCount: LegacyCount,
                cubCount: CubCount,
                stepCount: ParamSteps,
                startingValue: StartingValue,
                increment: Increment,
                reps: ColonyCount,
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
                    var scpgs = result.Data.Result.SorterCompPools.GroupBy(p => p.Name).ToList();
                    GenerationCount = scpgs[0].First().Generation;

                    foreach (var group in scpgs)
                    {
                        SorterPoolSummaryVms.Add
                        (
                            new SorterCompPoolEnsembleSummaryVm
                                (
                                    run: RunName,
                                    replications: GenerationCount * ColonySize,
                                    colonySize: ColonySize,
                                    legacyCount: group.First().LegacyCount,
                                    cubCount: group.First().CubCount,
                                    mutationRate: group.First().MutationRate,
                                    colonyCount: ColonyCount,
                                    bestValues: group.Select(p => p.PhenotypeEvals.Select(ev => ev.Value.SorterEval)
                                                                    .Where(ev => ev.Success)
                                                                    .Min(ev => ev.SwitchUseCount)
                                                            ).ToList()
                                )
                        );
                    }

                    //var scps = result.Data.Result.SorterCompPools.ToList();

                    //GenerationCount = scps[0].Generation;

                    //SorterPoolSummaryVms.Add
                    //(
                    //    new SorterCompPoolEnsembleSummaryVm
                    //        (
                    //            name: EnsembleName,
                    //            generation: GenerationCount,
                    //            bestValues: scps.Select(p => p.PhenotypeEvals.Select(ev=>ev.Value.SorterEval)
                    //                                            .Where(ev => ev.Success)  
                    //                                            .Min(ev=>(double)ev.SwitchUseCount)
                    //                                    ).ToList()
                    //        )
                    //);


                    //if (scps[0].Generation % 1 == 0)
                    //{
                    //    foreach (var sorterCompPool in scps)
                    //    {
                    //        var sorterEvals =
                    //            sorterCompPool.PhenotypeEvals.Select(ev => ev.Value.SorterEval).ToList();

                    //        SorterPoolSummaryVms.Add
                    //        (
                    //            new SorterPoolSummaryVm2
                    //            (
                    //                sorterEvals: sorterEvals,
                    //                generation: sorterCompPool.Generation,
                    //                name: sorterCompPool.Name,
                    //                tweak: Double.Parse(sorterCompPool.Name) * 10 - 0.2
                    //            )
                    //        );
                    //    }
                    //}
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

        private int _paramSteps;
        public int ParamSteps
        {
            get { return _paramSteps; }
            set
            {
                _paramSteps = value;
                Reset();
                OnPropertyChanged("ParamSteps");
            }
        }

        private int _colonyCount;
        public int ColonyCount
        {
            get { return _colonyCount; }
            set
            {
                _colonyCount = value;
                Reset();
                OnPropertyChanged("ColonyCount");
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

        private int _legacyCount;
        public int LegacyCount
        {
            get { return _legacyCount; }
            set
            {
                _legacyCount = value;
                Reset();
                OnPropertyChanged("LegacyCount");
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

        private int _cubCount;
        public int CubCount
        {
            get { return _cubCount; }
            set
            {
                _cubCount = value;
                Reset();
                OnPropertyChanged("CubCount");
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

        private int _colonySize;
        public int ColonySize
        {
            get { return _colonySize; }
            set
            {
                _colonySize = value;
                Reset();
                OnPropertyChanged("ColonySize");
            }
        }


        private string _runName;

        public string RunName
        {
            get { return _runName; }
            set
            {
                _runName = value;
                OnPropertyChanged("RunName");
            }
        }

        #endregion

        //private ObservableCollection<SorterPoolSummaryVm2> _sorterPoolVms
        //    = new ObservableCollection<SorterPoolSummaryVm2>();
        //public ObservableCollection<SorterPoolSummaryVm2> SorterPoolSummaryVms
        //{
        //    get { return _sorterPoolVms; }
        //    set
        //    {
        //        _sorterPoolVms = value;
        //        OnPropertyChanged("SorterPoolSummaryVms");
        //    }
        //}

        private ObservableCollection<SorterCompPoolEnsembleSummaryVm> _sorterPoolVms
            = new ObservableCollection<SorterCompPoolEnsembleSummaryVm>();
        public ObservableCollection<SorterCompPoolEnsembleSummaryVm> SorterPoolSummaryVms
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
