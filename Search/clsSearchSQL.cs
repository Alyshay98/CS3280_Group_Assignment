using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CS3280GroupAssignment.Search
{
    internal class clsSearchSQL
    {
        /// <summary>
        /// clsDataAccess object
        /// </summary>
        private clsDataAccess access;

        /// <summary>
        /// instatiates the clsDataAccess object
        /// </summary>
        public clsSearchSQL()
        {
            access = new clsDataAccess();
        }

        /// <summary>
        /// general, gets the invoices in an SQL statement
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Invoice> GetInvoices()
        {
            try
            {
                int numRows = 0;
                List<Invoice> retList = new List<Invoice>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT * FROM Invoices", ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Invoice invoice = new Invoice(dr[0].ToString(), dr[1].ToString(), Convert.ToDecimal(dr[2]));
                    retList.Add(invoice);
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
        /// gets the invoices, with specific invoiceNum
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Invoice> GetInvoices(string invoiceNum)
        {
            try
            {
                int numRows = 0;
                List<Invoice> retList = new List<Invoice>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT * FROM Invoices WHERE InvoiceNum = " + invoiceNum, ref numRows);
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    Invoice invoice = new Invoice(dr[0].ToString(), dr[1].ToString(), Convert.ToDecimal(dr[2]));
                    retList.Add(invoice);
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
        /// gets invoices, with specified invoiceNum and invoiceDate
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Invoice> GetInvoices(string invoiceNum, string invoiceDate)
        {
            try
            {
                int numRows = 0;
                List<Invoice> retList = new List<Invoice>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT * FROM Invoices WHERE InvoiceNum = " + invoiceNum + "AND InvoiceDate = #" +
                    invoiceDate + "#", ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Invoice invoice = new Invoice(dr[0].ToString(), dr[1].ToString(), Convert.ToDecimal(dr[2]));
                    retList.Add(invoice);
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
        /// gets invoices, with specified invoiceNum, invoiceDate, and totalCost
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="invoiceDate"></param>
        /// <param name="totalCost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Invoice> GetInvoices(string invoiceNum, string invoiceDate, decimal totalCost)
        {
            try
            {
                int numRows = 0;
                List<Invoice> retList = new List<Invoice>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT * FROM Invoices WHERE InvoiceNum = " + invoiceNum + "AND InvoiceDate = #" +
                    invoiceDate + "# AND TotalCost = " + totalCost, ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Invoice invoice = new Invoice(dr[0].ToString(), dr[1].ToString(), Convert.ToDecimal(dr[2]));
                    retList.Add(invoice);
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
        /// gets invoices, with specified totalCost
        /// </summary>
        /// <param name="totalCost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Invoice> GetInvoicesWithCost(decimal totalCost)
        {
            try
            {
                int numRows = 0;
                List<Invoice> retList = new List<Invoice>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT * FROM Invoices WHERE TotalCost = " + totalCost, ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Invoice invoice = new Invoice(dr[0].ToString(), dr[1].ToString(), Convert.ToDecimal(dr[2]));
                    retList.Add(invoice);
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
        /// gets invoices, with specified totalCost and invoiceDate
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="invoiceDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Invoice> GetInvoicesWithCostDate(decimal totalCost, string invoiceDate)
        {
            try
            {
                int numRows = 0;
                List<Invoice> retList = new List<Invoice>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT * FROM Invoices WHERE TotalCost = " + totalCost + "and InvoiceDate = #"
                     + invoiceDate + "#", ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Invoice invoice = new Invoice(dr[0].ToString(), dr[1].ToString(), Convert.ToDecimal(dr[2]));
                    retList.Add(invoice);
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
        /// gets invoices, with specified invoiceDate
        /// </summary>
        /// <param name="invoiceDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Invoice> GetInvoiceDate(string invoiceDate)
        {
            try
            {
                int numRows = 0;
                List<Invoice> retList = new List<Invoice>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT * FROM Invoices WHERE InvoiceDate = #" + invoiceDate + "#", ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Invoice invoice = new Invoice(dr[0].ToString(), dr[1].ToString(), Convert.ToDecimal(dr[2]));
                    retList.Add(invoice);
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
        /// selects distinct invoiceNum
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetDistinctInvoiceNum()
        {
            try
            {
                int numRows = 0;
                List<string> retList = new List<string>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT DISTINCT(InvoiceNum) From Invoices order by InvoiceNum", ref numRows);
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    //retList.Add((Invoice)dr["InvoiceNum"]);
                    string invoiceNum = dr["InvoiceNum"].ToString();
                    
                    retList.Add(invoiceNum);
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
        /// selects distinct invoice date
        /// </summary>
        /// <param name="invoiceDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetDistinctInvoiceDate()
        {
            try
            {
                int numRows = 0;
                List<string> retList = new List<string>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT DISTINCT(InvoiceDate) From Invoices order by InvoiceDate", ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string invoiceDate = dr["InvoiceDate"].ToString();

                    retList.Add(invoiceDate);
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
        /// selects distinct total cost
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<decimal> GetDistinctTotalCost()
        {
            try
            {
                int numRows = 0;
                List<decimal> retList = new List<decimal>();

                //sql statement
                DataSet ds = access.ExecuteSQLStatement("SELECT DISTINCT(TotalCost) From Invoices order by TotalCost", ref numRows);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    decimal totalCost = Convert.ToDecimal(dr["TotalCost"]);

                    retList.Add(totalCost);
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
        /// Handle the error.
        /// </summary>
        /// <param name="sClass">The class in which the error occurred in.</param>
        /// <param name="sMethod">The method in which the error occurred in.</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                //Would write to a file or database here.
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        }
    }
}
