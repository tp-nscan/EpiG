﻿using System.Windows;

namespace EpiG
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var win = new MainWindow();
            win.DataContext = new EpiGAppVm();
            win.Show();
        }
    }
}
