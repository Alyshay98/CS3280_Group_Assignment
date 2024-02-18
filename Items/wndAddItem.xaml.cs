using System;
using System.Collections.Generic;
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
    /// Interaction logic for wndAddItem.xaml
    /// </summary>
    public partial class wndAddItem : Window
    {
        /// <summary>
        /// logic object
        /// </summary>
        clsItemsLogic logic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logic"></param>
        public wndAddItem(clsItemsLogic logic)
        {
            this.logic = logic;
            InitializeComponent();
        }

        /// <summary>
        /// Event method for when the cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event method for when the save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // attempt to add the item
            if(logic.AddItem(tbCode.Text, tbDesc.Text, tbCost.Text))
            {
                this.Close();
            }else
            {
                this.Height = 230;
                tbError.Text = "Something went wrong. Please make sure the new Item Code is unique and the new Cost is valid.";
                tbError.Visibility = Visibility.Visible;
            }
        }
    }
}
