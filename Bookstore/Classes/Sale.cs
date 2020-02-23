using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    public class Sale
    {
        private int saleID;
        private Order order;
        private DateTime date;
        private double totalAmount;
        private double paid;
        private Employee saleBy;

        public Sale()
        {
            this.SaleID = 0;
            this.Order = new Order();
            this.Date = DateTime.Now;
            this.TotalAmount = 0;
            this.Paid = 0;
        }

        public Sale(int sid, Order ord, double total, double paid, Employee saleby)
        {
            this.SaleID = sid;
            this.Order = ord;
            this.Date = DateTime.Now;
            this.TotalAmount = total;
            this.Paid = paid;
            this.SaleBy = saleby;
        }

        public Sale(int sid, Order ord, DateTime date, double total, double paid, Employee saleby)
        {
            this.SaleID = sid;
            this.Order = ord;
            this.Date = date;
            this.TotalAmount = total;
            this.Paid = paid;
            this.SaleBy = saleby;
        }

        public int SaleID
        {
            get { return this.saleID; }
            set { this.saleID = value; }
        }

        public Order Order
        {
            get { return this.order; }
            set { this.order = value; }
        }

        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public double TotalAmount
        {
            get { return this.totalAmount; }
            set { this.totalAmount = value; }
        }

        public double Paid
        {
            get { return this.paid; }
            set { this.paid = value; }
        }

        public Employee SaleBy
        {
            get { return this.saleBy; }
            set { this.saleBy = value; }
        }

        public string DateString()
        {
            return Date.Day + "-" + Date.Month + "-" + Date.Year;
        }

        public string TotalPriceString()
        {
            return TotalAmount.ToString("c");
        }

        public string PaidString()
        {
            return Paid.ToString("c");
        }

        public string ChangeString()
        {
            double change = Paid - TotalAmount;
            return change.ToString("c");
        }

        public string SaleItemsString()
        {
            List<Item> tempList = new List<Item>();
            string text = "";

            foreach (Item i in order.OrderItems)
            {
                i.ResetItemInOrder();
            }

                foreach (Item i in order.OrderItems)
            {
                i.NumInEachOrder++;
                if (tempList.IndexOf(i) == -1)
                {
                    tempList.Add(i);
                }
            }

            foreach (Item i in tempList)
            {
                text += i.ItemName + ":\t" + i.NumInEachOrder + "\n";
            }

            foreach (Item i in tempList)
            {
                i.ResetItemInOrder();
            }


            return text;

        }


        public override string ToString()
        {
            return "Sale " + SaleID + "\tOrderID: " + Order.OrderID + "\t" + Date + "\tTotalAmount: " + TotalAmount + "\tPaid: " + Paid + "\tEmployee: " + SaleBy.FirstName + " " + SaleBy.LastName;
        }
    }
}
