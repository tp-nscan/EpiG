using SorterControls.ViewModel;
using WpfUtils;

namespace EpiG
{
    public class EpiGAppVm : ViewModelBase
    {
        public EpiGAppVm()
        {
            _makeRandomSortersVm = new MakeRandomSortersVm();
            _sorterCompPoolVm = new SorterCompPoolVm();
            _stagedSorterCompPoolVm = new StagedSorterCompPoolVm();
        }

        private readonly MakeRandomSortersVm _makeRandomSortersVm;
        public MakeRandomSortersVm MakeRandomSortersVm
        {
            get { return _makeRandomSortersVm; }
        }

        private readonly SorterCompPoolVm _sorterCompPoolVm;
        public SorterCompPoolVm SorterCompPoolVm
        {
            get { return _sorterCompPoolVm; }
        }


        private readonly StagedSorterCompPoolVm _stagedSorterCompPoolVm;
        public StagedSorterCompPoolVm StagedSorterCompPoolVm
        {
            get { return _stagedSorterCompPoolVm; }
        }
    }
}
