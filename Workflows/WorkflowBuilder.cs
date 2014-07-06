using System;
using System.Threading.Tasks;
using MathUtils;

namespace Workflows
{
    public interface IWorkflowBuilder<T> : IEntity where T : IEntity
    {
        Task<IWorkflow<T>> Make();
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
        public WorkflowBuilderPassThrough
            (
                Guid guid, 
                T result
            )
            : base(guid)
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

        public override string EntityName
        {
            get { return "WorkflowBuilder.Passthrough"; }
        }

        public override IEntity GetPart(Guid key)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class WorkflowBuilderBase<T> : IWorkflowBuilder<T> where T : IEntity
    {
        protected WorkflowBuilderBase
            (
                Guid guid
            )
        {
            _guid = guid;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract Task<IWorkflow<T>> Make();

        public abstract string EntityName { get; }
        public abstract IEntity GetPart(Guid key);
    }

}