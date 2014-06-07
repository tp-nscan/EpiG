using System;
using MathUtils.Collections;

namespace Workflows
{
    public interface IWorkflow<T> : IGuid
    {
        IWorkflowBuilder<T> WorkflowBuilder { get; }
        T Result { get; }
    }

    public static class Workflow
    {
        public static IWorkflow<T> ToPassThroughWorkflow<T>(this T item, Guid guid)
        {
            return new WorkflowImpl<T>
                (
                    workflowBuilder: WorkflowBuilder.MakePassthrough
                    (
                        guid: guid,
                        result: item
                    ),
                    result: item
                );
        }
    }

    public class WorkflowImpl<T> : IWorkflow<T>
    {
        public WorkflowImpl(IWorkflowBuilder<T> workflowBuilder, T result)
        {
            _workflowBuilder = workflowBuilder;
            _result = result;
        }

        private readonly IWorkflowBuilder<T> _workflowBuilder;
        public IWorkflowBuilder<T> WorkflowBuilder
        {
            get { return _workflowBuilder; }
        }

        private readonly T _result;
        public T Result
        {
            get { return _result; }
        }

        public Guid Guid
        {
            get { return WorkflowBuilder.Guid; }
        }
    }
}
