using System.Collections.Generic;
using System.Windows.Media;
using Sorting.KeyPairs;
using WpfUtils;

namespace SorterControls.ViewModel
{
    public class SwitchVm : ViewModelBase
    {
        public SwitchVm
        (
            IKeyPair keyPair, 
            int keyCount, 
            List<Brush> lineBrushes
        )
        {
            _keyPair = keyPair;
            _keyCount = keyCount;
            _lineBrushes = lineBrushes;
        }

        private readonly IKeyPair _keyPair;
        public IKeyPair KeyPair
        {
            get { return _keyPair; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private Brush _switchBrush;
        public Brush SwitchBrush
        {
            get { return _switchBrush; }
            set
            {
                _switchBrush = value;
                OnPropertyChanged("SwitchBrush");
            }
        }

        private readonly List<Brush> _lineBrushes;
        public List<Brush> LineBrushes
        {
            get { return _lineBrushes; }
        }
    }
}
