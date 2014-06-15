using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using MathUtils.Rand;

namespace Genomic.Layers
{
    public interface ILayer<TG> where TG : class, IGenomeOld
    {
        int Generation { get; }
        IReadOnlyList<IOrg<TG>> Orgs { get; }
        IOrg<TG> GetOrg(Guid orgId);
    }

    public static class Layer
    {
        public static ILayer<IGenomeOld> CreateSimpleRandomLayer
            (
                int seed,
                uint symbolCount,
                int sequenceLength,
                int orgCount
            )
        {
            var randy = Rando.Fast(seed);

            var genomeBuilders = Enumerable.Range(0, orgCount)
                .Select(i => GenomeBuilderOld.MakeGenerator(
                    symbolCount: symbolCount,
                    sequenceLength: sequenceLength,
                    seed: randy.NextInt(),
                    guid: randy.NextGuid()));

            return Create
                (
                    Enumerable.Range(0, orgCount)
                        .Select(i => GenomeBuilderOld.MakeGenerator(
                            symbolCount: symbolCount,
                            sequenceLength: sequenceLength,
                            seed: randy.NextInt(),
                            guid: randy.NextGuid()))
                );
        }

        public static ILayer<TG> Create<TG>
        (
            IEnumerable<IGenomeBuilderOld<TG>> createFuncs
        ) where TG : class, IGenomeOld
        {
            return Make
            (
                generation: 0,
                orgs: createFuncs
                                .Select(cf=>Org.Make(cf,cf.Make()))
            );
        }

        public static ILayer<TG> Create<TG>
        (
            int seed,
            Func<int, IOrg<TG>> createFunc,
            int orgCount
        ) where TG : class, IGenomeOld
        {
            var randy = Rando.Fast(seed);
            return Make
                (
                    generation: 0,
                    orgs: Enumerable.Range(0, orgCount)
                                    .Select(i => createFunc(randy.NextInt()))
                );
        }

        //public static ILayer<TG> Multiply<TG>
        //(
        //    this ILayer<TG> layer,
        //    Func<TG, int, Guid, TG> genomeReproFunc,
        //    int newGenomeCount,
        //    int seed
        //) where TG : IGenome
        //{
        //    var randy = Rando.Fast(seed);

        //    return Make(
        //            generation: layer.Generation,
        //            orgs: layer.Orgs
        //                     .Repeat().Take(newGenomeCount)
        //                     .Select(g => genomeReproFunc(g, randy.NextInt(), randy.NextGuid()))
        //        );
        //}

        //public static ILayer<TG> MultiplyPreserveParents<TG>
        //    (
        //        this ILayer<TG> layer,
        //        Func<TG, int, Guid, TG> genomeReproFunc,
        //        int newGenomeCount,
        //        int seed
        //    ) where TG : IGenome
        //{
        //    var randy = Rando.Fast(seed);

        //    return Make(
        //            generation: layer.Generation,
        //            orgs: layer.Orgs.Concat
        //            (
        //                layer.Orgs
        //                     .Repeat().Take(newGenomeCount - layer.Orgs.Count)
        //                     .Select(g => genomeReproFunc(g, randy.NextInt(), randy.NextGuid()))
        //            )
        //        );
        //}

        //public static ILayer<TG> NextGen<TG>
        //    (
        //        this ILayer<TG> layer,
        //        int seed,
        //        IReadOnlyList<Tuple<Guid, double>> scores,
        //        int newGenomeCount
        //    ) where TG : IGenome
        //{
        //    return Make(
        //            generation: layer.Generation + 1,
        //            orgs: scores.OrderByDescending(t => t.Item2)
        //                           .Take(newGenomeCount)
        //                           .Select(p => layer.GetOrg(p.Item1))
        //                           .ToList()
        //        );
        //}

        public static ILayer<TG> Make<TG>
            (
                int generation,
                IEnumerable<IOrg<TG>> orgs
            ) where TG : class, IGenomeOld
        {
            return new LayerImpl<TG>
                (
                    generation,
                    orgs
                );
        }

    }

    public class LayerImpl<TG> : ILayer<TG> where TG : class, IGenomeOld
    {
        private readonly int _generation;
        private readonly IReadOnlyDictionary<Guid, IOrg<TG>> _orgs;

        public LayerImpl(int generation, IEnumerable<IOrg<TG>> orgs)
        {
            _generation = generation;
            _orgs = orgs.ToDictionary(t => t.Guid);
        }

        public int Generation
        {
            get { return _generation; }
        }

        private List<IOrg<TG>> _genomesList;
        public IReadOnlyList<IOrg<TG>> Orgs
        {
            get { return _genomesList ?? (_genomesList = new List<IOrg<TG>>(_orgs.Values)); }
        }

        public IOrg<TG> GetOrg(Guid orgId)
        {
            return _orgs.ContainsKey(orgId) ? _orgs[orgId] : default(IOrg<TG>);
        }
    }


    //        +	    //public class WorkflowBuilderImpl
    //+	    //{
    //+	    //    private Func<IGenome, IRando, IPhenotype> _phenotyper;
    //+	    //    public Func<IGenome, IRando, IPhenotype> Phenotyper
    //+	    //    {
    //+	    //        get { return _phenotyper; }
    //+	    //    }
    //+	
    //+	    //    private Func<IPhenotype, IRando, IPhenotypeEval> _phenotypeEvaluator;
    //+	    //    public Func<IPhenotype, IRando, IPhenotypeEval> PhenotypeEvaluator
    //+	    //    {
    //+	    //        get { return _phenotypeEvaluator; }
    //+	    //    }
    //+	
    //+	    //    private Func<IReadOnlyDictionary<Guid, IReadOnlyDictionary<Guid, IOrg>>, IRando, IPhenotypeEval> _nextLayerBuilder;
    //+	    //    public Func<IReadOnlyDictionary<Guid, IReadOnlyDictionary<Guid, IOrg>>, IRando, IPhenotypeEval> NextLayerBuilder
    //+	    //    {
    //+	    //        get { return _nextLayerBuilder; }
    //+	    //    }
    //+	
    //+	    //    private readonly IReadOnlyDictionary<Guid, IOrg> _orgs;
    //+	    //    public IReadOnlyDictionary<Guid, IOrg> Orgs
    //+	    //    {
    //+	    //        get { return _orgs; }
    //+	    //    }
    //+	
    //+	    //    private IReadOnlyDictionary<Guid, IPhenotype> _phenotypes;
    //+	    //    public IReadOnlyDictionary<Guid, IPhenotype> Phenotypes
    //+	    //    {
    //+	    //        get { return _phenotypes; }
    //+	    //    }
    //+	
    //+	    //    private IReadOnlyDictionary<Guid, IPhenotypeEval> _phenotypeEvals;
    //+	    //    public IReadOnlyDictionary<Guid, IPhenotypeEval> PhenotypeEvals
    //+	    //    {
    //+	    //        get { return _phenotypeEvals; }
    //+	    //    }
    //+	    //}
}
