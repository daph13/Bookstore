using Bookstore.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Bookstore.Databases.ViewModel
{
    public class ItemViewModel
    {
        private static ItemViewModel _myItemViewModel = new ItemViewModel();
        private ObservableCollection<Item> _allItems = new ObservableCollection<Item>();

        public ItemViewModel()
        {
            //Estable the connection to the View
        }

        public ObservableCollection<Item> AllItems
        {
            get
            {
                return _myItemViewModel._allItems;
            }
        }

        public IEnumerable<Item> GetItems()
        {
            //clear the list
            _myItemViewModel._allItems.Clear();

            try
            {
                //get all book, magazine and stationery items from the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM ((item LEFT JOIN book ON item.item_id = book.item_id)" +
                                                                  "LEFT JOIN magazine ON item.item_id = magazine.item_id)" +
                                                                  "LEFT JOIN stationery ON item.item_id = stationery.item_id ORDER BY item.item_id";

                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int itemID;
                            int stock;
                            double price;

                            Int32.TryParse((reader.GetString("item_id")), out itemID);
                            Int32.TryParse((reader.GetString("stock")), out stock);
                            Double.TryParse((reader.GetString("price")), out price);

                            string name = reader.GetString("item_name");
                            string description = reader.GetString("description");
                            string type = reader.GetString("itemtype");
                            string picture = reader.GetString("picture");
                            bool inInventory = reader.GetBoolean("inInventory");
                            
                            //add items to the list based on the type of item
                            if(type == "book")
                            {

                                string author = reader.GetString("author");
                                string genre = reader.GetString("genre");

                                Book book = new Book(itemID, name, description, stock, type, picture, price, author, genre);
                                book.InInventory = inInventory;

                                _myItemViewModel._allItems.Add(book);
                            }
                            else if(type == "magazine")
                            {
       
                                string publisher = reader.GetString("publisher");
                                Magazine mag = new Magazine(itemID, name, description, stock, type, picture, price, publisher);
                                mag.InInventory = inInventory;
                                _myItemViewModel._allItems.Add(mag);

                            }
                            else if (type == "stationery")
                            {

                                string colour = reader.GetString("colour");
                                Stationery stat = new Stationery(itemID, name, description, stock, type, picture, price, colour);
                                stat.InInventory = inInventory;
                                _myItemViewModel._allItems.Add(stat);
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                // handle string
                System.Diagnostics.Debug.WriteLine(e.ToString());
                Debug.WriteLine("something wrong");
            }
            return _myItemViewModel.AllItems;
        }

        public bool AddNewItem(Item newItem)
        {

            try
            {
                //insert book, magazine or stationery item into the databse
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand insertCommand1 = connection.CreateCommand();
                    MySqlCommand insertCommand2 = connection.CreateCommand();
                    insertCommand1.CommandText = @"INSERT INTO item
                                                    (item_id, item_name, description, stock, itemtype, picture, price, inInventory)
                                                    VALUES
                                                    (@id, @name, @description, @stock, @type, @picture, @price, @inventory)";

                    insertCommand1.Parameters.AddWithValue("@id", newItem.ItemID);
                    insertCommand1.Parameters.AddWithValue("@name", newItem.ItemName);
                    insertCommand1.Parameters.AddWithValue("@description", newItem.Description);
                    insertCommand1.Parameters.AddWithValue("@stock", newItem.Stock);
                    insertCommand1.Parameters.AddWithValue("@type", newItem.ItemType);
                    insertCommand1.Parameters.AddWithValue("@picture", newItem.Picture);
                    insertCommand1.Parameters.AddWithValue("@price", newItem.Price);
                    insertCommand1.Parameters.AddWithValue("@inventory", newItem.InInventory);


                    if (newItem is Book)
                    {
                        Book newBook = (Book)newItem;
                        insertCommand2.CommandText = @"INSERT INTO book
                                                     (item_id, author, genre)
                                                     VALUES
                                                     (@id, @author, @genre)";
                        insertCommand2.Parameters.AddWithValue("@id", newBook.ItemID);
                        insertCommand2.Parameters.AddWithValue("@author", newBook.Author);
                        insertCommand2.Parameters.AddWithValue("@genre", newBook.Genre);
                    }
                    else if(newItem is Magazine)
                    {
                        Magazine newMag = (Magazine)newItem;
                        insertCommand2.CommandText = @"INSERT INTO magazine
                                                     (item_id, publisher)
                                                     VALUES
                                                     (@id, @publisher)";
                        insertCommand2.Parameters.AddWithValue("@id", newMag.ItemID);
                        insertCommand2.Parameters.AddWithValue("@publisher", newMag.Publisher);
                    }
                    else if(newItem is Stationery)
                    {
                        Stationery newStat = (Stationery)newItem;
                        insertCommand2.CommandText = @"INSERT INTO stationery
                                                     (item_id, colour)
                                                     VALUES
                                                     (@id, @colour)";
                        insertCommand2.Parameters.AddWithValue("@id", newStat.ItemID);
                        insertCommand2.Parameters.AddWithValue("@colour", newStat.Colour);

                    }

                    insertCommand1.ExecuteNonQuery();
                    insertCommand2.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Can't insert new Item");
                return false;
            }

        }

        public bool DeleteItem (int id, string type)
        {
            bool proceed = false;
            try
            {
                //remove item based on item id and string type
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();

                    MySqlCommand checkCommand = connection.CreateCommand();
                    checkCommand.CommandText = @"SELECT * from orderitem WHERE item_id = @itemID";
                    checkCommand.Parameters.AddWithValue("@itemID", id);

                    using (MySqlDataReader reader = checkCommand.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        Debug.WriteLine(dt.Rows.Count);
                        if (dt.Rows.Count == 0)
                        {
                            proceed = true;
                        }
                    }

                    if(proceed == true)
                    {
                        MySqlCommand deleteCommand1 = connection.CreateCommand();
                        MySqlCommand deleteCommand2 = connection.CreateCommand();

                        deleteCommand1.CommandText = @"DELETE FROM item WHERE item_id = @itemID";
                        deleteCommand1.Parameters.AddWithValue("@itemID", id);

                        if (type == "book")
                        {
                            deleteCommand2.CommandText = @"DELETE FROM book WHERE item_id = @itemID";
                        }
                        else if (type == "magazine")
                        {
                            deleteCommand2.CommandText = @"DELETE FROM magazine WHERE item_id = @itemID";
                        }
                        else if (type == "stationery")
                        {
                            deleteCommand2.CommandText = @"DELETE FROM stationery WHERE item_id = @itemID";
                        }

                        deleteCommand2.Parameters.AddWithValue("@itemID", id);
                        deleteCommand2.ExecuteNonQuery();
                        deleteCommand1.ExecuteNonQuery();

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not remove item");
                return false;
            }
        }

        public bool UpdateItem (Item item)
        {
            try
            {
                //update book, magazine or stationery item in the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand updateCommand1 = connection.CreateCommand();
                    MySqlCommand updateCommand2 = connection.CreateCommand();

                    updateCommand1.CommandText = @"UPDATE item
                                                   SET item_name = @name, description = @desc, stock = @stock, itemtype = @type, picture = @picture, price = @price, inInventory = @inventory
                                                   WHERE item_id = @id";
                    updateCommand1.Parameters.AddWithValue("@id", item.ItemID);
                    updateCommand1.Parameters.AddWithValue("@name", item.ItemName);
                    updateCommand1.Parameters.AddWithValue("@desc", item.Description);
                    updateCommand1.Parameters.AddWithValue("@stock", item.Stock);
                    updateCommand1.Parameters.AddWithValue("@type", item.ItemType);
                    updateCommand1.Parameters.AddWithValue("@picture", item.Picture);
                    updateCommand1.Parameters.AddWithValue("@price", item.Price);
                    updateCommand1.Parameters.AddWithValue("@inventory", item.InInventory);

                    if (item is Book)
                    {
                        Book book = (Book)item;
                        updateCommand2.CommandText = @"UPDATE book
                                                   SET author = @author, genre = @genre
                                                   WHERE item_id = @id";
                        updateCommand2.Parameters.AddWithValue("@id", book.ItemID);
                        updateCommand2.Parameters.AddWithValue("@author", book.Author);
                        updateCommand2.Parameters.AddWithValue("@genre", book.Genre);
                    }
                    else if (item is Magazine)
                    {
                        Magazine magazine = (Magazine)item;
                        updateCommand2.CommandText = @"UPDATE magazine
                                                   SET publisher = @publisher
                                                   WHERE item_id = @id";
                        updateCommand2.Parameters.AddWithValue("@id", magazine.ItemID);
                        updateCommand2.Parameters.AddWithValue("@publisher", magazine.Publisher);
                    }
                    else if (item is Stationery)
                    {
                        Stationery stationery = (Stationery)item;
                        updateCommand2.CommandText = @"UPDATE stationery
                                                   SET colour = @colour
                                                   WHERE item_id = @id";
                        updateCommand2.Parameters.AddWithValue("@id", stationery.ItemID);
                        updateCommand2.Parameters.AddWithValue("@colour", stationery.Colour);
                    }

                    updateCommand1.ExecuteNonQuery();
                    updateCommand2.ExecuteNonQuery();

                    return true;

                }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not update item");
                return false;
            }
        }

        public bool UpdateItemStatus(Item item)
        {
            try
            {
                //update item's inInventory attribute in the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = @"UPDATE item
                                                  SET inInventory = @inInventory
                                                  WHERE item_id = @id";

                    updateCommand.Parameters.AddWithValue("@id", item.ItemID);
                    updateCommand.Parameters.AddWithValue("@inInventory", item.InInventory);
                    updateCommand.ExecuteNonQuery();
                    return true;
                }
            }
            catch(MySqlException)
            {
                Debug.WriteLine("Could not update item");
                return false;
            }
            
        }
    }
}
