using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS3280GroupAssignment.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        /// <summary>
        /// clsSearchLogic class for the object
        /// </summary>
        public clsSearchLogic clsSearchLogic;

        /// <summary>
        /// list of all invoices
        /// </summary>
        public ObservableCollection<Invoice> invoices { get; private set; }

        /// <summary>
        /// creates window and gathers data to fill dataGrid
        /// </summary>
        public wndSearch()
        {
            try
            {
                InitializeComponent();
                //intantiate the object
                clsSearchLogic = new clsSearchLogic();
                //fill the combo boxes
                FillComboBoxes();
                //update datagrid
                UpdateDataGrid();
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    MessageBox.Show(ex.Message, "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Something went wrong!", "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                System.Windows.Application.Current.Shutdown(1);
            }

        }

        /// <summary>
        /// updates the DataGrid based upon the information/filters used
        /// </summary>
        public void UpdateDataGrid()
        {
            try
            {
                //get cost if cbCost is not null
                decimal cost = 0;
                if (cbCostFilter.SelectedItem != null)
                {
                    bool success = Decimal.TryParse(cbCostFilter.SelectedItem.ToString(), out cost);
                }

                //gets the other filters from their cb
                string invoiceNum = (string)cbInvoiceFilter.SelectedItem;
                string invoiceDate = (string)cbInvoiceDate.SelectedItem;

                List<Invoice> tempInvoices = clsSearchLogic.GetInvoices(invoiceNum, invoiceDate, cost);

                //update the data grid
                invoices = new ObservableCollection<Invoice>(tempInvoices);
                searchDataGrid.ItemsSource = invoices;
                
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    MessageBox.Show(ex.Message, "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Something went wrong!", "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                System.Windows.Application.Current.Shutdown(1);
            }
        }

        /// <summary>
        /// fills the combo boxes with what is needed
        /// </summary>
        public void FillComboBoxes()
        {
            try
            {
                //gets the distrinct values for the combo boxes
                cbInvoiceFilter.ItemsSource = new ObservableCollection<string>(clsSearchLogic.getDistinctInvoiceNum());
                cbInvoiceDate.ItemsSource = new ObservableCollection<string>(clsSearchLogic.getDistinctDates());
                cbCostFilter.ItemsSource = new ObservableCollection<decimal>(clsSearchLogic.getDistinctCosts());
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    MessageBox.Show(ex.Message, "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Something went wrong!", "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                System.Windows.Application.Current.Shutdown(1);
            }
        }

        /// <summary>
        /// This will clear the filters put on the search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //clear the combo boxes
                cbInvoiceFilter.SelectedItem = null;
                cbInvoiceDate.SelectedItem = null;
                cbCostFilter.SelectedItem = null;

                //update the datagrid
                UpdateDataGrid();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// selects the invoice and returns to the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //here is where we will call to store the invoice
                if (searchDataGrid.SelectedItem != null)
                {
                    //get the selected row
                    var selectedRow = searchDataGrid.SelectedItem;

                    var selectedInvoice = selectedRow as Invoice;

                    clsSearchLogic.SelectedInvoice = selectedInvoice;
                    //goes back to the main screen
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// closes out the wndSearch and shows the main
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //clear the combo boxes
                cbInvoiceFilter.SelectedItem = null;
                cbInvoiceDate.SelectedItem = null;
                cbCostFilter.SelectedItem = null;
                //goes back to the main window
                this.Hide();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// changes the dataGrid based off the filters used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //just update the data grid based off of the filters
                UpdateDataGrid();
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    MessageBox.Show(ex.Message, "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Something went wrong!", "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                System.Windows.Application.Current.Shutdown(1);
            }
        }



        /// <summary>
        /// overriding onclosing method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.Hide();
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    MessageBox.Show(ex.Message, "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Something went wrong!", "CS 3280 Group Assignment", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                System.Windows.Application.Current.Shutdown(1);
            }
        }

        /// <summary>
        /// for error handling
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}
