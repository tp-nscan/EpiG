using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Genomic.Workflows;
using MathUtils.Rand;
using SorterGenome;
using Utils.BackgroundWorkers;
using WpfUtils;

namespace EpiG
{
    public class TestVm : ViewModelBase
    {
        private readonly IRando _rando;
        public TestVm()
        {
            _rando = Rando.Fast(123);
        }

        #region MakeSortersCommand

        RelayCommand _testCommand;
        public ICommand TestCommand
        {
            get
            {
                return _testCommand ?? (_testCommand
                    = new RelayCommand
                        (
                            param => OnTestCommand(),
                            param => CanTestCommand()
                        ));
            }
        }

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool>, int> _sorterCompPoolBackgroundWorker;
        private IDisposable _updateSubscription;
        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool>, int> SorterEvalBackgroundWorker
        {
            get
            {
                return _sorterCompPoolBackgroundWorker;
            }
        }

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool>, int> MakeSorterEvalBackgroundWorker()
        {
            return
                    _sorterCompPoolBackgroundWorker = RecursiveParamBackgroundWorker.Make(
                            parameters: _rando.ToIntEnumerator().Take(3).ToList(),
                            recursion: (w, i, c) => IterationResult.Make
                                (
                                    w.Update(i),
                                    ProgressStatus.StepComplete
                                ),
                                initialState: SorterCompPool.Make().ToPassThroughWorkflow(Guid.NewGuid())
                                                                   .ToRecursiveWorkflowRw()
                        );
        }

        void UpdateResults(IIterationResult<IRecursiveWorkflow<ISorterCompPool>> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)
            {
                Result = result.Data.Result.Generation.ToString();
                //_sorterEvals.Add(result.Data);
                //_sorterVms.InsertWhen(
                //        MakeSorterEvalVm(result.Data), ev => ev.SwitchesUsed > result.Data.SwitchUseCount
                //    );
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        async void OnTestCommand()
        {
            IsBusy = true;
            await MakeSorterEvals();
            IsBusy = false;
        }

        void Reset()
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

        async Task MakeSorterEvals()
        {
            IsBusy = true;
            Reset();
            while (! _cancellationTokenSource.IsCancellationRequested)
            {
                Reset();
                await SorterEvalBackgroundWorker.Start(_cancellationTokenSource);
            }
            IsBusy = false;
        }

        bool CanTestCommand()
        {
            return !IsBusy;
        }

        #endregion // TestCommand


        #region CancelMakeSortersCommand

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        RelayCommand _cancelTestCommand;
        public ICommand CancelTestCommand
        {
            get
            {
                return _cancelTestCommand ?? (_cancelTestCommand
                    = new RelayCommand
                        (
                            param => OnCancelTestCommand(),
                            param => CanCancelTestCommand()
                        ));
            }
        }

        void OnCancelTestCommand()
        {
            _cancellationTokenSource.Cancel();
            IsBusy = false;
        }

        bool CanCancelTestCommand()
        {
            return IsBusy;
        }

        #endregion // CancelMakeSortersCommand


        private bool _isBusy;
        private string _result;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
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
            }
        }
    }
}
