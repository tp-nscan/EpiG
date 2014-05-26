using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genomic.Genomes;
using MathUtils.Rand;

namespace Genomic.Layers
{
    public interface IWorkflow
    {
        Func<IGenome, IRando, IPhenotype> Phenotyper { get; }
        Func<IPhenotype, IRando, IPhenotypeEval> PhenotypeEvaluator { get; }
        Func<IReadOnlyDictionary<Guid, IReadOnlyDictionary<Guid, IOrg>>, IRando, IPhenotypeEval> NextLayerBuilder { get; }

        IReadOnlyDictionary<Guid, IOrg> Orgs { get; }
        IReadOnlyDictionary<Guid, IPhenotype> Phenotypes { get; }
        IReadOnlyDictionary<Guid, IPhenotypeEval> PhenotypeEvals { get; }

    }

    public static class Workflow
    {
    }

    public class WorkflowImpl : IWorkflow
    {
        public WorkflowImpl(IReadOnlyDictionary<Guid, IOrg> orgs)
        {
            _orgs = orgs;
        }

        private Func<IGenome, IRando, IPhenotype> _phenotyper;
        public Func<IGenome, IRando, IPhenotype> Phenotyper
        {
            get { return _phenotyper; }
        }

        private Func<IPhenotype, IRando, IPhenotypeEval> _phenotypeEvaluator;
        public Func<IPhenotype, IRando, IPhenotypeEval> PhenotypeEvaluator
        {
            get { return _phenotypeEvaluator; }
        }

        private Func<IReadOnlyDictionary<Guid, IReadOnlyDictionary<Guid, IOrg>>, IRando, IPhenotypeEval> _nextLayerBuilder;
        public Func<IReadOnlyDictionary<Guid, IReadOnlyDictionary<Guid, IOrg>>, IRando, IPhenotypeEval> NextLayerBuilder
        {
            get { return _nextLayerBuilder; }
        }

        private readonly IReadOnlyDictionary<Guid, IOrg> _orgs;
        public IReadOnlyDictionary<Guid, IOrg> Orgs
        {
            get { return _orgs; }
        }

        private IReadOnlyDictionary<Guid, IPhenotype> _phenotypes;
        public IReadOnlyDictionary<Guid, IPhenotype> Phenotypes
        {
            get { return _phenotypes; }
        }

        private IReadOnlyDictionary<Guid, IPhenotypeEval> _phenotypeEvals;
        public IReadOnlyDictionary<Guid, IPhenotypeEval> PhenotypeEvals
        {
            get { return _phenotypeEvals; }
        }
    }
}
