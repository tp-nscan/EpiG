using Sorting.KeyPairs;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SwitchVm : ViewModelBase
    {
        public SwitchVm(IKeyPair @switch)
        {
            _switch = @switch;
        }

        private readonly IKeyPair _switch;
        public IKeyPair Switch
        {
            get { return _switch; }
        }
    }
}
