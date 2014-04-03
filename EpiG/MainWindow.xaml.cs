using SorterControls.ViewModel;
using SorterControls.ViewModel.Design;

namespace EpiG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MakeRandomSortersVm();
        }
    }
}
