using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    public class Order
    {
        private int orderID;
        private string code;
        private DateTime date;
        private Customer customer;
        private bool isComplete;
        private List<Item> orderItems;


        public Order()
        {
            this.OrderID = 0;
            this.Code = "";
            this.Date = DateTime.Now;
            this.Customer = new Customer();
            this.IsComplete = false;
            this.OrderItems = new List<Item>();
        }

        public Order(int oid, string code, Customer cust, bool completed, List<Item> items)
        {
            this.OrderID = oid;
            this.Code = code;
            this.Date = DateTime.Now;
            this.Customer = cust;
            this.IsComplete = completed;
            this.OrderItems = items;
        }

        public Order(int oid, string code, DateTime d, Customer cust, bool completed, List<Item> items)
        {
            this.OrderID = oid;
            this.Code = code;
            this.Date = d;
            this.Customer = cust;
            this.IsComplete = completed;
            this.OrderItems = items;
        }

        public int OrderID
        {
            get { return this.orderID; }
            set { this.orderID = value; }
        }

        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }


        public Customer Customer
        {
            get { return this.customer; }
            set { this.customer = value; }
        }

        public bool IsComplete
        {
            get { return this.isComplete; }
            set { this.isComplete = value; }
        }

        public List<Item> OrderItems
        {
            get { return this.orderItems; }
            set { this.orderItems = value; }
        }

        public string DateToString()
        {
            return Date.Day + "/" + Date.Month + "/" + Date.Year;
        }

        public string TotalItems()
        {
            return OrderItems.Count.ToString();
        }

        public override string ToString()
        {
            return "Order " + OrderID + "\t" + Code + "\t" + Date + "\tCustomer: " + Customer.UserID + "\tCompleted: " + isComplete + "\tItems: " + OrderItems.ToString();
        }
    }
}
