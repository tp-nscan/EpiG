using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genomes.Builders
{
    public interface IPermutationGenomeBuilderRandom : IGenomeBuilderRandom, IPermutationEncoding
    {
    
    }

    public interface IPermutationGenomeBuilderMutator : IGenomeBuilderRandom, IGenomeBuilderMutator, IPermutationEncoding
    {
    }

    public static class PermutationGenomeBuilder
    {
        public static IEnumerable<IPermutationGenomeBuilderRandom> ToPermutationGenomeBuilders
            (
                this IRando rando,
                int builderCount,
                int degree,
                int permutationCount
            )
        {
            return Enumerable.Range(0, builderCount)
                 .Select
                 (
                     i => new PermutationGenomeBuilderRandomImpl
                        (
                            guid: rando.NextGuid(),
                            degree: degree,
                            permutationCount: permutationCount,
                            seed: rando.NextInt()
                        )
                 );
        }

        public static IPermutationGenomeBuilderMutator ToPermutationMutatorBuilder(
                this IGenome sourceGenome,
                Guid guid,
                int seed,
                int degree,
                double deletionRate,
                double insertionRate,
                double mutationRate
            )
        {
            return new PermutationGenomeBuilderMutatorImpl
                (
                    guid: guid,
                    sourceGenome: sourceGenome,
                    degree: degree,
                    seed: seed,
                    mutationRate: mutationRate,
                    insertionRate: insertionRate,
                    deletionRate: deletionRate
                );
        }
    }

    public class PermutationGenomeBuilderRandomImpl : IPermutationGenomeBuilderRandom
    {
        public PermutationGenomeBuilderRandomImpl(
            Guid guid, 
            int degree, 
            int permutationCount, 
            int seed
            )
        {
            _guid = guid;
            _degree = degree;
            _permutationCount = permutationCount;
            _seed = seed;
        }

        private readonly int _degree;
        public int Degree
        {
            get { return _degree; }
        }

        private readonly int _permutationCount;
        public int PermutationCount
        {
            get { return _permutationCount; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IGenome Make()
        {
            return Genome.Make(
                    sequence: Rando.Fast(Seed).ToPermutations(Degree)
                                    .Take(PermutationCount)
                                    .SelectMany(p => p.Values.Cast<uint>())
                                    .ToList(),
                    genomeBuilder: this
                );
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        public string EntityName
        {
            get { return "PermutationGenomeBuilderRandom"; }
        }

        public IEntity GetPart(Guid key)
        {
            return null;
        }
    }


    public class PermutationGenomeBuilderMutatorImpl : IPermutationGenomeBuilderMutator
    {
        public PermutationGenomeBuilderMutatorImpl
            (
                IGenome sourceGenome,
                Guid guid,
                int seed,
                int degree,
                double deletionRate,
                double insertionRate,
                double mutationRate
            )
        {
            _guid = guid;
            _seed = seed;
            _mutationRate = mutationRate;
            _sourceGenome = sourceGenome;
            _degree = degree;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IGenome Make()
        {
            var randy = Rando.Fast(Seed);
            return Genome.Make
                (
                    sequence: SourceGenome
                    .Sequence
                    .ToPermutations(Degree)
                    .ToList()
                    .MutateInsertDelete
                        (
                            doMutation: randy.ToBoolEnumerator(1.0),
                            doInsertion: randy.ToBoolEnumerator(InsertionRate),
                            doDeletion: randy.ToBoolEnumerator(DeletionRate),
                            mutator: p => p.Mutate(randy, MutationRate),
                            inserter: x => randy.ToPermutations(Degree).First(),
                            paddingFunc: x => randy.ToPermutations(Degree).First()
                        )
                    .SelectMany(p => p.Values.Cast<uint>())
                    .ToList(),
                    genomeBuilder: this
                );
        }

        public string GenomeBuilderType
        {
            get { return "PermutationGenomeBuilderMutator"; }
        }

        private readonly double _deletionRate;
        public double DeletionRate
        {
            get { return _deletionRate; }
        }

        private readonly double _insertionRate;
        public double InsertionRate
        {
            get { return _insertionRate; }
        }

        private readonly double _mutationRate;
        public double MutationRate
        {
            get { return _mutationRate; }
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private readonly IGenome _sourceGenome;
        public IGenome SourceGenome
        {
            get { return _sourceGenome; }
        }

        private readonly int _degree;
        public int Degree
        {
            get { return _degree; }
        }

        public int PermutationCount
        {
            get { return _sourceGenome.SequenceLength/Degree; }
        }

        public string EntityName
        {
            get { return "PermutationGenomeBuilderMutator"; }
        }

        public IEntity GetPart(Guid key)
        {
            return SourceGenome.Guid == key ? SourceGenome : null;
        }
    }

}
