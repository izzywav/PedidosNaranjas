using System.Windows;
using System.Windows.Controls;

namespace Pedidos.view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NewOrderWindow? newOrderWindow;
        private ModifyDeleteOrderWindow? modifyDeleteOrderWindow;
        public bool WindowOpened { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            WindowOpened = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            switch (btn!.Name)
            {
                case "btnNew":
                    if (!WindowOpened)
                    {
                        newOrderWindow = new NewOrderWindow(this);
                        newOrderWindow.Show();
                        WindowOpened = true;
                    }
                    break;
                case "btnModifyDelete":
                    if (!WindowOpened)
                    {
                        modifyDeleteOrderWindow = new ModifyDeleteOrderWindow(this);
                        modifyDeleteOrderWindow.Show();
                        WindowOpened = true;
                    }
                    break;
            }
        }
    }
}
