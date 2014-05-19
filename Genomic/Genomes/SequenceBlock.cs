using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;

namespace Genomic.Genomes
{
    public interface ISequenceBlock<T>
    {
        T Data { get; }
        string AltDataType { get; }
        IReadOnlyList<uint> AltData { get; }
    }

    public static class SequenceBlock
    {
        public static ISequenceBlock<T> ToSequenceBlock<T>(this T data)
        {
            return new SequenceBlockImpl<T>(data:data, altDataType:String.Empty, altData: null);
        }

        public static ISequenceBlock<T> ToSequenceBlock<T>(this IReadOnlyList<uint> altData, string altDataType)
        {
            return new SequenceBlockImpl<T>(data: default(T), altDataType: altDataType, altData: altData);
        }

        public static IReadOnlyList<uint> ToGenomeSubSequence(this ISequenceBlock<uint?> sequenceBlock)
        {
            return sequenceBlock.Data.HasValue ? sequenceBlock.Data.Value.ToEnumerable().ToList() : sequenceBlock.AltData;
        }
    }

    public class SequenceBlockImpl<T> : ISequenceBlock<T>
    {
        private readonly T _data;
        private readonly string _altDataType;
        private readonly IReadOnlyList<uint> _altData;

        public SequenceBlockImpl(T data, string altDataType, IReadOnlyList<uint> altData)
        {
            _data = data;
            _altDataType = altDataType;
            _altData = altData;
        }

        public T Data
        {
            get { return _data; }
        }

        public string AltDataType
        {
            get { return _altDataType; }
        }

        public IReadOnlyList<uint> AltData
        {
            get { return _altData; }
        }
    }

}
