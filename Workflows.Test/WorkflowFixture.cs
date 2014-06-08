using System;
using MathUtils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Workflows.Test
{
    [TestClass]
    public class WorkflowFixture
    {
        [TestMethod]
        public void RecursiveWorkflowTest()
        {
            var guid = Guid.NewGuid();
            var item = "intital";

            //var recursiveWorkflowBuilder = item.WrapWithGuid(guid)
            //                                   .ToPassThroughWorkflow(guid)
            //                                   .ToRecursiveFunctionWorkflowBuilder
                //(
                //    updateFunc: (s, i) => (s.Item + "_" + i).WrapWithGuid(Guid.NewGuid())
                //);


            //var recursiveWorkflow = recursiveWorkflowBuilder.
        }
    }
}
