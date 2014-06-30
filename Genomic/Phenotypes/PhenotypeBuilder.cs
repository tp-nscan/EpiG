using System;
using Genomic.Genomes;
using MathUtils;

namespace Genomic.Phenotypes
{
    public interface IPhenotypeBuilder<T>: IEntity where T : IEntity
    {
        IPhenotype<T> Make();
        IGenome Genome { get; }
    }


    public abstract class PhenotypeBuilderStandard<T> : PhenotypeBuilder<T>
        where T : IEntity
    {
        protected PhenotypeBuilderStandard(
                IGenome genome,
                Guid guid
            )
            : base(guid, genome)
        {
        }

        public abstract Func<IGenome, T> PhenoFunc { get; }
    }

    public abstract class PhenotypeBuilder<T> : IPhenotypeBuilder<T>
        where T : IEntity
    {
        protected PhenotypeBuilder(
            Guid guid,
            IGenome genome
         )
        {
            _genome = genome;
            _guid = guid;
        }

        public abstract IPhenotype<T> Make();

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
