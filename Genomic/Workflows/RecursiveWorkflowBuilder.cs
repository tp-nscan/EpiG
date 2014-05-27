using System;

namespace Genomic.Workflows
{
    public interface IRecursiveWorkflowBuilder<T> : IWorkflowBuilder<T>
    {
        new IRecursiveWorkflow<T> Make();
        IWorkflow<T> InitialWorkflow { get; }
        Func<T, int, T> UpdateFunc { get; }
    }

    public static class RecursiveWorkflowBuilder
    {
        public static IRecursiveWorkflowBuilder<T> Make<T>
            (
                Guid guid,
                string workflowBuilderType,
                int seed,
                IWorkflow<T> initialWorkflow,
                Func<T, int, T> updateFunc
            )
        {
            return new RecursiveWorkflowBuilderImpl<T>
                (
                    workflowBuilderType: workflowBuilderType,
                    seed: seed,
                    initialWorkflow: initialWorkflow,
                    updateFunc: updateFunc,
                    guid: guid
                );
        }
    }

    public class RecursiveWorkflowBuilderImpl<T> : IRecursiveWorkflowBuilder<T>
    {
        public RecursiveWorkflowBuilderImpl
        (
            Guid guid, 
            string workflowBuilderType, 
            int seed, 
            IWorkflow<T> initialWorkflow,
            Func<T, int, T> updateFunc
        )
        {
            _workflowBuilderType = workflowBuilderType;
            _seed = seed;
            _initialWorkflow = initialWorkflow;
            _updateFunc = updateFunc;
            _guid = guid;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IWorkflow<T> Make()
        {
            return ((IRecursiveWorkflowBuilder<T>)this).Make();
        }

        private readonly string _workflowBuilderType;
        IRecursiveWorkflow<T> IRecursiveWorkflowBuilder<T>.Make()
        {
            return null;
        }

        public string WorkflowBuilderType
        {
            get { return _workflowBuilderType; }
        }


        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private readonly IWorkflow<T> _initialWorkflow;
        public IWorkflow<T> InitialWorkflow
        {
            get { return _initialWorkflow; }
        }

        private readonly Func<T, int, T> _updateFunc;
        public Func<T, int, T> UpdateFunc
        {
            get { return _updateFunc; }
        }
    }
}
