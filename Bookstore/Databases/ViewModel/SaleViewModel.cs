using Bookstore.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Databases.ViewModel
{
    public class SaleViewModel
    {
        private static SaleViewModel _mySaleViewModel = new SaleViewModel();
        private ObservableCollection<Sale> _allSales = new ObservableCollection<Sale>();

        public SaleViewModel()
        {
            //Estable the connection to the View
        }

        public ObservableCollection<Sale> AllSales
        {
            get
            {
                return _mySaleViewModel._allSales;
            }
        }

        public IEnumerable<Sale> GetSales()
        {
            //clear list
            _mySaleViewModel._allSales.Clear();

            try
            {
                //get all sales from the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = @"SELECT * FROM sale";
                    App.MY_USERVIEWMODEL.GetEmployees();
                    App.MY_ORDERVIEWMODEL.GetOrders();

                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int saleID;
                            int orderID;
                            int empID;
                            double total;
                            double paid;

                            Int32.TryParse((reader.GetString("sale_id")), out saleID);
                            Int32.TryParse((reader.GetString("order_id")), out orderID);
                            Int32.TryParse((reader.GetString("saleby")), out empID);
                            Double.TryParse((reader.GetString("total")), out total);
                            Double.TryParse((reader.GetString("paid")), out paid);

                            string date = reader.GetString("date");
                            string[] numbers = date.Split('-');

                            DateTime dateToAdd = new DateTime(Int32.Parse(numbers[2]), Int32.Parse(numbers[1]), Int32.Parse(numbers[0]));

                            var getOrder = from order in App.MY_ORDERVIEWMODEL.AllOrders
                                           where order.OrderID == orderID
                                           select order;
                            Order thisOrder = getOrder.First();
                            var getEmployee = from employee in App.MY_USERVIEWMODEL.AllEmployees
                                              where employee.UserID == empID
                                              select employee;
                            Employee thisEmployee = getEmployee.First();

                            //add sale to the list
                            _mySaleViewModel._allSales.Add(new Sale(saleID, thisOrder, dateToAdd, total, paid, thisEmployee));
                        }

                    }

                }

            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not retrieve sales");
            }
            return _mySaleViewModel.AllSales;
        }

        public bool AddNewSale(Sale newSale)
        {
            //clear list
            _mySaleViewModel._allSales.Clear();

            try
            {
                //add new sale to the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = @"INSERT INTO sale
                                                (sale_id, order_id, date, total, paid, saleby)
                                                VALUES
                                                (@sid, @oid, @date, @total, @paid, @eid)";

                    string date = newSale.Date.Day + "-" + newSale.Date.Month + "-" + newSale.Date.Year;

                    insertCommand.Parameters.AddWithValue("@sid", newSale.SaleID);
                    insertCommand.Parameters.AddWithValue("@oid", newSale.Order.OrderID);
                    insertCommand.Parameters.AddWithValue("@date", date);
                    insertCommand.Parameters.AddWithValue("@total", newSale.TotalAmount);
                    insertCommand.Parameters.AddWithValue("@paid", newSale.Paid);
                    insertCommand.Parameters.AddWithValue("@eid", newSale.SaleBy.UserID);

                    insertCommand.ExecuteNonQuery();

                    return true;
                }

            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not add sale");
                return false;
            }


        }
    }
}
