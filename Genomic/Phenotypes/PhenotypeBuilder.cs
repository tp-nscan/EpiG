using System;
using Genomic.Genomes;
using MathUtils.Collections;

namespace Genomic.Phenotypes
{
    public interface IPhenotypeBuilder<T>: IGuid
    {
        IPhenotype<T> Make();
        IGenome Genome { get; }
        string PhenotypeBuilderType { get; }
    }

    public static class PhenotypeBuilder
    {
        public static IPhenotype<T> ToPhenotype<T>
            (
                this Func<IGenome, T> phenoFunc,
                IGenome genome,
                Guid guid
            )
        {
            return new PhenotypeBuilderStandard<T>
                (
                    guid: guid,
                    genome: genome,
                    phenoFunc: phenoFunc
                ).Make();
        }
    }

    public class PhenotypeBuilderStandard<T> : PhenotypeBuilder<T>
    {
        public PhenotypeBuilderStandard(
                Guid guid,
                IGenome genome,
                Func<IGenome, T> phenoFunc
            )
            : base(guid, "PhenotypeBuilderStandard", genome)
        {
            _phenoFunc = phenoFunc;
        }

        private readonly Func<IGenome, T> _phenoFunc;
        private Func<IGenome, T> PhenoFunc
        {
            get { return _phenoFunc; }
        }

        public override IPhenotype<T> Make()
        {
            return Phenotype.Make(
                value: PhenoFunc(Genome),
                phenotypeBuilder: this
                );
        }
    }

    public abstract class PhenotypeBuilder<T> : IPhenotypeBuilder<T>
    {
        protected PhenotypeBuilder(
            Guid guid, 
            string phenotypeBuilderType,
            IGenome genome
         )
        {
            _phenotypeBuilderType = phenotypeBuilderType;
            _genome = genome;
            _guid = guid;
        }

        public abstract IPhenotype<T> Make();

        private readonly IGenome _genome;
        public IGenome Genome
        {
            get { return _genome; }
        }

        private readonly string _phenotypeBuilderType;
        public string PhenotypeBuilderType
        {
            get { return _phenotypeBuilderType; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }
}
