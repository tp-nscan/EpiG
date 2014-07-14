using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.BackgroundWorkers
{
    public interface IRecursiveParamBackgroundWorker<T, X>
    {
        bool IsComplete { get; }
        IObservable<IIterationResult<T>> OnIterationResult { get; }
        Task Start(CancellationTokenSource cancellationTokenSource);
        void Stop();
        int CurrentIteration { get; }
        T CurrentState { get; }
        IReadOnlyList<X> Parameters { get; }
    }


    public static class RecursiveParamBackgroundWorker
    {
        public static IRecursiveParamBackgroundWorker<T, X> Make<T, X>
            (
                IReadOnlyList<X> parameters,
                Func<T, X, CancellationToken, IIterationResult<T>> recursion,
                T initialState
            )
        {
            return new RecursiveParamBackgroundWorkerImpl<T, X>
                (
                    parameters: parameters,
                    recursion: recursion,
                    initialState: initialState
                );
        }
    }

    public class RecursiveParamBackgroundWorkerImpl<T, X> : IRecursiveParamBackgroundWorker<T, X>
    {
        public RecursiveParamBackgroundWorkerImpl
            (
                IReadOnlyList<X> parameters,
                Func<T, X, CancellationToken, IIterationResult<T>> recursion,
                T initialState
            )
        {
            _parameters = parameters;
            _recursion = recursion;
            _currentState = initialState;
            _currentIteration = 0;
        }

        private readonly Func<T, X, CancellationToken, IIterationResult<T>> _recursion;

        private readonly Subject<IIterationResult<T>> _onIterationResult = new Subject<IIterationResult<T>>();
        public IObservable<IIterationResult<T>> OnIterationResult
        {
            get { return _onIterationResult; }
        }

        public async Task Start(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            var keepGoing = true;
            for (; _currentIteration < Parameters.Count; )
            {

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    _onIterationResult.OnNext(
                        IterationResult.Make(default(T), ProgressStatus.StepIncomplete)
                    );
                    keepGoing = false;
                }

                var result = IterationResult.Make(default(T), ProgressStatus.StepIncomplete);
                await Task.Run
                    (
                        () => result = _recursion(
                                                    CurrentState, 
                                                    Parameters[_currentIteration], 
                                                    _cancellationTokenSource.Token
                                                 )
                    );

                if (result.ProgressStatus != ProgressStatus.StepComplete)
                {
                    keepGoing = false;
                }
                else
                {
                    _currentState = result.Data;
                    _currentIteration++;
                    _onIterationResult.OnNext(result);
                }

                if (!keepGoing)
                {
                    break;
                }
            }
        }


        private CancellationTokenSource _cancellationTokenSource;
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private int _currentIteration;
        public int CurrentIteration
        {
            get { return _currentIteration; }
        }

        private T _currentState;
        public T CurrentState
        {
            get { return _currentState; }
        }

        private readonly IReadOnlyList<X> _parameters;
        public IReadOnlyList<X> Parameters
        {
            get { return _parameters; }
        }

        public bool IsComplete
        {
            get { return CurrentIteration == Parameters.Count; }
        }
    }
}
