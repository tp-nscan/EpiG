using System;
using Genomic.Genomes;
using MathUtils;

namespace Genomic.Phenotypes
{
    public interface IPhenotypeBuilder : IEntity
    {
        IPhenotype Make();
        IGenome Genome { get; }
    }

    public abstract class PhenotypeBuilder : IPhenotypeBuilder
    {
        protected PhenotypeBuilder(
            Guid guid,
            IGenome genome
         )
        {
            _genome = genome;
            _guid = guid;
        }

        public abstract IPhenotype Make();

        private readonly IGenome _genome;
        public IGenome Genome
        {
            get { return _genome; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract string EntityName { get; }

        public abstract IEntity GetPart(Guid key);
    }
}
