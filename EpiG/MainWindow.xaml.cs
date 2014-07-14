using System;
using System.Linq;
using EpiG.Data;

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
            //var context = new EpiGContext();
            //context.Sorters.Add(new Sorter() { Created = DateTime.Now });
            //context.SaveChanges();
            //Title = context.Sorters.Count().ToString();
            DataContext = new EpiGAppVm();
        }
    }
}
