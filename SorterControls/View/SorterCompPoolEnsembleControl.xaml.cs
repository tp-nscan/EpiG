using System.Windows.Controls;
using System.Windows.Input;

namespace SorterControls.View
{
    /// <summary>
    /// Interaction logic for StagedSorterCompPoolControl.xaml
    /// </summary>
    public partial class SorterCompPoolEnsembleControl
    {
        public SorterCompPoolEnsembleControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGrid.SelectAllCells();
            dataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dataGrid);
            dataGrid.UnselectAllCells();
        }
    }
}
