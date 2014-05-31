using System;

namespace MathUtils.Rand
{
    class RandoReg : IRando
    {
        private readonly Random _random;

        public RandoReg(int seed)
        {
            _seed = seed;
            _random = new Random(seed);
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        public uint NextUint()
        {
            throw new NotImplementedException();
        }

        public int NextInt(int maxVal)
        {
            return _random.Next(maxVal);
        }

        public double NextDouble()
        {
            _useCount++;
            return _random.NextDouble();
        }
        public int NextInt()
        {
            _useCount++;
            return _random.Next();
        }

        private long _useCount;
        public long UseCount
        {
            get { return _useCount; }
        }

        public IRando Rando { get { return this; } }
    }
}