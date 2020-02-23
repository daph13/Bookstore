using Bookstore.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Databases.ViewModel
{
    public class OrderViewModel
    {
        private static OrderViewModel _myOrderViewModel = new OrderViewModel();
        private ObservableCollection<Order> _allOrders = new ObservableCollection<Order>();

        public OrderViewModel()
        {
            //Estable the connection to the View
        }

        public ObservableCollection<Order> AllOrders
        {
            get
            {
                return _myOrderViewModel._allOrders;
            }
        }

        public bool AddNewOrder(Order newOrder)
        {
            // Insert a new order to the database
            try
            {
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand insertCustomerCommand = connection.CreateCommand();
                    MySqlCommand insertCommand1 = connection.CreateCommand();
                    MySqlCommand insertCommand2 = connection.CreateCommand();



                    insertCommand1.CommandText = @"INSERT INTO `order`
                                                (orderID, code, date, user_id, complete)
                                                VALUES
                                                (@id, @code, @date, @uid, @complete)";

                    string date = newOrder.Date.Day + "-" + newOrder.Date.Month + "-" + newOrder.Date.Year;


                    insertCommand1.Parameters.AddWithValue("@id", newOrder.OrderID);
                    insertCommand1.Parameters.AddWithValue("@code", newOrder.Code);
                    insertCommand1.Parameters.AddWithValue("@date", date);
                    insertCommand1.Parameters.AddWithValue("@uid", newOrder.Customer.UserID);
                    insertCommand1.Parameters.AddWithValue("@complete", 0);

                    //add order to the order table
                    insertCommand1.ExecuteNonQuery();

                    insertCommand2.CommandText = @"INSERT INTO orderitem
                                                  (order_id, item_id, quantity)
                                                  VALUES
                                                  (@oid, @iid, @quantity)";
                    
                    insertCommand2.Parameters.AddWithValue("@oid", newOrder.OrderID);

                    //add order items in orderitem table
                    foreach (Item i in newOrder.OrderItems)
                    {
                        insertCommand2.Parameters.AddWithValue("@iid", i.ItemID);
                        insertCommand2.Parameters.AddWithValue("@quantity", i.NumInEachOrder);
                        insertCommand2.ExecuteNonQuery();

                        insertCommand2.Parameters.RemoveAt("@iid");
                        insertCommand2.Parameters.RemoveAt("@quantity");

                        i.ResetItemInOrder();
                    }

                    return true;
                }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not add item");
                return false;
            }
        }


        public IEnumerable<Order> GetOrders()
        {
            //clear list
            _myOrderViewModel._allOrders.Clear();

            try
            {
                //get all orders from the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM `order`";
                    App.MY_USERVIEWMODEL.GetUsers();

                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderID;
                            int custID;

                            Int32.TryParse((reader.GetString("orderID")), out orderID);
                            Int32.TryParse((reader.GetString("user_id")), out custID);
                            bool complete = reader.GetBoolean("complete");
                            string code = reader.GetString("code");


                            string date = reader.GetString("date");
                            string[] numbers = date.Split('-');

                            DateTime dateToAdd = new DateTime(Int32.Parse(numbers[2]), Int32.Parse(numbers[1]), Int32.Parse(numbers[0]));

                            //get customer object with that has the same id as the customer id from the order
                            Customer customer = (Customer)App.MY_USERVIEWMODEL.AllUsers.FirstOrDefault(u => u.UserID == custID);

                            //add order to the list
                            _myOrderViewModel._allOrders.Add(new Order(orderID, code, dateToAdd, customer, complete, new List<Item>()));

                        }
                    }

                    getCommand.CommandText = "SELECT * FROM orderitem";
                    //get all order items
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        App.MY_ITEMVIEWMODEL.GetItems();
                        while (reader.Read())
                        {
                            int orderID;
                            int itemID;
                            int quantity;
                            int count = 1;

                            Int32.TryParse((reader.GetString("order_id")), out orderID);
                            Int32.TryParse((reader.GetString("item_id")), out itemID);
                            Int32.TryParse((reader.GetString("quantity")), out quantity);

                            while (count <= quantity)
                            {
                                Item item = App.MY_ITEMVIEWMODEL.AllItems.FirstOrDefault(i => i.ItemID == itemID);
                                Order order = _myOrderViewModel.AllOrders.FirstOrDefault(o => o.OrderID == orderID);
                                order.OrderItems.Add(item);
                                count++;
                            }

                        }
                    }

                }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not retrieve orders");
            }
            return _myOrderViewModel.AllOrders;
        }

        public bool DeleteOrder(int orderID)
        {
            try
            {
                //delete order in the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    //remove order items based on order id
                    MySqlCommand deleteCommand = connection.CreateCommand();
                    deleteCommand.CommandText = @"DELETE FROM orderitem WHERE order_id = @orderID";
                    deleteCommand.Parameters.AddWithValue("@orderID", orderID);

                    deleteCommand.ExecuteNonQuery();
                    //remove database
                    deleteCommand.CommandText = @"DELETE FROM `order` WHERE orderID = @orderID";
                    deleteCommand.ExecuteNonQuery();

                    return true;

                }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not remove order");
                return false;
            }
        }

        public bool UpdateOrder(Order order)
        {
            try
            {
                //update order in the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand updateCommand = connection.CreateCommand();
                    //remove order items based on order id
                    updateCommand.CommandText = @"DELETE FROM orderitem WHERE order_id = @orderID";
                    updateCommand.Parameters.AddWithValue("@orderID", order.OrderID);

                    updateCommand.ExecuteNonQuery();
                    
                    updateCommand.CommandText = @"INSERT INTO orderitem
                                                  (order_id, item_id, quantity)
                                                  VALUES
                                                  (@orderID, @iid, @quantity)";


                    App.MY_ITEMVIEWMODEL.GetItems();

                        //add updated order items
                        foreach(Item item in order.OrderItems)
                        {
                            updateCommand.Parameters.AddWithValue("@iid", item.ItemID);
                            updateCommand.Parameters.AddWithValue("@quantity", item.NumInEachOrder);
                            updateCommand.ExecuteNonQuery();

                            updateCommand.Parameters.RemoveAt("@iid");
                            updateCommand.Parameters.RemoveAt("@quantity");

                            item.ResetItemInOrder();
                        }


                }

                    return true;
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not update order");
                return false;
            }
        }

        public bool UpdateOrderStatus(int orderID)
        {
            try
            {
                //update order's complete attribute in the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand updateCommand = connection.CreateCommand();

                    updateCommand.CommandText = @"UPDATE `order`
                                                   SET complete = 1
                                                   WHERE orderID = @orderID";

                    updateCommand.Parameters.AddWithValue("@orderID", orderID);
                    updateCommand.ExecuteNonQuery();

                    return true;
                }

            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not update order status");
                return false;
            }
        }

    }
}
