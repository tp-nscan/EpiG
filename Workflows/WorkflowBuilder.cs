using System;
using System.Threading.Tasks;
using MathUtils.Collections;

namespace Workflows
{
    public interface IWorkflowBuilder<T> : IGuid where T : IEntity
    {
        Task<IWorkflow<T>> Make();
        string WorkflowBuilderType { get; }
    }

    public static class WorkflowBuilder
    {
        public static IWorkflowBuilder<T> MakePassthrough<T>
        (
            Guid guid,
            T result
        ) where T : IEntity
        {
            return new WorkflowBuilderPassThrough<T>
            (
                guid: guid,
                result: result
            );
        }
    }

    public class WorkflowBuilderPassThrough<T> : WorkflowBuilderBase<T>
        where T : IEntity
    {
        public const string Name = "WorkflowBuilder.Passthrough";

        public WorkflowBuilderPassThrough
            (
                Guid guid, 
                T result
            )
            : base(guid, Name)
        {
            _result = result;
        }

        private readonly T _result;
        public T Result
        {
            get { return _result; }
        }

        public override Task<IWorkflow<T>> Make()
        {
            var taskSource = new TaskCompletionSource<IWorkflow<T>>();
            taskSource.SetResult(
                new PassThroughWorkflow<T>
                    (
                        workflowBuilder: this,
                        result: Result
                    )
            );
            return taskSource.Task;
        }
    }

    public abstract class WorkflowBuilderBase<T> : IWorkflowBuilder<T> where T : IEntity
    {
        protected WorkflowBuilderBase
            (
                Guid guid, 
                string workflowBuilderType
            )
        {
            _guid = guid;
            _workflowBuilderType = workflowBuilderType;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract Task<IWorkflow<T>> Make();

        private readonly string _workflowBuilderType;
        public string WorkflowBuilderType
        {
            get { return _workflowBuilderType; }
        }

    }

}