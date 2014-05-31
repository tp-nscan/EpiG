using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;

namespace MathUtils.Rand
{
    public interface IRando
    {
        double NextDouble();
        int NextInt();
        uint NextUint();
        int NextInt(int maxVal);
        int Seed { get; }
        long UseCount { get; }
    }

    public static class Rando
    {
        public static IRando Standard(int seed)
        {
            return new RandoReg(seed);
        }

        public static IRando Fast(int seed)
        {
            return new RandoFast(seed);
        }

        public static IRando Spawn(this IRando rando)
        {
            return new RandoFast(rando.NextInt());
        }

        public static IEnumerable<IRando> ToRandomEnumerator(this IRando rando)
        {
            while (true)
            {
                yield return Fast(rando.NextInt());
            }
        }

        #region int

        public static IEnumerable<int> ToIntEnumerator(this IRando rando, int maxValue)
        {
            while (true)
            {
                yield return rando.NextInt(maxValue);
            }
        }

        public static IEnumerable<int> ToIntEnumerator(this IRando rando)
        {
            while (true)
            {
                yield return rando.NextInt();
            }
        }

        #endregion

        #region uint

        public static uint NextUint(this IRando rando, uint maxVal)
        {
            var mask = Numbers.UintMask(maxVal.HiBit());
            while (true)
            {
                var retVal = rando.NextUint() & mask;
                if (retVal < maxVal)
                {
                    return retVal;
                }
            }
        }


        public static IEnumerable<uint> ToUintEnumerator(this IRando rando)
        {
            while (true)
            {
                yield return rando.NextUint();
            }
        }

        public static IEnumerable<uint> ToUintEnumerator(this IRando rando, uint maxVal)
        {
            var mask = Numbers.UintMask(maxVal.HiBit());
            while (true)
            {
                var retVal = rando.NextUint() & mask;
                if (retVal < maxVal)
                {
                    yield return retVal;
                }
            }
        }

        #endregion

        #region ulong

        public static IEnumerable<ulong> ToUlongEnumerator(this IRando rando)
        {
            while (true)
            {
                var retVal = (ulong) rando.NextUint();
                yield return (retVal << 32) + rando.NextUint();
            }
        }

        public static IEnumerable<ulong> ToUlongEnumerator(this IRando rando, ulong maxVal)
        {
            var mask = Numbers.UlongMask(maxVal.HiBit());
            while (true)
            {
                var retVal = rando.NextUlong() & mask;
                if (retVal < maxVal)
                {
                    yield return retVal;
                }
            }
        }

        public static ulong NextUlong(this IRando rando, ulong maxVal)
        {
            var mask = Numbers.UlongMask(maxVal.HiBit());
            while (true)
            {
                var retVal = rando.NextUlong() & mask;
                if (retVal < maxVal)
                {
                    return retVal;
                }
            }
        }

        public static ulong NextUlong(this IRando rando)
        {
            var mask = (ulong)rando.NextUint();
            return (mask << 32) + rando.NextUint();
        }

        #endregion

        public static IEnumerable<bool> ToBoolEnumerator(this IRando rando, double trueProbability)
        {
            while (true)
            {
                yield return rando.NextDouble() < trueProbability;
            }
        }

        public static IEnumerable<double> ToDoubleEnumerator(this IRando rando)
        {
            while (true)
            {
                yield return rando.NextDouble();
            }
        }

        #region guid

        public static Guid NextGuid(this IRando rando)
        {
            return new Guid
                (
                    (uint)rando.NextInt(),
                    (ushort)rando.NextInt(),
                    (ushort)rando.NextInt(),
                    (byte)rando.NextInt(),
                    (byte)rando.NextInt(),
                    (byte)rando.NextInt(),
                    (byte)rando.NextInt(),
                    (byte)rando.NextInt(),
                    (byte)rando.NextInt(),
                    (byte)rando.NextInt(),
                    (byte)rando.NextInt()
                );
        }




        public static IEnumerable<Guid> ToGuidEnumerator(this IRando rando)
        {
            while (true)
            {
                yield return new Guid
                    (
                        (uint) rando.NextInt(),
                        (ushort) rando.NextInt(),
                        (ushort) rando.NextInt(),
                        (byte) rando.NextInt(),
                        (byte) rando.NextInt(),
                        (byte) rando.NextInt(),
                        (byte) rando.NextInt(),
                        (byte) rando.NextInt(),
                        (byte) rando.NextInt(),
                        (byte) rando.NextInt(),
                        (byte) rando.NextInt()
                    );
            }
        }

        #endregion

        public static IEnumerable<T> Pick<T>(this IRando rando, IReadOnlyList<T> items)
        {
            while (true)
            {
                yield return items[rando.NextInt(items.Count)];
            }
        }

        public static T[] FisherYatesShuffle<T>(this IRando rando, IReadOnlyList<T> items)
        {
            var arrayLength = items.Count;
            var retArray = items.ToArray();
            for (var i = arrayLength - 1; i > 0; i--)
            {
                var j = rando.NextInt(i + 1);
                var temp = retArray[i];
                retArray[i] = retArray[j];
                retArray[j] = temp;
            }
            return retArray;
        }

        public static IEnumerable<double> ExpDist(this IRando rando, double max)
        {
            var logMax = Math.Log(max);
            while (true)
            {
                yield return Math.Exp(rando.NextDouble()*logMax);
            }
        }

        public static IEnumerable<double> PowDist(this IRando rando, double max, double pow)
        {
            while (true)
            {
                yield return Math.Pow(rando.NextDouble(), pow)*max;
            }
        }

    }
}
