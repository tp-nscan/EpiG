using System;
using System.Linq;
using Genomic.Genomes;
using MathUtils;
using MathUtils.Collections;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome.Phenotypes
{
    public class SorterPhenotypeBuilderComposer : ISorterPhenotypeBuilder
    {
        public SorterPhenotypeBuilderComposer(
            Guid guid, 
            IGenome genome, 
            int keyCount, 
            int skips
         )
        {
            _guid = guid;
            _genome = genome;
            _keyCount = keyCount;
            _skips = skips;
        }

        const int PrefixChunkCount = 6;

        public ISorterPhenotype Make(Guid guid)
        {

            var sorter =
                Genome.Sequence
                      .ToKeyPairs().ToSorter(KeyCount);


            return new SorterPhenotypeStandard(
                    guid: guid,
                    sorter: sorter,
                    sorterPhenotypeBuilder: this
                );



            //var prefixChunks = Genome.Sequence.Take(KeyCount * PrefixChunkCount)
            //    .Select(b=>(int)b)
            //    .Chunk(KeyCount)
            //    .Select(c => new PermuationImpl(c))
            //    .ToList();

            //var chunk0 = prefixChunks[0];
            //var chunk1 = prefixChunks[1];
            //var chunk2 = prefixChunks[2];
            //var chunk3 = prefixChunks[3];
            //var chunk4 = prefixChunks[4];
            //var chunk5 = prefixChunks[5];

            //var compChunk = chunk2;
            //if (Skips == 1)
            //{
            //    compChunk = chunk3;
            //}
            ////if (Skips == 2)
            ////{
            ////    compChunk = chunk4;
            ////}
            ////if (Skips == 3)
            ////{
            ////    compChunk = chunk5;
            ////}

            //var comp1 = chunk0.Compose(compChunk);
            //var comp2 = chunk1.Compose(compChunk);

            //var prefix = chunk0.Values
            //            .Concat(chunk1.Values)
            //            .Concat(comp1.Values)
            //            .Concat(comp2.Values)
            //            .Select(b => (uint)b)
            //            .ToList();

            ////var suffix = Genome.Sequence.Skip(PrefixChunkCount*KeyCount);

            //var suffix = (Skips == 0)
            //    ? Genome.Sequence.Skip(PrefixChunkCount * KeyCount)
            //    : Genome.Sequence.Skip(PrefixChunkCount * KeyCount).Reverse();

            //var sorter = 
            //    prefix.Concat
            //    (
            //        suffix
            //    )
            //    .ToKeyPairs().ToSorter(KeyCount);

   
            //return new SorterPhenotypeStandard(
            //        guid: guid,
            //        sorter: sorter,
            //        sorterPhenotypeBuilder: this
            //    );
        }

        private readonly IGenome _genome;
        public IGenome Genome
        {
            get { return _genome; }
        }

        private readonly int _skips;
        public int Skips
        {
            get { return _skips; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }


        public string EntityName
        {
            get { return "SorterPhenotypeBuilderComposer"; }
        }

        public IEntity GetPart(Guid key)
        {
            if (Guid == key)
            {
                return this;
            }
            return null;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

    }

    public static class OpsEx
    {
        

    }
}
