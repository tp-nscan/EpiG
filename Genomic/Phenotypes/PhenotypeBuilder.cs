using System;
using System.Threading.Tasks;
using Genomic.Layers;
using MathUtils.Collections;

namespace Genomic.Phenotypes
{
    public interface IPhenotypeBuilder<TP, T>: IGuid 
        where TP : IPhenotype<T>, IGuid
    {
        Task<TP> Make();
        IOrg Org { get; }
        string PhenotypeBuilderType { get; }
    }

    public abstract class PhenotypeBuilder<TP, T> : 
        IPhenotypeBuilder<TP, T> where TP : IPhenotype<T>, IGuid
    {
        protected PhenotypeBuilder(
            Guid guid, 
            string workflowBuilderType, 
            IOrg org
         )
        {
            _phenotypeBuilderType = workflowBuilderType;
            _org = org;
            _guid = guid;
        }

        public abstract Task<TP> Make();

        private readonly IOrg _org;
        public IOrg Org
        {
            get { return _org; }
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
