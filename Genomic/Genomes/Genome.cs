using System;
using System.Collections.Generic;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genomes
{
    public interface IGenome : IGuid
    {
        IReadOnlyList<uint> Sequence { get; }
    }

    public static class Genome
    {
        public static IGenome Make(Guid guid, IReadOnlyList<uint> sequence)
        {
            return new SimpleGenomeImpl
                (
                    guid: guid,
                    sequence: sequence
                );
        }

        public static IGenome Copy
            (
                this IGenome genome,
                IRando randy,
                Func<IReadOnlyList<uint>, IRando, IReadOnlyList<uint>> copyFunc
            )
        {
            return Make(randy.NextGuid(), copyFunc(genome.Sequence, randy));
        }

        //public static IGenome Copy<TC>
        //    (
        //        this IGenome genome,
        //        IRando randy,
        //        double mutationRate,
        //        double insertionRate,
        //        double deletionRate
        //    )
        //{
        //    return new SimpleGenomeImpl
        //        (
        //            guid: randy.NextGuid(),
        //            sequence: sequence
        //        );
        //}
    }

    public class SimpleGenomeImpl : IGenome
    {

        public SimpleGenomeImpl(Guid guid, IReadOnlyList<uint> sequence)
        {
            _guid = guid;
            _sequence = sequence;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly IReadOnlyList<uint> _sequence;
        public IReadOnlyList<uint> Sequence
        {
            get { return _sequence; }
        }
    }
}