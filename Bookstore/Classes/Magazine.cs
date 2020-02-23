using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    public class Magazine : Item
    {
        //private int magID;
        private string publisher;

        public Magazine() : base()
        {
            //this.MagID = 0;
            this.Publisher = "";
        }

        //public Magazine(string name, string desc, int stock, string type, string pic, double price, string publisher)
        // : base(name, desc, stock, type, pic, price)
        //{
        //    //this.MagID = mid;
        //    this.Publisher = publisher;
        //}

        public Magazine(int iid, string name, string desc, int stock, string type, string pic, double price, string publisher) 
            : base(iid, name, desc, stock, type, pic, price)
        {
            //this.MagID = mid;
            this.Publisher = publisher; 
        }

        //public int MagID
        //{
        //    get { return this.magID; }
        //    set { this.magID = value; }
        //}


        public string Publisher
        {
            get { return this.publisher; }
            set { this.publisher = value; }
        }

        public override string ToString()
        {
            return "Magazine " + base.ToString() + "\t" + Publisher;
        }
    }
}
