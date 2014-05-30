using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utils.BackgroundWorkers
{
    public interface IRecursiveParamBackgroundWorker<T, X>
    {
        IObservable<IIterationResult<T>> OnIterationResult { get; }
        Task Start();
        void Stop();
        int CurrentIteration { get; }
        T CurrentState { get; }
        IReadOnlyList<X> Parameters { get; }
    }


    public static class RecursiveParamBackgroundWorker
    {

    }

    public class RecursiveParamBackgroundWorkerImpl<T, X> : IRecursiveParamBackgroundWorker<T, X>
    {
        public RecursiveParamBackgroundWorkerImpl(IReadOnlyList<X> parameters)
        {
            _parameters = parameters;
        }


        private IObservable<IIterationResult<T>> _onIterationResult;
        public IObservable<IIterationResult<T>> OnIterationResult
        {
            get { return _onIterationResult; }
        }

        public Task Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
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

        private IReadOnlyList<X> _parameters;
        public IReadOnlyList<X> Parameters
        {
            get { return _parameters; }
        }
    }
}
