using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CS3280GroupAssignment.Items
{
    public class clsItemsSQL
    {
        /// <summary>
        /// DataAccess object
        /// </summary>
        private clsDataAccess access;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public clsItemsSQL() 
        {
            access = new clsDataAccess();
        }

        /// <summary>
        /// Gets all objects from the ItemDesc table and return them as an Item list
        /// </summary>
        /// <returns>List of Items</returns>
        public List<Item> GetAllItems()
        {
            try
            {
                int numRows = 0;
                List<Item> retList = new List<Item>();

                // sql statement
                DataSet ds = access.ExecuteSQLStatement("select ItemCode, ItemDesc, Cost from ItemDesc", ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Item item = new Item(dr[0].ToString(), dr[1].ToString(), (decimal)dr[2]);
                    retList.Add(item);
                }
                return retList;
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Gets all InvoiceNum strings for invoices that have the given item
        /// </summary>
        /// <param name="ItemCode">ItemCode for the item</param>
        /// <returns>List of InvoiceNum strings</returns>
        public List<string> GetInvoiceNumsContainingItem(string ItemCode)
        {
            try
            {
                int numRows = 0;
                List<string> retList = new List<string>();

                // sql statement
                DataSet ds = access.ExecuteSQLStatement("select InvoiceNum from LineItems where ItemCode = '" + ItemCode + "'", ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    retList.Add(dr[0].ToString());
                }
                return retList;
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Checks to see if a given item is on any invoice
        /// </summary>
        /// <param name="ItemCode">ItemCode for the item to check</param>
        /// <returns>True if the item is on any invoice, false if the item isn't on any invoices</returns>
        public bool IsItemOnInvoice(string ItemCode)
        {
            try
            {
                int numRows = 0;

                // sql statement
                DataSet ds = access.ExecuteSQLStatement("select distinct(InvoiceNum) from LineItems where ItemCode = '" + ItemCode + "'", ref numRows);
                if (numRows == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Updates the ItemDesc for the given item
        /// </summary>
        /// <param name="ItemCode">ItemCode for the item</param>
        /// <param name="ItemDesc">New ItemDesc</param>
        /// <returns>Number of rows updated</returns>
        public int UpdateItemDesc(string ItemCode, string ItemDesc)
        {
            try
            {
                return access.ExecuteNonQuery("Update ItemDesc Set ItemDesc = '" + ItemDesc + "' where ItemCode = '" + ItemCode + "'");
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Updates the Cost for the given item
        /// </summary>
        /// <param name="ItemCode">ItemCode for the item</param>
        /// <param name="Cost">New Cost</param>
        /// <returns>Number of rows updated</returns>
        public int UpdateItemCost(string ItemCode, decimal Cost)
        {
            try
            {
                return access.ExecuteNonQuery("Update ItemDesc Set Cost = " + Cost.ToString() + " where ItemCode = '" + ItemCode + "'");
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Inserts a new Item into the ItemDesc table
        /// </summary>
        /// <param name="ItemCode">New ItemCode for the item</param>
        /// <param name="ItemDesc">New ItemDesc</param>
        /// <param name="Cost">New Cost</param>
        /// <returns>Number of rows updated</returns>
        public int InsertNewItem(string ItemCode, string ItemDesc, decimal Cost)
        {
            try
            {
                return access.ExecuteNonQuery("Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('" + ItemCode + "', '" + ItemDesc + "', " + Cost.ToString() + ")");
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Deletes the given item from the ItemDesc table
        /// </summary>
        /// <param name="ItemCode">ItemCode for the item</param>
        /// <returns>Number of rows deleted</returns>
        public int DeleteItem(string ItemCode)
        {
            try
            {
                return access.ExecuteNonQuery("Delete from ItemDesc Where ItemCode = '" + ItemCode + "'");
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Returns the sum of item costs for all items on one particular invoice
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public decimal GetSumLineItemsCost(string InvoiceNum)
        {
            try
            {
                int numRows = 0;

                // sql statement
                DataSet ds = access.ExecuteSQLStatement("Select Sum(ItemDesc.Cost) from ItemDesc, LineItems where LineItems.InvoiceNum = " + InvoiceNum + " AND LineItems.ItemCode = ItemDesc.ItemCode", ref numRows);
                return (Decimal)ds.Tables[0].Rows[0][0];
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// Updates the TotalCost of the given invoice
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <param name="TotalCost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int UpdateInvoiceTotalCost(string InvoiceNum, decimal TotalCost)
        {
            try
            {
                return access.ExecuteNonQuery("UPDATE Invoices SET TotalCost = " + TotalCost.ToString() + " WHERE InvoiceNum = " + InvoiceNum);
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }
    }
}
