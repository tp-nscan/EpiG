using Sorting.KeyPairs;

namespace Sorting.Evals
{
    public interface ISwitchEval
    {
        double UseCount { get; }
        IKeyPair KeyPair { get; }
    }

    public static class SwitchEval 
    {
        public static ISwitchEval Make
            (
                IKeyPair keyPair,
                double useCount
            )
        {
            return new SwitchEvalImpl
                (
                    keyPair: keyPair,
                    useCount: useCount
                );
        }
    }

    public class SwitchEvalImpl : ISwitchEval
    {
        public SwitchEvalImpl
         (
            IKeyPair keyPair,
            double useCount
         )
        {
            _keyPair = keyPair;
            _useCount = useCount;
        }

        private readonly double _useCount;
        public double UseCount
        {
            get { return _useCount; }
        }

        private readonly IKeyPair _keyPair;
        public IKeyPair KeyPair
        {
            get { return _keyPair; }
        }
    }
}
