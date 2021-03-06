﻿using System.Collections.Generic;
using SorterGenome.CompPool;
using Sorting.Evals;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SorterPoolVm : ViewModelBase
    {
        public SorterPoolVm
            (
                int keyCount,
                IEnumerable<ISorterEval> sorterEvals,
                int displaySize,
                bool showStages,
                bool showUnused,
                int generation,
                int sorterDisplayCount,
                SorterCompPoolStageType sorterCompPoolStageType, 
                string name
            )
        {
            _generation = generation;
            _sorterCompPoolStageType = sorterCompPoolStageType;
            _name = name;

            SorterGalleryVm = new SorterGalleryVm
                (
                    keyCount: keyCount,
                    sorterEvals: sorterEvals,
                    displaySize: displaySize,
                    showStages: showStages,
                    showUnused: showUnused,
                    sorterDisplayCount: sorterDisplayCount
                );
        }

        private int _generation;
        public int Generation
        {
            get { return _generation; }
            set
            {
                _generation = value;
                OnPropertyChanged("Generation");
            }
        }

        private SorterCompPoolStageType _sorterCompPoolStageType;
        public SorterCompPoolStageType SorterCompPoolStageType
        {
            get { return _sorterCompPoolStageType; }
            set
            {
                _sorterCompPoolStageType = value;
                OnPropertyChanged("SorterCompPoolStageType");
            }
        }

        private SorterGalleryVm _sorterGalleryVm;
        public SorterGalleryVm SorterGalleryVm
        {
            get { return _sorterGalleryVm; }
            set
            {
                _sorterGalleryVm = value;
                OnPropertyChanged("SorterGalleryVm");
            }
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }
    }
}
