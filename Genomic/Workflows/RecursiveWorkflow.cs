using System;
using MathUtils.Collections;

namespace Genomic.Workflows
{
    public interface IRecursiveWorkflow<T> : IWorkflow<T> 
    {
        IRecursiveWorkflowBuilder<T> RecursiveWorkflowBuilder { get; }
    }

    public class RecursiveWorkflow
    {
        public static IRecursiveWorkflow<T>  Make<T>
            (
                Guid guid,
                IRecursiveWorkflowBuilder<T> recursiveWorkflowBuilder,
                T result
            )
        {
            return new RecursiveWorkflowImpl<T>
                (
                    guid:  guid,
                    recursiveWorkflowBuilder:  recursiveWorkflowBuilder,
                    result: result
                );
        }
    }

    public class RecursiveWorkflowImpl<T> : IRecursiveWorkflow<T>
    {
        public RecursiveWorkflowImpl
            (
                Guid guid, 
                IRecursiveWorkflowBuilder<T> recursiveWorkflowBuilder, 
                T result
            )
        {
            _guid = guid;
            _recursiveWorkflowBuilder = recursiveWorkflowBuilder;
            _result = result;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly IRecursiveWorkflowBuilder<T> _recursiveWorkflowBuilder;
        public IRecursiveWorkflowBuilder<T> RecursiveWorkflowBuilder
        {
            get { return _recursiveWorkflowBuilder; }
        }

        private readonly T _result;

        public IWorkflowBuilder<T> WorkflowBuilder
        {
            get { return _recursiveWorkflowBuilder; }
        }

        public T Result
        {
            get { return _result; }
        }
    }
}
