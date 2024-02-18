using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3280GroupAssignment.Items
{
    public class Item : INotifyPropertyChanged
    {
        /// <summary>
        /// ItemCode from db
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Actual variable for item description
        /// </summary>
        private string _ItemDesc;

        /// <summary>
        /// ItemDesc property
        /// </summary>
        public string ItemDesc 
        { 
            get
            {
                return _ItemDesc;
            }
            set
            {
                _ItemDesc = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ItemDesc"));
                }
            }
        }

        /// <summary>
        /// Actual variable for item cost
        /// </summary>
        private decimal _Cost;

        /// <summary>
        /// Cost property
        /// </summary>
        public decimal Cost 
        {
            get
            {
                return _Cost;
            }
            set
            {
                _Cost = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Cost"));
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <param name="ItemDesc"></param>
        /// <param name="Cost"></param>
        public Item(string ItemCode, string ItemDesc, decimal Cost) 
        {
            this.ItemCode = ItemCode;
            this.ItemDesc = ItemDesc;
            this.Cost = Cost;
        }

        /// <summary>
        /// Default constructor stub
        /// </summary>
        public Item() { }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>ItemDesc as a string</returns>
        public override String ToString()
        {
            return this.ItemDesc;
        }

        /// <summary>
        /// PropertyChangedEventHandler object to comply with the interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
