//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Genomic.Genomes.Parsers;
//using MathUtils.Collections;
//using MathUtils.Rand;

//namespace Genomic.Genomes.Builders
//{
//    public interface IPermutationGenomeBuilderOld : IGenomeBuilderOld<IGenome>
//    {
//        int Degree { get; }
//        int TargetPermutationCount { get; }
//    }

//    public static class PermutationGenomeBuilder
//    {
//        public static IPermutationGenomeBuilderOld MakeRandom(
//                Guid guid,
//                int seed,
//                int degree,
//                int targetPermutationCount
//            )
//        {

//            return new PermutationGenomeBuilderOldRandom
//                (
//                    guid: guid,
//                    seed: seed,
//                    degree: degree,
//                    targetPermutationCount: targetPermutationCount
//                );
//        }


//        public static IPermutationGenomeBuilderOld MakeMutator(
//                Guid guid,
//                int seed,
//                int degree,
//                IGenome sourceGenomeOld,
//                double mutationRate
//            )
//        {
//            return new PermutationGenomeBuilderOldMutator
//                (
//                    guid: guid,
//                    seed: seed,
//                    permutationGenomeParser: PermutationGenomeParser.Make(degree),
//                    sourceGenomeOld: sourceGenomeOld,
//                    mutationRate: mutationRate
//                );
//        }

//    }


//    public class PermutationGenomeBuilderOldRandom : IPermutationGenomeBuilderOld
//    {
//        public PermutationGenomeBuilderOldRandom
//            (
//                Guid guid, 
//                int seed,
//                int degree,
//                int targetPermutationCount
//            )
//        {
//            _guid = guid;
//            _seed = seed;
//            _degree = degree;
//            _targetPermutationCount = targetPermutationCount;
//        }

//        private readonly Guid _guid;
//        public Guid Guid
//        {
//            get { return _guid; }
//        }

//        public IGenome Make()
//        {
//            IReadOnlyList<uint> initialPermutation = 
//                      Enumerable.Range(0, Degree)
//                                .Select(v => (uint)v)
//                                .ToList();

//            var randy = Rando.Fast(Seed);

//            return Genome.Make
//                (
//                    sequence: Enumerable.Range(0, TargetPermutationCount)
//                    .Select(i => initialPermutation.FisherYatesShuffle(randy))
//                    .SelectMany(b => b)
//                    .ToList(),
//                    genomeBuilder: this
//                );
//        }

//        public GenomeBuilderType GenomeBuilderType
//        {
//            get { return GenomeBuilderType.PermutationRandom; }
//        }

//        private readonly int _seed;
//        public int Seed
//        {
//            get { return _seed; }
//        }

//        private readonly int _degree;
//        public int Degree
//        {
//            get { return _degree; }
//        }

//        private readonly int _targetPermutationCount;
//        public int TargetPermutationCount
//        {
//            get { return _targetPermutationCount; }
//        }

//    }

//    public class PermutationGenomeBuilderOldMutator: IPermutationGenomeBuilderOld
//    {
//        public PermutationGenomeBuilderOldMutator
//         (
//                Guid guid,
//                int seed,
//                IGenome sourceGenomeOld, 
//                IPermutationGenomeParser permutationGenomeParser, 
//                double mutationRate
//         )
//        {
//            _guid = guid;
//            _seed = seed;
//            _sourceGenomeOld = sourceGenomeOld;
//            _permutationGenomeParser = permutationGenomeParser;
//            _mutationRate = mutationRate;
//            _targetPermutationCount = SourceGenomeOld.Sequence.Count/PermutationGenomeParser.Degree;
//        }

//        private readonly Guid _guid;
//        public Guid Guid
//        {
//            get { return _guid; }
//        }

//        public IGenome Make()
//        {
//            var randy = Rando.Fast(Seed);

//            IReadOnlyList<ISequenceBlock<IPermutation>> sbs =
//                PermutationGenomeParser.GetSequenceBlocks(SourceGenomeOld)
//                                       .Select(b => b.Data.Mutate(randy, MutationRate))
//                                       .Select(mb => mb.ToSequenceBlock())
//                                       .ToList();

//            return PermutationGenomeParser.GetSequence(sbs);
//        }

//        public GenomeBuilderType GenomeBuilderType
//        {
//            get { return GenomeBuilderType.PermutationRandom; }
//        }

//        private readonly int _seed;
//        public int Seed
//        {
//            get { return _seed; }
//        }

//        private readonly IPermutationGenomeParser _permutationGenomeParser;
//        public IPermutationGenomeParser PermutationGenomeParser
//        {
//            get { return _permutationGenomeParser; }
//        }

//        private readonly IGenome _sourceGenomeOld;
//        public IGenome SourceGenomeOld
//        {
//            get { return _sourceGenomeOld; }
//        }

//        public int Degree
//        {
//            get { return PermutationGenomeParser.Degree; }
//        }

//        private readonly double _mutationRate;
//        public double MutationRate
//        {
//            get { return _mutationRate; }
//        }

//        private readonly int _targetPermutationCount;
//        public int TargetPermutationCount
//        {
//            get { return _targetPermutationCount; }
//        }
//    }

//}
