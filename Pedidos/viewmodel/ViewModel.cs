using Pedidos.connection;
using Pedidos.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Pedidos.viewmodel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Connection con;

        // New
        public ICommand New { get; set; }
        // Save
        public ICommand Save { get; set; }
        // Delete
        public ICommand Delete { get; set; }
        // Update
        public ICommand Update { get; set; }

        // Property change handler
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Constructor
        public ViewModel()
        {

            con = new Connection();
            New = new Command(NewAction);
            Save = new Command(SaveAction);
            Delete = new Command(DeleteAction);
            Update = new Command(UpdateAction);
            orderList = con.GetPedidos();
        }

        private ObservableCollection<Pedido> orderList;
        public ObservableCollection<Pedido> OrderList
        {
            get { return orderList; }
            set
            {
                orderList = value;
                OnPropertyChanged("OrderList");
            }
        }

        private Pedido selectedOrder;
        public Pedido SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged("SelectedOrder");

                if (selectedOrder != null)
                {
                    NPedido = selectedOrder.NPedido;
                    Cliente = selectedOrder.Cliente;
                    DNI = selectedOrder.DNI;
                    Cantidad = selectedOrder.Cantidad;
                    Fecha = selectedOrder.Fecha;

                    OnPropertyChanged(nameof(NPedido));
                    OnPropertyChanged(nameof(Cliente));
                    OnPropertyChanged(nameof(DNI));
                    OnPropertyChanged(nameof(Cantidad));
                    OnPropertyChanged(nameof(Fecha));
                }
            }
        }

        // Model Attributes
        private string nPedido;
        public string NPedido
        {
            get { return nPedido; }
            set
            {
                nPedido = value;
                OnPropertyChanged("NPedido");
            }
        }
        private string cliente;
        public string Cliente
        {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged("Cliente");
            }
        }
        private string dni;
        public string DNI
        {
            get { return dni; }
            set
            {
                dni = value;
                OnPropertyChanged("DNI");
            }
        }
        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set
            {
                cantidad = value;
                OnPropertyChanged("Cantidad");
            }
        }
        private string fecha;
        public string Fecha
        {
            get { return fecha; }
            set
            {
                fecha = value;
                OnPropertyChanged("Fecha");
            }
        }
        private void NewAction(object parameter)
        {
            NPedido = "";
            Cliente = "";
            DNI = "";
            Cantidad = 0;
            Fecha = "";
        }
        private void SaveAction(object parameter)
        {
            Pedido newPedido;
            try
            {
                // Alert if fields are empty
                if (NPedido == "" || Cliente == "" || DNI == "" || Cantidad == 0 || Fecha == "")
                {
                    MessageBox.Show("Please fill all the fields.");
                    return;
                }

                // Alert if the order already exists
                foreach (Pedido order in OrderList)
                {
                    if (order.NPedido == NPedido)
                    {
                        MessageBox.Show("The order already exists.");
                        return;
                    }
                }

                newPedido = new Pedido();

                newPedido.NPedido = NPedido;
                newPedido.Cliente = Cliente;
                newPedido.DNI = DNI;
                newPedido.Cantidad = Cantidad;
                newPedido.Fecha = Fecha;
                
                MessageBox.Show("Record added successfully");
                con.SaveNewPedido(newPedido);

                OrderList.Add(newPedido);
                NPedido = "";
                Cliente = "";
                DNI = "";
                Cantidad = 0;
                Fecha = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteAction(object parameter)
        {
            try
            {
                if (SelectedOrder != null)
                {
                    con.DeletePedido(SelectedOrder.NPedido);
                    OrderList.Remove(SelectedOrder);
                    SelectedOrder = null;
                    NPedido = "";
                    Cliente = "";
                    DNI = "";
                    Cantidad = 0;
                    Fecha = "";
                }
                else
                {
                    MessageBox.Show("Please select an order to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateAction(object parameter)
        {
            try
            {
                if (SelectedOrder != null)
                {
                    Pedido newPedido = new Pedido();

                    newPedido.NPedido = NPedido;
                    newPedido.Cliente = Cliente;
                    newPedido.DNI = DNI;
                    newPedido.Cantidad = Cantidad;
                    newPedido.Fecha = Fecha;

                    con.UpdateExistingPedido(newPedido.NPedido, newPedido.Cliente, newPedido.DNI, newPedido.Cantidad, newPedido.Fecha);

                    OrderList.Remove(SelectedOrder);
                    OrderList.Add(newPedido);
                    SelectedOrder = null;
                    NPedido = "";
                    Cliente = "";
                    DNI = "";
                    Cantidad = 0;
                    Fecha = "";
                }
                else
                {
                    MessageBox.Show("Please select an order to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class StringToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            return null;
        }
    }

}
