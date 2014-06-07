using System;
using MathUtils.Collections;

namespace Workflows
{
    public interface IWorkflowBuilder<T> : IGuid 
    {
        IWorkflow<T> Make();
        string WorkflowBuilderType { get; }
    }

    public static class WorkflowBuilder
    {
        public static IWorkflowBuilder<T> MakePassthrough<T>
            (
                Guid guid,
                T result
            )
        {
            return new WorkflowBuilderPassThrough<T>
            (
                guid: guid,
                result: result
            );
        }
    }

    public class WorkflowBuilderPassThrough<T> : WorkflowBuilderBase<T>
    {
        public static string Name = "Passthrough";

        public WorkflowBuilderPassThrough
            (
                Guid guid, 
                T result
            )
            : base(guid, Name)
        {
            _result = result;
        }

        private readonly T _result;
        public T Result
        {
            get { return _result; }
        }

        public override IWorkflow<T> Make()
        {
            return new WorkflowImpl<T>(
                    workflowBuilder: this,
                    result: Result
                );
        }
    }

    public abstract class WorkflowBuilderBase<T> : IWorkflowBuilder<T>
    {
        protected WorkflowBuilderBase
            (
                Guid guid, 
                string workflowBuilderType
            )
        {
            _guid = guid;
            _workflowBuilderType = workflowBuilderType;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract IWorkflow<T> Make();

        private readonly string _workflowBuilderType;
        public string WorkflowBuilderType
        {
            get { return _workflowBuilderType; }
        }

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