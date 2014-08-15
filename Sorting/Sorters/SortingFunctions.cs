using System.Linq;
using Sorting.Evals;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.Sorters
{
    public static class SortingFunctions
    {
        public static ISortResult Sort<T>
            (
                this ISorter sorter,
                ISwitchableGroup<T> switchableGroup
            )
        {
            var switchUseList = Enumerable.Repeat(0.0, sorter.KeyPairCount).ToList();
            var totalSuccess = true;
            var switchSet = KeyPairSwitchSet.Make<T>(switchableGroup.KeyCount);

            foreach (var switchable in switchableGroup.Switchables)
            {
                var current = switchable.Item;
                var sortSuccess = false;

                for (var i = 0; i < sorter.KeyPairCount; i++)
                {
                    if (switchSet.IsSorted(current))
                    {
                        sortSuccess = true;
                        break;
                    }

                    var res = switchSet.SwitchFunction(sorter.KeyPair(i))(current);
                    current = res.Item1;
                    if (res.Item2)
                    {
                        switchUseList[i]++;
                        if (switchSet.IsSorted(current))
                        {
                            sortSuccess = true;
                            break;
                        }
                    }
                }

                totalSuccess &= sortSuccess;
            }

            return SortResult.Make(
                sorter: sorter,
                switchableGroupGuid: switchableGroup.Guid,
                success: totalSuccess,
                switchUseList: switchUseList,
                switchableGroupCount: switchableGroup.SwitchableCount
                );
        }

        public static T Sort<T>
        (
            this ISorter sorter,
            T item
        )
        {
            var switchSet = KeyPairSwitchSet.Make<T>(sorter.KeyCount);

            T result = item;

            for (var i = 0; i < sorter.KeyPairCount; i++)
            {
                if (switchSet.IsSorted(result))
                {
                    break;
                }

                result = switchSet.SwitchFunction(sorter.KeyPair(i))(result).Item1;
            }
            return result;
        }

        public static ISortResult FullTest(this ISorter sorter)
        {
            return sorter.Sort(Switchable.AllSwitchablesForKeyCount(sorter.KeyCount)
                .ToSwitchableGroup(
                guid: SwitchableGroup.GuidOfAllSwitchableGroupsForKeyCount(sorter.KeyCount), 
                keyCount:sorter.KeyCount));
        }

    }
}
