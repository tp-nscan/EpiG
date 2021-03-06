﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MathUtils.Rand;
using SorterGenome.CompPool;
using Sorting.Evals;
using System.Linq;
using Utils.BackgroundWorkers;
using Workflows;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterCompPoolVm : ViewModelBase
    {
        public SorterCompPoolVm()
        {
            _legacyCount = 1;
            _mutationRate = 0.03;
            _cubCount = 6;

            _colonyCount = 64;
            Seed = 1234;
            _keyPairCount = 400;
            _keyCount = 10;

            SorterPoolVm = new SorterPoolVm
                (
                    keyCount: KeyCount,
                    sorterEvals: Enumerable.Empty<ISorterEval>(),
                    displaySize: 3,
                    showStages: false,
                    showUnused: false,
                    generation: 0,
                    sorterDisplayCount: 10,
                    sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                    name: ""
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

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool>, int> _sorterCompPoolBackgroundWorker;
        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool>, int> SorterCompPoolBackgroundWorker
        {
            get
            {
                return _sorterCompPoolBackgroundWorker;
            }
        }

        private IRecursiveWorkflow<ISorterCompPool> _initialState;
        private IRecursiveWorkflow<ISorterCompPool> InitialState
        {
            get
            {
                return _initialState ??
                    (
                        _initialState = PermutationStyle ? MakePermutation() : MakeStandard()
                    );
            }
        }

        IRecursiveWorkflow<ISorterCompPool> MakeStandard()
        {
            return SorterCompPool.InitStandardFromSeed
                (
                    seed: Seed,
                    orgCount: ColonyCount,
                    seqenceLength: KeyPairCount,
                    keyCount: KeyCount,
                    deletionRate: DeletionRate,
                    insertionRate: InsertionRate,
                    mutationRate: MutationRate,
                    legacyCount: LegacyCount,
                    cubCount: CubCount,
                    name: ""
                ).ToPassThroughWorkflow(_rando.NextGuid())
                .ToRecursiveWorkflowRndWlk(_rando.NextGuid());
        }

        IRecursiveWorkflow<ISorterCompPool> MakePermutation()
        {
            return SorterCompPool.InitPermuterFromSeed
                (
                    seed: Seed,
                    orgCount: ColonyCount,
                    permutationCount: (KeyPairCount * 2) / KeyCount,
                    degree: KeyCount,
                    deletionRate: DeletionRate,
                    insertionRate: InsertionRate,
                    mutationRate: MutationRate,
                    legacyCount: LegacyCount,
                    cubCount: CubCount,
                    name: ""
                ).ToPassThroughWorkflow(_rando.NextGuid())
                .ToRecursiveWorkflowRndWlk(_rando.NextGuid());
        }


        void Reset()
        {
            _initialState = null;
        }

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool>, int>
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

        void UpdateResults(IIterationResult<IRecursiveWorkflow<ISorterCompPool>> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)

                SorterPoolVm.SorterCompPoolStageType = result.Data.Result.SorterCompPoolStageType;

                if (result.Data.Result.SorterCompPoolStageType == SorterCompPoolStageType.MakeNextGeneration)
                {
                    {

                        var sorterEvals =
                            result.Data.Result.PhenotypeEvals.Select(ev => ev.Value.SorterEval).ToList();

                        SorterPoolVm = new SorterPoolVm
                            (
                                keyCount: KeyCount,
                                sorterEvals: sorterEvals,
                                displaySize: SorterPoolVm.SorterGalleryVm.DisplaySize,
                                showStages: SorterPoolVm.SorterGalleryVm.ShowStages,
                                showUnused: SorterPoolVm.SorterGalleryVm.ShowUnused,
                                generation: result.Data.Result.Generation,
                                sorterDisplayCount: SorterPoolVm.SorterGalleryVm.SorterDisplayCount,
                                sorterCompPoolStageType: result.Data.Result.SorterCompPoolStageType,
                                name:""
                            );

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

        private IRando _rando;
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

        #endregion


        #region SorterPoolVm related

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

        #endregion

    }
}


