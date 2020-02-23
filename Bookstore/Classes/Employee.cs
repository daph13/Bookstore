using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    public class Employee : User
    {
        private bool isManager;
        private string password;
        private bool isEmployed;

        public Employee() : base()
        {
            this.IsManager = false;
            this.Password = "";
            this.IsEmployed = true;
        }

        public Employee(int uid, string fName, string lName, string type, bool manager, string password, bool employed)
            :base(uid, fName, lName, type)
        {
            this.IsManager = manager;
            this.Password = password;
            this.IsEmployed = employed;
        }

        public bool IsManager
        {
            get { return this.isManager; }
            set { this.isManager = value; }
        }

        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public bool IsEmployed
        {
            get { return this.isEmployed; }
            set { this.isEmployed = value; }
        }

        public string EmployeeName()
        {
            return base.FirstName + " " + base.LastName;
        }

        public override string ToString()
        {
            return "Employee " +  base.ToString() + "\tManager: " + IsManager + "\t" + Password + "\tEmployed: " + IsEmployed;
        }
    }
}
