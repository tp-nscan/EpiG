using System;
using Genomic.Genomes;
using MathUtils.Collections;

namespace Genomic
{
    public interface IOrg<G> : IGuid where G : IGenome
    {
        IOrgBuilder<G> Builder { get; }
        G Genome { get; }
    }

    public class Org<G> : IOrg<G> where G : IGenome
    {
        public Guid Guid { get; private set; }
        public IOrgBuilder<G> Builder { get; private set; }
        public G Genome { get; private set; }
    }

    public interface IOrgBuilder<G> where G : IGenome
    {
        IOrg<G> Make();
    }

    public interface IPhenotypeBuilder<G, P> where G : IGenome
    {
        P Make(G genome);
    }
}
