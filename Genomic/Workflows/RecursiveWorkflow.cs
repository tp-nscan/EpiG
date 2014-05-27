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
                    recursiveWorkflowBuilder:  recursiveWorkflowBuilder,
                    result: result
                );
        }
    }

    public class RecursiveWorkflowImpl<T> : IRecursiveWorkflow<T>
    {
        public RecursiveWorkflowImpl
            (
                IRecursiveWorkflowBuilder<T> recursiveWorkflowBuilder, 
                T result
            )
        {
            _recursiveWorkflowBuilder = recursiveWorkflowBuilder;
            _result = result;
        }

        public Guid Guid
        {
            get { return RecursiveWorkflowBuilder.Guid; }
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
