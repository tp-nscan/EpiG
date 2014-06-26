using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genomes
{
    public interface IPermutationEncoding
    {
        int Degree { get; }
        int PermutationCount { get; }
    }


    public static class PermutationGenome
    {
        public static IEnumerable<uint> PermutationMutate
            (
                this IEnumerable<uint> sequence, 
                IRando rando, 
                int degree,
                double mutationRate
            )
        {
            return
                sequence.Chunk(degree)
                    .Select(c => Permutation.ToPermutation(c.ToList()).Mutate(rando, mutationRate))
                    .SelectMany(mp => mp.Values)
                    .Cast<uint>();
        }

    }

        //public static IEnumerable<IPermutation> Mutate(this IEnumerable<IPermutation> permutations, IRando rando, double mutationRate)
        //{
        //    for
        //    return new PermuationImpl
        //        (
        //            values: permutations.Values
        //                            .ToList().FisherYatesPartialShuffle(rando, mutationRate)
        //        );
        //}
}
