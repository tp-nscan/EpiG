using System;
using MathUtils;

namespace Genomic.PhenotypeEvals
{
    public interface IPhenotypeEval : IEntity
    {
        IComparable Result { get; }
        IPhenotypeEvalBuilder PhenotypeEvalBuilder { get; }
    }

    public abstract class PhenotypeEvalBase : IPhenotypeEval
    {
        protected PhenotypeEvalBase(
                Guid guid, 
                IPhenotypeEvalBuilder phenotypeEvalBuilder, 
                IComparable result
            )
        {
            _guid = guid;
            _phenotypeEvalBuilder = phenotypeEvalBuilder;
            _result = result;
        }

        public abstract string EntityName { get; }
        public abstract IEntity GetPart(Guid key);

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

// ReSharper disable once InconsistentNaming
        protected readonly IComparable _result;
        public IComparable Result
        {
            get { return _result; }
        }

        private readonly IPhenotypeEvalBuilder _phenotypeEvalBuilder;
        public IPhenotypeEvalBuilder PhenotypeEvalBuilder
        {
            get { return _phenotypeEvalBuilder; }
        }
    }
}
