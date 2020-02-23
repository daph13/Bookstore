using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    public class Stationery : Item
    {
        //private int statID;
        private string colour;

        public Stationery() : base()
        {
            //this.StatID = 0;
            this.Colour = ""; 
        }

        //public Stationery(string name, string desc, int stock, string type, string pic, double price, string colour)
        // : base(name, desc, stock, type, pic, price)
        //{
        //    //this.StatID = sid;
        //    this.Colour = colour;
        //}

        public Stationery(int iid, string name, string desc, int stock, string type, string pic, double price, string colour)
            :base(iid, name, desc, stock, type, pic, price)
        {
            //this.StatID = sid;
            this.Colour = colour;
        }

        //public int StatID
        //{
        //    get { return this.statID; }
        //    set { this.statID = value; }
        //}

        public string Colour
        {
            get { return this.colour; }
            set { this.colour = value; }
        }

        public override string ToString()
        {
            return "Stationery " + base.ToString() + "\t" + Colour;
        }

    }
}
