using System;
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

            var recursiveWorkflowBuilder = item.ToPassThroughWorkflow(guid)
                                               .ToRecursiveFunctionWorkflowBuilder
                (
                    updateFunc: (s, i) => s + "_" + i
                );


            //var recursiveWorkflow = recursiveWorkflowBuilder.
        }
    }
}
