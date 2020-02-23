using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    public class Customer : User
    {
        private string email;
        private string phone;

        public Customer() : base()
        {
            this.Email = "";
            this.Phone = "";
        }

        public Customer(int uid, string fName, string lName, string type, string email, string phone)
            :base(uid, fName, lName, type)
        {
            this.Email = email;
            this.Phone = phone;
        }

        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }

        public string CustomerName()
        {
            return base.FirstName + " " + base.LastName;
        }

        public override string ToString()
        {
            return "Customer: " + base.ToString() + "\t" + Email + "\t" + Phone;
        }
    }
}
