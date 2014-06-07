namespace Utils
{
    public interface IRandomWalk<T>
    {
        T Step(int seed);
    }
}
