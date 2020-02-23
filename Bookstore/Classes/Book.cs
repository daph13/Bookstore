using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Classes
{
    
    public class Book :Item
    {
        private string author;
        private string genre;

        public Book() :base()
        {
            this.Author = "";
            this.Genre = "";
        }

        public Book(int iid, string name, string desc, int stock, string type, string pic, double price, string author, string genre) 
            : base(iid, name, desc, stock, type, pic, price)
        {
            this.Author = author;
            this.Genre = genre;
        }

        public string Author
        {
            get { return this.author; }
            set { this.author = value; }
        }

        public string Genre
        {
            get { return this.genre; }
            set { this.genre = value; }
        }

        public override string ToString()
        {
            return "Book " + base.ToString() + Author + "\t" + Genre;
        }

    }
}
