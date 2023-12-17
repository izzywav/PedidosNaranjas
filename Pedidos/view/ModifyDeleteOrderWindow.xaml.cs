using Pedidos.viewmodel;
using System;
using System.Windows;

namespace Pedidos.view
{
    /// <summary>
    /// Interaction logic for ModifyDeleteOrderWindow.xaml
    /// </summary>
    public partial class ModifyDeleteOrderWindow : Window
    {
        private MainWindow mainWindow;

        public ModifyDeleteOrderWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            // Set the data context
            DataContext = new ViewModel();
        }

        override protected void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            mainWindow.WindowOpened = false;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
