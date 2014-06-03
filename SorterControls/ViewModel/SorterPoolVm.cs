using System.Collections.Generic;
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
                int generation
            )
        {
            _generation = generation;

            SorterGalleryVm = new SorterGalleryVm
                (
                    keyCount: keyCount,
                    sorterEvals: sorterEvals,
                    displaySize: displaySize,
                    showStages: showStages,
                    showUnused: showUnused
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

    }
}
