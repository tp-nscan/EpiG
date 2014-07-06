using System;
using System.Collections.Immutable;
using System.Linq;
using MathUtils;
using MathUtils.Collections;
using Utils;

namespace Workflows
{
    public interface IRecursiveWorkflowBuilder<T> : IEntity where T : IEntity
    {
        IWorkflow<T> InitialWorkflow { get; }
        IRecursiveWorkflowBuilder<T> Iterate(int seed);
        T Make(T initial, int seed);
        IImmutableList<int> Seeds { get; }
    }

    public static class RecursiveWorkflowBuilder
    {
        //public static IRecursiveWorkflowBuilder<T> ToRecursiveFunctionWorkflowBuilder<T>
        //    (
        //        this IWorkflow<T> initialWorkflow, 
        //        Func<T, int, T> updateFunc
        //    ) where T : IEntity
        //{
        //    return new RecursiveWorkflowBuilderFunc<T>
        //        (
        //            seeds: ImmutableList<int>.Empty, 
        //            initialWorkflow: initialWorkflow, 
        //            updateFunc: updateFunc
        //        );
        //}

        public static IRecursiveWorkflowBuilder<T> ToRecursiveRndWlkWorkflowBuilder<T>
        (
            this IWorkflow<T> initialWorkflow
        ) where T : IRandomWalk<T>, IEntity
        {
            return new RecursiveWorkflowBuilderRandomWalk<T>
                (
                    seeds: ImmutableList<int>.Empty,
                    initialWorkflow: initialWorkflow
                );
        }

    }

    public abstract class RecursiveWorkflowBuilderBase<T> : IRecursiveWorkflowBuilder<T>
        where T : IEntity
    {
        protected RecursiveWorkflowBuilderBase
            (
                IImmutableList<int> seeds,
                IWorkflow<T> initialWorkflow
            )
        {
            _seeds = seeds;
            _initialWorkflow = initialWorkflow;
            _guid = initialWorkflow.Guid.ToHash()
                                       .ToEnumerable()
                                       .Concat(Seeds)
                                       .ToList()
                                       .TailToGuid();
        }

        private readonly IImmutableList<int> _seeds;
        public IImmutableList<int> Seeds
        {
            get { return _seeds; }
        }

        public abstract T Make(T initial, int seed);

        public abstract IRecursiveWorkflowBuilder<T> Iterate(int seed);

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly IWorkflow<T> _initialWorkflow;
        public IWorkflow<T> InitialWorkflow
        {
            get { return _initialWorkflow; }
        }

        public abstract string EntityName { get; }
        public abstract IEntity GetPart(Guid key);
    }

    //public abstract class RecursiveWorkflowBuilderFunc<T> : RecursiveWorkflowBuilderBase<T>
    //    where T : IEntity
    //{
    //    public RecursiveWorkflowBuilderFunc(
    //            IImmutableList<int> seeds, 
    //            IWorkflow<T> initialWorkflow, 
    //            Func<T, int, T> updateFunc
    //        ) : base(seeds, initialWorkflow)
    //    {
    //        _updateFunc = updateFunc;
    //    }

    //    public override T Make(T initial, int seed)
    //    {
    //        return UpdateFunc(initial, seed);
    //    }

    //    public override IRecursiveWorkflowBuilder<T> Iterate(int seed)
    //    {
    //        return new RecursiveWorkflowBuilderFunc<T>(
    //                seeds: Seeds.Add(seed),
    //                initialWorkflow: InitialWorkflow,
    //                updateFunc: UpdateFunc
    //            );
    //    }
        
    //    private readonly Func<T, int, T> _updateFunc;
    //    public Func<T, int, T> UpdateFunc
    //    {
    //        get { return _updateFunc; }
    //    }
    //}
}
