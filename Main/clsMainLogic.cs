using CS3280GroupAssignment.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CS3280GroupAssignment.Main
{
    public class clsMainLogic
    {
        // GetAll Items returns List<clsItem>

        /// <summary>
        /// Variable to get the Items class
        /// </summary>
        Items.clsItemsSQL sSql;

        /// <summary>
        /// Search Class Variable
        /// </summary>
        Search.Invoice clsInvoice;

        /// <summary>
        /// MainSQL class variable
        /// </summary>
        clsMainSQL MainSQL;

        /// <summary>
        /// Constructor
        /// </summary>
        public clsMainLogic()
        {
            sSql = new Items.clsItemsSQL();
            MainSQL = new clsMainSQL();
            //dataAccess = new clsDataAccess();
        }

        /// <summary>
        /// Gets all the items 
        /// </summary>
        /// <returns></returns>
        public List<Items.Item> GetAllItems()
        {
            return sSql.GetAllItems();
        }

        /// <summary>
        /// Saves a new Invoice
        /// </summary>
        /// <param name="newInvoice"></param>
        /// <param name="TotalCost"></param>
        /// <param name="InvoiceDate"></param>
        public void SaveNewInvoice(Invoice newInvoice, int TotalCost, string InvoiceDate)
        {
            //Call SQL method to create new Invoice with values from newInvoice obj
            try
            {
                MainSQL.InsertNewInvoice(InvoiceDate, TotalCost);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Edits an Invoice
        /// </summary>
        /// <param name="OldInvoiceID"></param>
        /// <param name="clsNewInvoice"></param>
        /// <exception cref="Exception"></exception>
       public void EditInvoice(string OldInvoiceID, string clsNewInvoice, string InvoiceDate, int TotalCost)
       {
           
            try
            {
                // grab old invoice from DB
                List<Invoice> SSQL = MainSQL.GetInvoiceContainingInvoiceNum(OldInvoiceID);

                ///updateInvoice SQL method
                MainSQL.UpdateInvoice(TotalCost, InvoiceDate ,clsNewInvoice);

                // Edit old invoice fields in DB using fields from clsNewInvoice

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the Invoice
        /// Returns the Invoice
        /// </summary>
        /// <param name="InvoiceNumber"></param>
        /// <exception cref="Exception"></exception>
        public void GetInvoice(string InvoiceNumber)
        {
            try
            {
                // getinvoice from db with InvoiceNumber
                MainSQL.GetInvoiceContainingInvoiceNum(InvoiceNumber);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }            
        }
    }
}
