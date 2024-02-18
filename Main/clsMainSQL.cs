using CS3280GroupAssignment.Items;
using CS3280GroupAssignment.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace CS3280GroupAssignment.Main
{
    public class clsMainSQL
    {

        /// <summary>
        /// clsDataAccess class variable
        /// </summary>
        private clsDataAccess access;

        /// <summary>
        /// COnstructor
        /// </summary>
        public clsMainSQL() 
        {
            access = new clsDataAccess();
        }

        
        /// <summary>
        /// Updates the Invoice
        /// </summary>
        /// <param name="TotalCost"></param>
        /// <param name="InvoiceDate"></param>
        /// <param name="InvoiceNum"></param>
        /// <returns></returns>
        public int UpdateInvoice(decimal TotalCost,string InvoiceDate, string InvoiceNum)
        {
            try
            {
                return access.ExecuteNonQuery("Update Invoices Set TotalCost  =  " + TotalCost.ToString() + ", InvoiceDate = #" + InvoiceDate + "# where InvoiceNum = " + InvoiceNum);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Inserts a line item 
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <param name="LineItemNum"></param>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public int InsertLineItems(string InvoiceNum, int LineItemNum, string ItemCode)
        {
            try
            {
                return access.ExecuteNonQuery("Insert Into LineItems (InvoiceNum, LineItemNum, ItemCode) Values (" + InvoiceNum + ", " + LineItemNum.ToString() + ", '" + ItemCode.ToString() + "')");
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            } 
        }

        /// <summary>
        /// Inserts a New Invoice
        /// </summary>
        /// <param name="InvoiceDate"></param>
        /// <param name="TotalCost"></param>
        /// <returns></returns>
        public int InsertNewInvoice(string InvoiceDate, decimal TotalCost)
        {
            try
            {
                return access.ExecuteNonQuery("Insert into Invoices (InvoiceDate, TotalCost) Values ('" + InvoiceDate + "', " + TotalCost.ToString() + ")");
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the invoice that contains an Invoice number
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <returns></returns>
        public List<Invoice> GetInvoiceContainingInvoiceNum(string InvoiceNum)
        {
            try
            {
                int numRows = 0;
                List<Invoice> retlist = new List<Invoice>();

                DataSet ds = access.ExecuteSQLStatement("Select InvoiceNum, InvoiceDate, TotalCost From Invoices where InvoiceNum = " + InvoiceNum, ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Invoice invoice = new Invoice(dr[0].ToString(), dr[1].ToString(), Decimal.Parse(dr[2].ToString()));
                    retlist.Add((Invoice)dr[0]);
                }
                return retlist;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            } 
        }


        /// <summary>
        /// Method that gets the newest created Invoice
        /// </summary>
        /// <returns></returns>
        public Invoice GetNewestInvoice()
        {
            try
            {
                int numRows = 0;
                List<Invoice> retlist = new List<Invoice>();

                DataSet ds = access.ExecuteSQLStatement("Select InvoiceNum, InvoiceDate, TotalCost From Invoices where InvoiceNum = (Select Max(InvoiceNum) from Invoices)", ref numRows);

                Invoice invoice = new Invoice(ds.Tables[0].Rows[0][0].ToString(), ds.Tables[0].Rows[0][1].ToString(), Decimal.Parse(ds.Tables[0].Rows[0][2].ToString()));
                return invoice;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Gets all objects from the ItemDesc table and return them as an Item list.
        /// Code written by Daniel Oldham.
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
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Get Line items containing the invoice number
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <returns></returns>
        public List<Item> GetLineItemsContainingInvoiceNum(string InvoiceNum)
        {
            try
            {
                int numRows = 0;
                List<Item> retlist = new List<Item>();

                // SQL Statement
                DataSet ds = access.ExecuteSQLStatement("Select LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost from LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = " + InvoiceNum, ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Item item = new Item(dr[0].ToString(), dr[1].ToString(), (decimal)dr[2]);
                    retlist.Add(item);
                }

                return retlist;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes an invoice number
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <returns></returns>
        public int DeleteInvoiceNum (string InvoiceNum)
        {
            try
            {
                return access.ExecuteNonQuery("Delete from LineItems Where InvoiceNum = " + InvoiceNum);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

       /// <summary>
       /// Deletes a single Line item
       /// </summary>
       /// <param name="LineItemNum"></param>
       /// <returns></returns>
        public int DeleteSingleLineItem(string LineItemNum)
        {
            try
            {
                return access.ExecuteNonQuery("Delete from LineItems Where LineItemNum = " + LineItemNum);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            } 
        }

        /// <summary>
        /// Deletes all line items associated with one invoice
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <returns></returns>
        public int DeleteInvoiceLineItems(string InvoiceNum)
        {
            try
            {
                return access.ExecuteNonQuery("Delete from LineItems Where InvoiceNum = " + InvoiceNum);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        
    }
}
