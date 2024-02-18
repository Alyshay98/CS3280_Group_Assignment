using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3280GroupAssignment.Search
{
    public class Invoice : INotifyPropertyChanged
    {
        /// <summary>
        /// variable for the invoice number
        /// </summary>
        private string _InvoiceNum;

        /// <summary>
        /// property for the invoice num
        /// </summary>
        public string InvoiceNum
        {
            get { return _InvoiceNum; }
            set
            {
                _InvoiceNum = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InvoiceNum")); }
            }
        }

        /// <summary>
        /// variable for the invoice date
        /// </summary>
        private string _InvoiceDate;

        /// <summary>
        /// property for the invoice date
        /// </summary>
        public string InvoiceDate
        {
            get { return _InvoiceDate; }
            set
            {
                _InvoiceDate = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InvoiceDate")); }
            }
        }

        /// <summary>
        /// variable for the total cost
        /// </summary>
        private decimal _TotalCost;

        /// <summary>
        /// property for the invoice date
        /// </summary>
        public decimal TotalCost
        {
            get { return _TotalCost; }

            set
            {
                _TotalCost = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InvoiceDate")); }
            }
        }

        /// <summary>
        /// public constructor for the Invoice object
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="invoiceDate"></param>
        /// <param name="totalCost"></param>
        public Invoice(string invoiceNum, string invoiceDate, decimal totalCost)
        {
            this.InvoiceNum = invoiceNum;
            this._InvoiceDate = invoiceDate;
            this._TotalCost = totalCost;
        }


        /// <summary>
        /// PropertyChangedEventHandler object to comply with the interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
