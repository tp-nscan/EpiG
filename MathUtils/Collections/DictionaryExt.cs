using System;
using System.Collections.Generic;

namespace MathUtils.Collections
{
    public static class DictionaryExt
    {

        public static void Merge<TK, TV>
        (
            this IDictionary<TK, TV> ableA,
            IReadOnlyDictionary<TK, TV> ableB,
            Func<TV, TV, TV> mergeFunc
        )
        {
            foreach (var key in ableB.Keys)
            {
                ableA[key] = ableA.ContainsKey(key)
                    ? mergeFunc(ableA[key], ableB[key])
                    : ableB[key];
            }
        }

        public static IReadOnlyDictionary<TK, TV> ToDictionaryIgnoreDupes<TK, TV>
            (
                this IEnumerable<TV> items,
                Func<TV, TK> keySelector
            )
        {
            var dictRet = new Dictionary<TK, TV>();
            foreach (var item in items)
            {
                var key = keySelector(item);
                if (! dictRet.ContainsKey(key))
                {
                    dictRet[key] = item;
                }
            }
            return dictRet;
        }

    }
}
