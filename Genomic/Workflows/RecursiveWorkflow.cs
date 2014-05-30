﻿using System;
using MathUtils.Collections;

namespace Genomic.Workflows
{
    public interface IRecursiveWorkflow<T> : IGuid
    {
        IRecursiveWorkflowBuilder<T> RecursiveWorkflowBuilder { get; }

        T Result { get; }
    }


    public static class RecursiveWorkflow
    {
        public static IRecursiveWorkflow<T> Update<T>(
                this IRecursiveWorkflow<T> precursor, 
                int seed
            )
        {
            return new RecursiveWorkflowImpl<T>(
                result: precursor.RecursiveWorkflowBuilder.Make(precursor.Result, seed),
                recursiveWorkflowBuilder: precursor.RecursiveWorkflowBuilder.Iterate(seed));
        }
    }


    public class RecursiveWorkflowImpl<T> : IRecursiveWorkflow<T>
    {
        private readonly IRecursiveWorkflowBuilder<T> _recursiveWorkflowBuilder;
        private readonly T _result;

        public RecursiveWorkflowImpl(T result, IRecursiveWorkflowBuilder<T> recursiveWorkflowBuilder)
        {
            _result = result;
            _recursiveWorkflowBuilder = recursiveWorkflowBuilder;
        }

        public Guid Guid
        {
            get { return RecursiveWorkflowBuilder.Guid; }
        }

        public IRecursiveWorkflowBuilder<T> RecursiveWorkflowBuilder
        {
            get { return _recursiveWorkflowBuilder; }
        }

        public T Result
        {
            get { return _result; }
        }
    }

    //public class RecursiveWorkflowRandomWalk<T> : IRecursiveWorkflow<T> where T : IRandomWalk
    //{
    //    private readonly IRecursiveWorkflowBuilder<T> _recursiveWorkflowBuilder;
    //    private readonly T _result;

    //    public RecursiveWorkflowRandomWalk(T result, IRecursiveWorkflowBuilder<T> recursiveWorkflowBuilder)
    //    {
    //        _result = result;
    //        _recursiveWorkflowBuilder = recursiveWorkflowBuilder;
    //    }

    //    public Guid Guid
    //    {
    //        get { return RecursiveWorkflowBuilder.Guid; }
    //    }

    //    public IRecursiveWorkflowBuilder<T> RecursiveWorkflowBuilder
    //    {
    //        get { return _recursiveWorkflowBuilder; }
    //    }

    //    public T Result
    //    {
    //        get { return _result; }
    //    }
    //}
}
