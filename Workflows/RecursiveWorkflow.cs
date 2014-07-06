using System;
using MathUtils;
using Utils;

namespace Workflows
{
    public interface IRecursiveWorkflow<T> : IEntity where T : IEntity
    {
        IRecursiveWorkflowBuilder<T> RecursiveWorkflowBuilder { get; }
        T Result { get; }
    }


    public static class RecursiveWorkflow
    {
        public static IRecursiveWorkflow<T> ToRecursiveWorkflowRndWlk<T>
            (
                this IWorkflow<T> initialWorkflow,
                Guid guid
            ) where T : IRandomWalk<T>, IEntity
        {
            return new RecursiveWorkflowImpl<T>(
                    guid: guid,
                    result: initialWorkflow.Result,
                    recursiveWorkflowBuilder: initialWorkflow.ToRecursiveRndWlkWorkflowBuilder()
                );
        }

        //public static IRecursiveWorkflow<T> ToRecursiveWorkflow<T>(
        //        this IWorkflow<T> initialWorkflow,
        //        Func<T, int, T> updateFunc
        //) where T : IEntity
        //{
        //    return new RecursiveWorkflowImpl<T>(
        //        result: initialWorkflow.Result,
        //        recursiveWorkflowBuilder: initialWorkflow.ToRecursiveFunctionWorkflowBuilder(updateFunc)
        //        );
        //}

        public static IRecursiveWorkflow<T> Update<T>(
                this IRecursiveWorkflow<T> precursor,
                Guid guid,
                int seed
            ) where T : IEntity
        {
            return new RecursiveWorkflowImpl<T>(
                guid: guid,
                result: precursor.RecursiveWorkflowBuilder.Make(precursor.Result, seed),
                recursiveWorkflowBuilder: precursor.RecursiveWorkflowBuilder.Iterate(seed));
        }

    }


    public class RecursiveWorkflowImpl<T> : IRecursiveWorkflow<T> where T : IEntity
    {
        private readonly IRecursiveWorkflowBuilder<T> _recursiveWorkflowBuilder;
        private readonly T _result;
        private readonly Guid _guid;

        public RecursiveWorkflowImpl(Guid guid, T result, IRecursiveWorkflowBuilder<T> recursiveWorkflowBuilder)
        {
            _guid = guid;
            _result = result;
            _recursiveWorkflowBuilder = recursiveWorkflowBuilder;
        }

        public IRecursiveWorkflowBuilder<T> RecursiveWorkflowBuilder
        {
            get { return _recursiveWorkflowBuilder; }
        }

        public T Result
        {
            get { return _result; }
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public string EntityName
        {
            get { return "RecursiveWorkflowImpl"; }
        }

        public IEntity GetPart(Guid key)
        {
            if (Guid == key)
            {
                return this;
            }
            return RecursiveWorkflowBuilder.GetPart(key);
        }
    }
}
