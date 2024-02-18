using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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

namespace CS3280GroupAssignment.Items
{
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// 
    /// This window will be instantiated by the main window and must be stored there as a class variable.
    /// Other windows may access ItemDesc data from the db by calling methods from the clsItemsSQL class.
    /// </summary>
    public partial class wndItems : Window
    {
        /// <summary>
        /// Logic object
        /// </summary>
        clsItemsLogic logic;

        /// <summary>
        /// List of all items
        /// </summary>
        public ObservableCollection<Item> items { get; private set; }

        /// <summary>
        /// Boolean to record if updates have been made to the ItemDesc table.
        /// Public accessibility so that other windows can see if updates have been made and respond accordingly.
        /// </summary>
        public bool updatesMade { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public wndItems()
        {
            try
            {
                InitializeComponent();
                // create objects
                logic = new clsItemsLogic();

                // items are an observable collection so that updates to the collection are reflected in the UI
                // TODO: implement an event to update the database whenever updates are made to the datagrid
                items = new ObservableCollection<Item>(logic.GetCurrentItems());
                itemsDataGrid.ItemsSource = items;
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
        /// Event method for when the delete item button is clicked.
        /// Attempts to delete the item currently selected in the datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Item item = itemsDataGrid.SelectedItem as Item;
                if (item == null)
                {
                    return;
                }

                // try to delete item
                if (!logic.DeleteItem(item))
                {
                    // get invoices with the item
                    List<string> invoiceNums = logic.sql.GetInvoiceNumsContainingItem(item.ItemCode);
                    string msg = "Error - that item is currently on the following invoices:";
                    foreach (string num in invoiceNums)
                    {
                        msg += "\n" + num;
                    }
                    tbError.Text = msg;
                    tbError.Visibility = Visibility.Visible;
                }
                else
                {
                    // remove item from the list, UI will be updated because it's an observable collection
                    items.Remove(item);

                    // set updatesMade flag
                    updatesMade = true;

                    tbError.Visibility = Visibility.Collapsed;
                }
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
        /// Event method for when the add item button is clicked.
        /// Opens a new window with functionality to add a new item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndAddItem addItem = new wndAddItem(this.logic);
                addItem.ShowDialog();
                // if new item has been added, update the data grid
                if (logic.newItem != null)
                {
                    // add new item to the observable collection
                    items.Add(logic.newItem);
                    logic.newItem = null;

                    // set updatesMade flag
                    updatesMade = true;
                }
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
        /// Event method for when a cell in the items data grid is edited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    // get current item
                    Item item = itemsDataGrid.SelectedItem as Item;
                    if (item == null)
                    {
                        return;
                    }

                    var column = e.Column as DataGridBoundColumn;
                    if (column.Header.ToString() == "Item Description")
                    {
                        // update description
                        logic.sql.UpdateItemDesc(item.ItemCode, ((TextBox)e.EditingElement).Text);

                        // set updatesMade flag
                        updatesMade = true;
                    }
                    else if (column.Header.ToString() == "Item Cost")
                    {
                        // try to convert new cost
                        decimal newCost;
                        if (!decimal.TryParse(((TextBox)e.EditingElement).Text, out newCost))
                        {
                            return;
                        }

                        // update database
                        logic.UpdateItemCost(item.ItemCode, newCost);

                        // set updatesMade flag
                        updatesMade = true;
                    }
                }
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
        /// OnClosing override method.
        /// Ensures that this window's closing method will simply hide this window instead of closing it.
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
    }
}
