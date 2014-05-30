using System;
using System.Collections.Immutable;
using MathUtils.Collections;

namespace Genomic.Workflows
{
    public interface IRecursiveWorkflowBuilder<T> : IGuid
    {
        IImmutableList<int> Seeds { get; }
        T Make(T initial, int seed);
        IRecursiveWorkflowBuilder<T> Iterate(int seed);
    }

    public static class RecursiveWorkflowBuilder
    {
        public static IRecursiveWorkflowBuilder<T> ToRecursiveWorkflowBuilder<T>(
                this Func<T, int, T> updateFunc,
                IWorkflow<T> initialWorkflow, 
                int seed
            )
        {
            return new RecursiveWorkflowBuilderFunc<T>
                (
                    seeds: ImmutableList<int>.Empty.Add(seed), 
                    initialWorkflow: initialWorkflow, 
                    updateFunc: updateFunc
                );
        }

    }

    public abstract class RecursiveWorkflowBuilderBase<T> : IRecursiveWorkflowBuilder<T>
    {
        protected RecursiveWorkflowBuilderBase
            (
                IImmutableList<int> seeds,
                IWorkflow<T> initialWorkflow
            )
        {
            _seeds = seeds;
            _initialWorkflow = initialWorkflow;
            _guid = Seeds.FromTail();
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
    }

    public class RecursiveWorkflowBuilderFunc<T> : RecursiveWorkflowBuilderBase<T>
    {
        public RecursiveWorkflowBuilderFunc(
                IImmutableList<int> seeds, 
                IWorkflow<T> initialWorkflow, 
                Func<T, int, T> updateFunc
            ) : base(seeds, initialWorkflow)
        {
            _updateFunc = updateFunc;
        }

        public override T Make(T initial, int seed)
        {
            return UpdateFunc(initial, seed);
        }

        public override IRecursiveWorkflowBuilder<T> Iterate(int seed)
        {
            return new RecursiveWorkflowBuilderFunc<T>(
                seeds: Seeds.Add(seed),
                initialWorkflow: InitialWorkflow,
                updateFunc: UpdateFunc
                );
        }
        
        private readonly Func<T, int, T> _updateFunc;
        public Func<T, int, T> UpdateFunc
        {
            get { return _updateFunc; }
        }
    }

    public class RecursiveWorkflowBuilderRandomWalk<T> : RecursiveWorkflowBuilderBase<T> where T : IRandomWalk<T>
    {
        public RecursiveWorkflowBuilderRandomWalk(
                IImmutableList<int> seeds,
                IWorkflow<T> initialWorkflow
            )
            : base(seeds, initialWorkflow)
        {
        }

        public override T Make(T initial, int seed)
        {
            return initial.Step(seed);
        }

        public override IRecursiveWorkflowBuilder<T> Iterate(int seed)
        {
            return new RecursiveWorkflowBuilderRandomWalk<T>(
                seeds: Seeds.Add(seed),
                initialWorkflow: InitialWorkflow
                );
        }
    }

}
