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

    public interface IOrg<G> : IOrg where G : class, IGenomeOld
    {
        IGenomeBuilderOld<G> BuilderOld { get; }
        G Genome { get; }
    }

    public static class Org
    {
        public static IOrg<G> ToOrg<G>(this IGenomeBuilderOld<G> builderOld) 
            where G : class, IGenomeOld
        {
            return new OrgImpl<G>
            (
                builderOld: builderOld,
                genome: null
            );
        }

        public static IOrg<G> Make<G>
        (
            IGenomeBuilderOld<G> builderOld,
            G genome
        ) where G : class, IGenomeOld
        {
            return new OrgImpl<G>
            (
                builderOld: builderOld,
                genome: genome
            );
        }
    }

    public class OrgImpl<G> : IOrg<G> where G : class, IGenomeOld
    {
        public OrgImpl
            (
                IGenomeBuilderOld<G> builderOld, 
                G genome
            )
        {
            _builderOld = builderOld;
            _genome = genome;
        }

        public Guid Guid
        {
            get { return BuilderOld.Guid; }
        }

        private readonly IGenomeBuilderOld<G> _builderOld;
        public IGenomeBuilderOld<G> BuilderOld
        {
            get { return _builderOld; }
        }

        private G _genome;
        public G Genome
        {
            get { return _genome ?? (_genome = BuilderOld.Make()); }
        }
    }

    public interface IPhenotypeBuilder<G, P> where G : class, IGenomeOld
    {
        P Make(G genome);
    }

    public interface IPopulationBuilder<G> where G : class, IGenomeOld
    {
        IEnumerable<IOrg<G>> MakeOrgs();
    }



}
