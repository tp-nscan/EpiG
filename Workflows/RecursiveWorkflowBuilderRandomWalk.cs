using System;
using System.Collections.Immutable;
using MathUtils;
using Utils;

namespace Workflows
{
    public class RecursiveWorkflowBuilderRandomWalk<T> : RecursiveWorkflowBuilderBase<T>
        where T : IRandomWalk<T>, IEntity
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

        public override string EntityName
        {
            get { return "RecursiveWorkflowBuilderRandomWalk"; }
        }

        public override IEntity GetPart(Guid key)
        {
            if (InitialWorkflow.GetPart(key) != null)
            {
                return InitialWorkflow.GetPart(key);
            }
            if (this.Guid == key)
            {
                return this;
            }
            return null;
        }
    }
}