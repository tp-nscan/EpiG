using System.Collections.Immutable;
using Utils;

namespace Workflows
{
    public class RecursiveWorkflowBuilderRandomWalk<T> : RecursiveWorkflowBuilderBase<T> 
        where T : IRandomWalk<T>
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