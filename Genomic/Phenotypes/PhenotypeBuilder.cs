using System.Threading.Tasks;
using Genomic.Layers;
using MathUtils.Collections;

namespace Genomic.Phenotypes
{
    public interface IPhenotypeBuilder<TP, T> where TP : IPhenotype<T>, IGuid
    {
        Task<TP> Make();
        IOrg Org { get; }
        string WorkflowBuilderType { get; }
    }

    public abstract class PhenotypeBuilder<TP, T> : IPhenotypeBuilder<TP, T> where TP : IPhenotype<T>, IGuid
    {
        protected PhenotypeBuilder(string workflowBuilderType, IOrg org)
        {
            _workflowBuilderType = workflowBuilderType;
            _org = org;
        }

        public abstract Task<TP> Make();

        private readonly IOrg _org;
        public IOrg Org
        {
            get { return _org; }
        }

        private readonly string _workflowBuilderType;
        public string WorkflowBuilderType
        {
            get { return _workflowBuilderType; }
        }
    }
}
