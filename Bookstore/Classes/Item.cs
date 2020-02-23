using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    public class Item
    {
        private int itemID;
        private string itemName;
        private string description;
        private int stock;
        private string itemType;
        private string picture;
        private double price;
        private int numInEachOrder = 0;
        private bool inInventory = true;

        //private int count = 1;

        public Item()
        {
            this.ItemID = 1;
            this.ItemName = "";
            this.Description = "";
            this.Stock = 0;
            this.ItemType = "";
            this.Picture = "";
            this.Price = 0.00;
            this.InInventory = true;
        }

        //public Item(string name, string desc, int stock, string type, string pic, double price)
        //{
        //    this.ItemName = name;
        //    this.Description = desc;
        //    this.Stock = stock;
        //    this.ItemType = type;
        //    this.Picture = pic;
        //    this.Price = price;
        //}

        public Item(int id, string name, string desc, int stock, string type, string pic, double price)
        {
            this.ItemID = id;
            this.ItemName = name;
            this.Description = desc;
            this.Stock = stock;
            this.ItemType = type;
            this.Picture = pic;
            this.Price = price;
            this.InInventory = true;
        }


        public int ItemID
        {
            get { return this.itemID; }
            set { this.itemID = value; }
        }

        public string ItemName
        {
            get { return this.itemName; }
            set { this.itemName = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public int Stock
        {
            get { return this.stock; }
            set { this.stock = value; }
        }

        public string ItemType
        {
            get { return this.itemType; }
            set { this.itemType = value; }
        }

        public string Picture
        {
            get { return this.picture; }
            set { this.picture = value; }
        }

        public double Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        public bool InInventory
        {
            get { return this.inInventory; }
            set { this.inInventory = value; }
        }

        public int NumInEachOrder
        {
            get { return this.numInEachOrder; }
            set { this.numInEachOrder = value; }
        }

        public void ResetItemInOrder()
        {
            this.NumInEachOrder = 0;
        }

        public string PriceDisplay()
        {
            return Price.ToString("C");
        }


        public override string ToString()
        {
            return "Item ID: " + ItemID + "\t" + ItemName + "\t" + Description + "\tStock: " + Stock + "\t" + ItemType + "\t" + Picture;
        }
    }
}
