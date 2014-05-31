using Genomic.Workflows;

namespace SorterGenome
{
    public interface ISorterCompPool 
        : IRandomWalk<ISorterCompPool>
    {
        int Generation { get; }
    }

    public static class SorterCompPool
    {
        public static ISorterCompPool Make()
        {
            return new SorterCompPoolImpl();
        }
    }

    public class SorterCompPoolImpl : ISorterCompPool
    {
        public SorterCompPoolImpl()
        {
            _generation = 0;
        }

        SorterCompPoolImpl(int generation)
        {
            _generation = generation;
        }

        public ISorterCompPool Step(int seed)
        {
            for (var i = 0; i < 80000000; i++)
            {
                seed = seed ^ i;
            }

            return new SorterCompPoolImpl(Generation + 1);
        }

        private readonly int _generation;
        public int Generation
        {
            get { return _generation; }
        }
    }
}
