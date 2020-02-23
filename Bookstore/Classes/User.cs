using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    public class User
    {
        private int userID;
        private string firstName;
        private string lastName;
        private string userType;

        public User()
        {
            this.UserID = 0;
            this.FirstName = "";
            this.LastName = "";
            this.UserType = "";
        }

        public User(int id, string fName, string lName, string type)
        {
            this.UserID = id;
            this.FirstName = fName;
            this.LastName = lName;
            this.UserType = type;
        }

        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }

        public string UserType        {
            get { return this.userType; }
            set { this.userType = value; }
        }

        public override string ToString()
        {
            return "User ID: " + UserID + "\t" + FirstName + " " + LastName + "\t" + UserType;
        }
    }
}
