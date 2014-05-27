using MathUtils.Collections;

namespace Genomic.Workflows
{
    public interface IWorkflowBuilder<T> : IGuid 
    {
        IWorkflow<T> Make();
        string WorkflowBuilderType { get; }
        int Seed { get; }
    }

    public static class WorkflowBuilder
    {

    }

    public class WorkflowBuilderImpl
    {

    }

    //public interface IWorkflowBuilder : IGuid
    //{
    //    Func<IGenome, IRando, IPhenotype> Phenotyper { get; }
    //    Func<IPhenotype, IRando, IPhenotypeEval> PhenotypeEvaluator { get; }
    //    Func<IReadOnlyDictionary<Guid, IReadOnlyDictionary<Guid, IOrg>>, IRando, IPhenotypeEval> NextLayerBuilder { get; }

    //    IReadOnlyDictionary<Guid, IOrg> Orgs { get; }
    //    IReadOnlyDictionary<Guid, IPhenotype> Phenotypes { get; }
    //    IReadOnlyDictionary<Guid, IPhenotypeEval> PhenotypeEvals { get; }

    //}

    //public static class WorkflowBuilder
    //{

    //}

    //public class WorkflowBuilderImpl
    //{
    //    private Func<IGenome, IRando, IPhenotype> _phenotyper;
    //    public Func<IGenome, IRando, IPhenotype> Phenotyper
    //    {
    //        get { return _phenotyper; }
    //    }

    //    private Func<IPhenotype, IRando, IPhenotypeEval> _phenotypeEvaluator;
    //    public Func<IPhenotype, IRando, IPhenotypeEval> PhenotypeEvaluator
    //    {
    //        get { return _phenotypeEvaluator; }
    //    }

    //    private Func<IReadOnlyDictionary<Guid, IReadOnlyDictionary<Guid, IOrg>>, IRando, IPhenotypeEval> _nextLayerBuilder;
    //    public Func<IReadOnlyDictionary<Guid, IReadOnlyDictionary<Guid, IOrg>>, IRando, IPhenotypeEval> NextLayerBuilder
    //    {
    //        get { return _nextLayerBuilder; }
    //    }

    //    private readonly IReadOnlyDictionary<Guid, IOrg> _orgs;
    //    public IReadOnlyDictionary<Guid, IOrg> Orgs
    //    {
    //        get { return _orgs; }
    //    }

    //    private IReadOnlyDictionary<Guid, IPhenotype> _phenotypes;
    //    public IReadOnlyDictionary<Guid, IPhenotype> Phenotypes
    //    {
    //        get { return _phenotypes; }
    //    }

    //    private IReadOnlyDictionary<Guid, IPhenotypeEval> _phenotypeEvals;
    //    public IReadOnlyDictionary<Guid, IPhenotypeEval> PhenotypeEvals
    //    {
    //        get { return _phenotypeEvals; }
    //    }
    //}
}