﻿using SorterControls.View;
using Sorting.TestData;

namespace SorterControls.ViewModel.Design
{
    public class DesignSorterVmImpl : SorterVmImpl
    {
        public DesignSorterVmImpl()
            : base
            (
                sorter: Sorters.TestSorter(keyCount, 1234, 50),
                lineBrushes: LineBrushFactory.GradedBlueBrushes(keyCount),
                width: 8
            )
        { }

        private const int keyCount = 16;
    }
}