using System;
using System.Collections.Generic;
using System.Linq;

namespace MathUtils.Collections
{

    public interface IGuid
    {
        Guid Guid { get; }
    }

    public interface IGuid<out T> : IGuid
    {
        T Item { get; }
    }


    public static class GuidExt
    {
        public static IEnumerable<Guid> NewGuids()
        {
            while (true)
            {
                yield return Guid.NewGuid();
            }
        }

        public static Guid TailToGuid(this IReadOnlyList<int> ints)
        {
            return ints.Reverse().RoundRobin(0).Take(11).ToArray().ToGuid();
        }

        public static Guid ToGuid(this int[] ints)
        {
            return new Guid
                (
                    (uint)ints[0],
                    (ushort)ints[1],
                    (ushort)ints[2],
                    (byte)ints[3],
                    (byte)ints[4],
                    (byte)ints[5],
                    (byte)ints[6],
                    (byte)ints[7],
                    (byte)ints[8],
                    (byte)ints[9],
                    (byte)ints[10]
                );
        }

        public static int ToHash(this Guid guid)
        {
            var aw = guid.ToByteArray();
            var hashRet = BitConverter.ToInt32(aw, 0);
            hashRet += 23 * BitConverter.ToInt32(aw, 4);
            hashRet += 23 * BitConverter.ToInt32(aw, 8);
            hashRet += 23 * BitConverter.ToInt32(aw, 12);
            return hashRet;
        }

        public static Guid Add(this Guid lhs, Guid rhs)
        {
            var leftArray = lhs.ToByteArray();
            var leftInts = Enumerable.Range(0, 4).Select(d => BitConverter.ToUInt32(leftArray, d * 4));

            var rightArray = rhs.ToByteArray();
            var rightInts = Enumerable.Range(0, 4).Select(d => BitConverter.ToUInt32(rightArray, d * 4));

            return new Guid (
                    leftInts.Zip(rightInts, (x, y) => x + y).SelectMany(BitConverter.GetBytes).ToArray()
                );
        }

        public static Guid Add(this Guid lhs, uint rhs)
        {
            var leftArray = lhs.ToByteArray();
            var leftInts = Enumerable.Range(0, 4).Select(d => BitConverter.ToUInt32(leftArray, d * 4));

            var rightInts = Enumerable.Range(0, 4).Select(d => (uint)(rhs * (d + 1)));

            return new Guid(
                    leftInts.Zip(rightInts, (x, y) => x + y).SelectMany(BitConverter.GetBytes).ToArray()
                );
        }

        public static Guid Add(this Guid lhs, int rhs)
        {
            var leftArray = lhs.ToByteArray();
            var leftInts = Enumerable.Range(0, 4).Select(d => BitConverter.ToUInt32(leftArray, d * 4)).ToArray();

            var rightInts = Enumerable.Range(0, 4).Select(d => (uint)(rhs * leftInts[d]));

            return new Guid(
                    leftInts.Zip(rightInts, (x, y) => x + y).SelectMany(BitConverter.GetBytes).ToArray()
                );
        }

        public static Guid Add(this Guid lhs, IEnumerable<int> rhs)
        {
            return rhs.Aggregate(lhs, (current, rh) => current.Add(rh));
        }
    }
}
