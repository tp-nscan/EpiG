using SorterControls.ViewModel;
using WpfUtils;

namespace EpiG
{
    public class EpiGAppVm : ViewModelBase
    {
        public EpiGAppVm()
        {
            _makeRandomSortersVm = new MakeRandomSortersVm();
        }

        private MakeRandomSortersVm _makeRandomSortersVm;
        public MakeRandomSortersVm MakeRandomSortersVm
        {
            get { return _makeRandomSortersVm; }
            set { _makeRandomSortersVm = value; }
        }
    }
}
