using System;
using System.Collections.Generic;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using MathUtils.Collections;

namespace Genomic.Layers
{
    public interface IOrg : IGuid
    {
        
    }

    public interface IOrg<G> : IOrg where G : class, IGenome
    {
        IGenomeBuilder<G> Builder { get; }
        G Genome { get; }
    }

    public static class Org
    {
        public static IOrg<G> ToOrg<G>(this IGenomeBuilder<G> builder) 
            where G : class, IGenome
        {
            return new OrgImpl<G>
            (
                builder: builder,
                genome: null
            );
        }

        public static IOrg<G> Make<G>
        (
            IGenomeBuilder<G> builder,
            G genome
        ) where G : class, IGenome
        {
            return new OrgImpl<G>
            (
                builder: builder,
                genome: genome
            );
        }
    }

    public class OrgImpl<G> : IOrg<G> where G : class,  IGenome
    {
        public OrgImpl
            (
                IGenomeBuilder<G> builder, 
                G genome
            )
        {
            _builder = builder;
            _genome = genome;
        }

        public Guid Guid
        {
            get { return Builder.Guid; }
        }

        private readonly IGenomeBuilder<G> _builder;
        public IGenomeBuilder<G> Builder
        {
            get { return _builder; }
        }

        private G _genome;
        public G Genome
        {
            get { return _genome ?? (_genome = Builder.Make()); }
        }
    }

    public interface IPhenotypeBuilder<G, P> where G : class, IGenome
    {
        P Make(G genome);
    }

    public interface IPopulationBuilder<G> where G : class, IGenome
    {
        IEnumerable<IOrg<G>> MakeOrgs();
    }



}
