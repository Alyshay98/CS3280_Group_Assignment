using CS3280GroupAssignment.Items;
using CS3280GroupAssignment.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS3280GroupAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        /// <summary>
        /// Search class variable
        /// </summary>
        Search.wndSearch wndSearch;

        /// <summary>
        /// Items Class variable
        /// </summary>
        Items.wndItems wndItems;

        /// <summary>
        /// MainSQL class variable
        /// </summary>
        Main.clsMainSQL clsMainSQL;

        /// <summary>
        /// Invoice variable
        /// </summary>
        Invoice currentInvoice;

        /// <summary>
        /// Boolean flag for currently editing invoice
        /// </summary>
        bool currEditInvoice = false;

        /// <summary>
        /// Boolean flas for adding a new invoice
        /// </summary>
        bool AddNewInvoice = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public wndMain()
        {
            try
            {
                InitializeComponent();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                wndSearch = new Search.wndSearch();
                wndItems = new Items.wndItems();
                clsMainSQL = new Main.clsMainSQL();
                GBItemControl.IsEnabled = false;
                cbItems.ItemsSource = clsMainSQL.GetAllItems();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Click event for the Search Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currEditInvoice || AddNewInvoice)
                {
                    return;
                }
                // update the datagrid
                wndSearch.UpdateDataGrid();
                wndSearch.FillComboBoxes();
                wndSearch.ShowDialog();

                /// set invoicenum TB 
                /// fill out dg with items on invoice
                currentInvoice = wndSearch.clsSearchLogic.SelectedInvoice;
                dgItems.ItemsSource = new ObservableCollection<Item>(clsMainSQL.GetLineItemsContainingInvoiceNum(currentInvoice.InvoiceNum));

                /// set invoice number
                TBInvoice.Text = currentInvoice.InvoiceNum;
                /// set total cost 
                UpdateTotalCostLabel();
                /// set the date
                DpDatePicker.Text = currentInvoice.InvoiceDate;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Click Event for the Items Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currEditInvoice || AddNewInvoice)
                {
                    return;
                }
                wndItems.ShowDialog();
                // if updates have been made
                if (wndItems.updatesMade)
                {
                    // reset items source
                    cbItems.ItemsSource = clsMainSQL.GetAllItems();
                    // fill out dg with items on invoice
                    currentInvoice = wndSearch.clsSearchLogic.SelectedInvoice;
                    dgItems.ItemsSource = new ObservableCollection<Item>(clsMainSQL.GetLineItemsContainingInvoiceNum(currentInvoice.InvoiceNum));
                    // update cost label
                    UpdateTotalCostLabel();
                    wndItems.updatesMade = false;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Adds an invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // set the invoice num txtbox to "TBD"
                TBInvoice.Text = "TBD";

                // enable item control GB
                GBItemControl.IsEnabled = true;
                AddNewInvoice = true;
                // bind dg to a new empty observable collection of items
                dgItems.ItemsSource = new ObservableCollection<Item>();

                // set total cost 
                lblTotalCost.Content = "$0";
                // set the date
                DpDatePicker.Text = null;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Saves an Invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // should only save if the date is not null
                if(DpDatePicker.Text == null)
                {
                    return;
                }

                if (AddNewInvoice)
                {
                    decimal stringCost;
                    stringCost = Decimal.Parse(lblTotalCost.Content.ToString().Substring(1));


                    //call insertnewinvoice SQL method
                    clsMainSQL.InsertNewInvoice(DpDatePicker.Text.ToString(), stringCost);

                    Invoice newInvoice = clsMainSQL.GetNewestInvoice();
                    int counter = 1;
                    //for loop
                     foreach (Item item in dgItems.ItemsSource)
                     {
                        //insertnewlinetitem sql method
                        clsMainSQL.InsertLineItems(newInvoice.InvoiceNum, counter, item.ItemCode);
                        counter++;
                     }

                    TBInvoice.Text = newInvoice.InvoiceNum.ToString();
                    AddNewInvoice = false;
                    // update new invoice
                    currentInvoice = newInvoice;

                    // disable group box again
                    GBItemControl.IsEnabled = false;
                    tbItemCost.Text = null;
                    cbItems.SelectedIndex = -1;
                }
                else if (currEditInvoice)
                {
                    decimal TotalCost = 0;

                    clsMainSQL.DeleteInvoiceLineItems(currentInvoice.InvoiceNum);

                    int counter = 1;
                    //for loop
                    foreach (Item item in dgItems.ItemsSource)
                    {
                        //insertnewlinetitem sql method
                        clsMainSQL.InsertLineItems(currentInvoice.InvoiceNum, counter, item.ItemCode);
                        counter++;
                        TotalCost += item.Cost;
                    }
                    clsMainSQL.UpdateInvoice(TotalCost, DpDatePicker.ToString(), currentInvoice.InvoiceNum);

                    currEditInvoice = false;
                    // disable group box again
                    GBItemControl.IsEnabled = false;
                    tbItemCost.Text = null;
                    cbItems.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Add an Item click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((currEditInvoice || AddNewInvoice) && cbItems.SelectedIndex != -1)
                {
                    ObservableCollection<Item> items = dgItems.ItemsSource as ObservableCollection<Item>;
                    Item item = cbItems.SelectedItem as Item;
                    items.Add(item);

                    UpdateTotalCostLabel();
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            

        }

        /// <summary>
        /// Loops through the items datagrid, calculates the total cost for the current invoice, and displays it
        /// </summary>
        void UpdateTotalCostLabel()
        {
            try
            {
                decimal totalCost = 0;
                foreach (Item item in dgItems.ItemsSource)
                {
                    totalCost += item.Cost;
                }
                lblTotalCost.Content = "$" + totalCost.ToString();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Button to remove an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Item item = dgItems.SelectedItem as Item;
                if(item == null)
                {
                    return;
                }
                ObservableCollection<Item> items = dgItems.ItemsSource as ObservableCollection<Item>;
                items.Remove(item);
                UpdateTotalCostLabel();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Button to edit an invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(AddNewInvoice || currEditInvoice || currentInvoice == null)
                {
                    return;
                }
                // bind cbitems to a new empty observable collection of items
                cbItems.ItemsSource = clsMainSQL.GetAllItems();
                currEditInvoice = true;
                GBItemControl.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event method that handles the displaying of an item cost when the item is selected in the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // change cost in text box
            Item item = cbItems.SelectedItem as Item;
            if (item != null)
            {
                tbItemCost.Text = item.Cost.ToString();
            }
        }

        /// <summary>
        /// exception handler that shows the error
        /// </summary>
        /// <param name="sClass">the class</param>
        /// <param name="sMethod">the method</param>
        /// <param name="sMessage">the error message</param>
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