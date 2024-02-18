using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CS3280GroupAssignment.Items
{
    public class clsItemsLogic
    {
        /// <summary>
        /// sql object
        /// </summary>
        public clsItemsSQL sql;

        
        /// <summary>
        /// Item object to keep track of the new item added, if there is one.
        /// Marked as nullable because when there is no new item, it will be set to null.
        /// </summary>
        public Item? newItem { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public clsItemsLogic() 
        {
            sql = new clsItemsSQL();
            newItem = null;
        }

        /// <summary>
        /// Gets the entire list of items from the database using the sql object
        /// </summary>
        /// <returns>List of all items</returns>
        public List<Item> GetCurrentItems()
        {
            try
            {
                return sql.GetAllItems();
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Performs the operation to delete an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if the item was deleted successfully, false otherwise</returns>
        public bool DeleteItem(Item item)
        {
            try
            {
                if (sql.IsItemOnInvoice(item.ItemCode))
                {
                    // can't delete
                    return false;
                }

                sql.DeleteItem(item.ItemCode);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Performs the logic to add a new item.
        /// Checks to make sure all new item arguments are valid, then adds the item to the database.
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemDesc"></param>
        /// <param name="itemCost"></param>
        /// <returns>true if the item was added, false if otherwise</returns>
        public bool AddItem(string itemCode, string itemDesc, string itemCost) 
        {
            try
            {
                // make sure args have content
                if (itemCode.Equals(string.Empty) || itemDesc.Equals(string.Empty) || itemCost.Equals(string.Empty))
                {
                    return false;
                }

                // make sure item code is unique
                foreach (Item item in sql.GetAllItems())
                {
                    if (item.ItemCode == itemCode)
                    {
                        return false;
                    }
                }

                decimal newCost;
                if (!Decimal.TryParse(itemCost, out newCost))
                {
                    return false;
                }

                // add item to db
                sql.InsertNewItem(itemCode, itemDesc, newCost);
                newItem = new Item(itemCode, itemDesc, newCost);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Updates the cost of the given item in the database.
        /// Also updates the TotalCost of each invoice that contains the item.
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="newCost"></param>
        public void UpdateItemCost(string itemCode, decimal newCost)
        {
            try
            {
                // update item cost
                sql.UpdateItemCost(itemCode, newCost);
                // loop thru invoices that contain item, update their total cost
                foreach (string invoice in sql.GetInvoiceNumsContainingItem(itemCode))
                {
                    decimal newTotal = sql.GetSumLineItemsCost(invoice);
                    sql.UpdateInvoiceTotalCost(invoice, newTotal);
                }
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }
    }
}
