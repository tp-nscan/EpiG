namespace Genomic.Genomes
{
    public interface IOrgEval<TG> where TG : class, IGenome
    {
        int Generation { get; }
        IOrg<TG> Org { get; }
        /// <summary>
        /// Low scores are better
        /// </summary>
        double Score { get; }
        bool Success { get; }
    }

    public static class OrgEval
    {
        public static IOrgEval<TG> Make<TG>
        (
            IOrg<TG> genome,
            double score,
            int generation,
            bool success
        ) where TG : class, IGenome
        {
            return new OrgEvalImpl<TG>
                (
                    org: genome,
                    score: score,
                    generation: generation,
                    success: success
                );
        }
    }

    class OrgEvalImpl<TG> : IOrgEval<TG> where TG : class, IGenome
    {
        private readonly double _score;

        public OrgEvalImpl(IOrg<TG> org, double score, int generation, bool success)
        {
            _org = org;
            _score = score;
            _generation = generation;
            _success = success;
        }

        private readonly int _generation;
        public int Generation
        {
            get { return _generation; }
        }

        private readonly IOrg<TG> _org;
        public IOrg<TG> Org
        {
            get { return _org; }
        }

        public double Score
        {
            get { return _score; }
        }

        private readonly bool _success;
        public bool Success
        {
            get { return _success; }
        }
    }
}
