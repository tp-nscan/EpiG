﻿using System;
using System.Threading.Tasks;
using Genomic.Genomes;
using MathUtils.Collections;

namespace Genomic.Phenotypes
{
    public interface IPhenotypeBuilder<T>: IGuid
    {
        Task<IPhenotype<T>> Make();
        IGenome Genome { get; }
        string PhenotypeBuilderType { get; }
    }

    public abstract class PhenotypeBuilder<T> : IPhenotypeBuilder<T>
    {
        protected PhenotypeBuilder(
            Guid guid, 
            string workflowBuilderType,
            IGenome genome
         )
        {
            _phenotypeBuilderType = workflowBuilderType;
            _genome = genome;
            _guid = guid;
        }

        public abstract Task<IPhenotype<T>> Make();

        private readonly IGenome _genome;
        public IGenome Genome
        {
            get { return _genome; }
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
