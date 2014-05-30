namespace Genomic.Workflows
{
    public interface IRandomWalk<T>
    {
        T Step(int seed);
    }
}
