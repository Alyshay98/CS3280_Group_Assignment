using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CS3280GroupAssignment.Search
{
    public class clsSearchLogic
    {
        /// <summary>
        /// class sql object for search
        /// </summary>
        clsSearchSQL clsSearchSQL;

        /// <summary>
        /// will be the selected invoice from the DataGrid
        /// </summary>
        public Invoice SelectedInvoice { get; set; }

        /// <summary>
        /// public constructor instantiates the clsSearchSQL
        /// </summary>
        public clsSearchLogic()
        {
            try
            {
                clsSearchSQL = new clsSearchSQL();

                //initalize selectedInvoice
                SelectedInvoice = null;
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
        /// gets the distinct invoice num from sql class
        /// </summary>
        public List<string> getDistinctInvoiceNum()
        {
            try
            {
                return clsSearchSQL.GetDistinctInvoiceNum();
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// gets the distinct dates from sql class
        /// </summary>
        public List<string> getDistinctDates()
        {
            try
            {
                return clsSearchSQL.GetDistinctInvoiceDate();
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// gets the distinct costs from the sql class
        /// </summary>
        public List<decimal> getDistinctCosts()
        {
            try
            {
                return clsSearchSQL.GetDistinctTotalCost();
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }

        /// <summary>
        /// gets invoices depending on the data given
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="invoiceDate"></param>
        /// <param name="totalCost"></param>
        /// <returns></returns>
        public List<Invoice> GetInvoices(string invoiceNum, string invoiceDate, decimal totalCost)
        {
            try
            {
                //all invoices
                if (string.IsNullOrEmpty(invoiceNum) && string.IsNullOrEmpty(invoiceDate) && totalCost == 0)
                {
                    return clsSearchSQL.GetInvoices();
                }
                //all invoices where invoiceNum
                else if (!string.IsNullOrEmpty(invoiceNum) && string.IsNullOrEmpty(invoiceDate) && totalCost == 0)
                {
                    return clsSearchSQL.GetInvoices(invoiceNum);
                }
                //all invoices where invoiceNum & invoiceDate
                else if (!string.IsNullOrEmpty(invoiceNum) && !string.IsNullOrEmpty(invoiceDate) && totalCost == 0)
                {
                    return clsSearchSQL.GetInvoices(invoiceNum, invoiceDate);
                }
                //all invoices where invoiceNum & invoiceDate & totalCost
                else if (!string.IsNullOrEmpty(invoiceNum) && !string.IsNullOrEmpty(invoiceDate) && totalCost != 0)
                {
                    return clsSearchSQL.GetInvoices(invoiceNum, invoiceDate, totalCost);
                }
                //all invoices where totalCost
                else if (string.IsNullOrEmpty(invoiceNum) && string.IsNullOrEmpty(invoiceDate) && totalCost != 0)
                {
                    return clsSearchSQL.GetInvoicesWithCost(totalCost);
                }
                //all invoices where totalCost & invoiceDate
                else if (string.IsNullOrEmpty(invoiceNum) && !string.IsNullOrEmpty(invoiceDate) && totalCost != 0)
                {
                    return clsSearchSQL.GetInvoicesWithCostDate(totalCost, invoiceDate);
                }
                //all invoices where invoiceDate
                else if (string.IsNullOrEmpty(invoiceNum) && !string.IsNullOrEmpty(invoiceDate) && totalCost == 0)
                {
                    return clsSearchSQL.GetInvoiceDate(invoiceDate);
                }
                return new List<Invoice>();
            }
            catch (Exception e)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + e.Message);
            }
        }
    }
}
