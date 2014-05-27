using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Layers;
using Genomic.Workflows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genomic.Test.Layers
{
    [TestClass]
    public class LayerFixture
    {
        [TestMethod]
        public void TestPhenotypeEvalDbl()
        {
            const double lowVal = 1.0;
            const double medVal = 2.0;
            const double hiVal = 3.0;

            var evalList = new List<IPhenotypeEval>();

            evalList.Add(new PhenotypeEvalDbl(null, medVal));
            evalList.Add(new PhenotypeEvalDbl(null, lowVal));
            evalList.Add(new PhenotypeEvalDbl(null, hiVal));

            var sortedList = evalList.OrderBy(e=>e).ToList();

            Assert.AreEqual(lowVal, sortedList[0]);
            Assert.AreEqual(medVal, sortedList[1]);
            Assert.AreEqual(hiVal, sortedList[2]);
        }

        [TestMethod]
        public void TestWorkflow()
        {
            const int seed = 123;
            var initialGuid = Guid.NewGuid();
            var nextGuid = Guid.NewGuid();

            const string workflowType = "Passthrough";
            const int initialValue = 42;

            var workflow = WorkflowBuilder.MakePassthrough(
                    guid: initialGuid,
                    workflowBuilderType: workflowType,
                    seed: seed,
                    result: initialValue
                ).Make();


            var rwb = RecursiveWorkflowBuilder.Make
                (
                    workflowBuilderType: "Test Recursive",
                    seed: seed,
                    initialWorkflow: workflow,
                    updateFunc: (i, j) => i + 1,
                    guid: nextGuid
                );

            var result = rwb.Make();
        }
    }
}
