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
            _sorterCompPoolEnsembleVm = new SorterCompPoolEnsembleVm();
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


        private readonly SorterCompPoolEnsembleVm _sorterCompPoolEnsembleVm;
        public SorterCompPoolEnsembleVm SorterCompPoolEnsembleControl
        {
            get { return _sorterCompPoolEnsembleVm; }
        }
    }
}
