using System;
using MathUtils.Collections;

namespace Workflows
{
    public interface IWorkflow<T> : IGuid, IGuidParts where T : IEntity
    {
        IWorkflowBuilder<T> WorkflowBuilder { get; }
        T Result { get; }
        string WorkflowType { get; }
    }

    public static class Workflow
    {
        public static IWorkflow<T> ToPassThroughWorkflow<T>
            (
                this T item, 
                Guid guid
            ) where T : IEntity
        {
            return new PassThroughWorkflow<T>
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

    public class PassThroughWorkflow<T> : IWorkflow<T> where T : IEntity
    {
        public const string Name = "Workflow.Passthrough";

        public PassThroughWorkflow
            (
                IWorkflowBuilder<T> workflowBuilder, 
                T result
            )
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

        public string WorkflowType
        {
            get { return Name; }
        }

        public object GetPart(Guid key)
        {
            if (Result.Guid == key)
            {
                return Result;
            }
            return Result.GetPart(key);
        }

        public Guid Guid
        {
            get { return WorkflowBuilder.Guid; }
        }

    }
}
