using System;
using MathUtils.Rand;
using Sorting.Switchables;

namespace Sorting.TestData
{
    public static class Switchables
    {
        static Switchables()
        {
            var randy = Rando.Fast(129);
            switchableSet = randy.ToSwitchableGroup<uint>(randy.NextGuid(), 13, 11);
        }

        static readonly ISwitchableGroup<uint> switchableSet;
        public static ISwitchableGroup<uint> SwitchableSet
        {
            get { return switchableSet; }
        }

    }
}
