using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using MathUtils.Collections;

namespace Genomic
{
    public interface IPopulation<G> : IGuid where G : class, IGenome
    {
        IEnumerable<IOrg<G>> Orgs { get; }
        IPopulationBuilder<G> Bulder { get; }
    }

    public static class Population
    {
        public static PopulationImpl<G> Make<G>(this IPopulationBuilder<G> bulder, Guid guid)
            where G : class, IGenome
        {
            var orgs = bulder.MakeOrgs();

            return new PopulationImpl<G>(
                guid: guid,
                orgs: orgs,
                bulder: bulder
                );
        }
    }

    public class PopulationImpl<G> : IPopulation<G> where G : class, IGenome
    {
        public PopulationImpl(Guid guid, IEnumerable<IOrg<G>> orgs, IPopulationBuilder<G> bulder)
        {
            _guid = guid;
            _orgs = orgs.ToList();
            _bulder = bulder;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly IReadOnlyList<IOrg<G>> _orgs;
        public IEnumerable<IOrg<G>> Orgs
        {
            get { return _orgs; }
        }

        private readonly IPopulationBuilder<G> _bulder;
        public IPopulationBuilder<G> Bulder
        {
            get { return _bulder; }
        }
    }

}