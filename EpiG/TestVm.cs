using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MathUtils.Rand;
using SorterGenome;
using SorterGenome.CompPool;
using Sorting.Sorters;
using Utils.BackgroundWorkers;
using Workflows;
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

        #region TestCommand

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

        async void OnTestCommand()
        {
            IsBusy = true;
            await DoTestAsync();
            IsBusy = false;
        }


        bool CanTestCommand()
        {
            return !IsBusy;
        }

        #endregion // TestCommand


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


        #region CancelTestCommand

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


        async Task DoTestAsync()
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

        private IRecursiveParamBackgroundWorker<IRecursiveWorkflow<ISorterCompPool<ISorter>>, int> MakeSorterEvalBackgroundWorker()
        {
            return null;
        }

        void UpdateResults(IIterationResult<IRecursiveWorkflow<ISorterCompPool<ISorter>>> result)
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


        private string _result;
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
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
