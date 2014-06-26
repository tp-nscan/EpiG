using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sorting.Evals;
using Sorting.Json.Sorters;

namespace Sorting.Json.Evals
{
    public class SorterEvalToJson
    {
        public Guid SwitchableGroupGuid { get; set; }

        public List<double> SwitchUseList { get; set; } 

        public SorterToJson SorterToJson { get; set; }

        public int SwitchableGroupCount { get; set; }

        public bool Success { get; set; }

        public int SwitchesUsed { get; set; }
    }

    public static class SorterEvalToJsonExt
    {
        public static SorterEvalToJson ToJsonAdapter(this ISortResult sortResult)
        {
            var sorterEvalToJson = new SorterEvalToJson
            {
                SwitchableGroupGuid = sortResult.SwitchableGroupGuid,
                SorterToJson = sortResult.Sorter.ToJsonAdapter(),
                SwitchableGroupCount = sortResult.SwitchableGroupCount,
                Success = sortResult.Success,
                SwitchesUsed = sortResult.SwitchUseCount,
                SwitchUseList = sortResult.SwitchUseList.ToList()
            };

            return sorterEvalToJson;
        }

        public static string ToJsonString(this ISortResult sortResult)
        {
            return JsonConvert.SerializeObject(sortResult.ToJsonAdapter(), Formatting.None);
        }

        public static ISortResult ToSorterEval(this string serialized)
        {
            return JsonConvert.DeserializeObject<SorterEvalToJson>(serialized)
                              .ToSorterEval();
        }

        public static ISortResult ToSorterEval(this SorterEvalToJson sorterEvalToJson)
        {
            return SortResult.Make
                (
                    sorter: sorterEvalToJson.SorterToJson.ToSorter(),
                    switchableGroupGuid: sorterEvalToJson.SwitchableGroupGuid,
                    switchUseList: sorterEvalToJson.SwitchUseList,
                    success: sorterEvalToJson.Success,
                    switchableGroupCount: sorterEvalToJson.SwitchableGroupCount
                );
        }
    }
}
