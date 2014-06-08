﻿using System;
using MathUtils.Collections;
using Utils;

namespace Workflows
{
    public interface IRecursiveWorkflow<T> : IGuid where T : IGuid, IGuidParts
    {
        IRecursiveWorkflowBuilder<T> RecursiveWorkflowBuilder { get; }
        T Result { get; }
    }


    public static class RecursiveWorkflow
    {
        public static IRecursiveWorkflow<T> ToRecursiveWorkflowRndWlk<T>(
            this IWorkflow<T> initialWorkflow
            ) where T : IRandomWalk<T>, IGuid, IGuidParts
        {
            return new RecursiveWorkflowImpl<T>(
                result: initialWorkflow.Result,
                recursiveWorkflowBuilder: initialWorkflow.ToRecursiveRandomWalkWorkflowBuilder()
                );
        }

        public static IRecursiveWorkflow<T> ToRecursiveWorkflow<T>(
                this IWorkflow<T> initialWorkflow,
                Func<T, int, T> updateFunc
        ) where T : IGuid, IGuidParts
        {
            return new RecursiveWorkflowImpl<T>(
                result: initialWorkflow.Result,
                recursiveWorkflowBuilder: initialWorkflow.ToRecursiveFunctionWorkflowBuilder(updateFunc)
                );
        }

        public static IRecursiveWorkflow<T> Update<T>(
                this IRecursiveWorkflow<T> precursor,
                int seed
            ) where T : IGuid, IGuidParts
        {
            return new RecursiveWorkflowImpl<T>(
                result: precursor.RecursiveWorkflowBuilder.Make(precursor.Result, seed),
                recursiveWorkflowBuilder: precursor.RecursiveWorkflowBuilder.Iterate(seed));
        }

    }


    public class RecursiveWorkflowImpl<T> : IRecursiveWorkflow<T> where T : IGuid, IGuidParts
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
}
