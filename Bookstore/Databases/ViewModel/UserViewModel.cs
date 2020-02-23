using Bookstore.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Databases
{
    public class UserViewModel
    {
        private static UserViewModel _myUserViewModel = new UserViewModel();
        private ObservableCollection<User> _allUsers = new ObservableCollection<User>();
        private ObservableCollection<Employee> _allEmployees = new ObservableCollection<Employee>();


        public UserViewModel()
        {
            //Estable the connection to the View
        }

        public ObservableCollection<User> AllUsers
        {
            get
            {
                return _myUserViewModel._allUsers;
            }
        }

        public ObservableCollection<Employee> AllEmployees
        {
            get
            {
                return _myUserViewModel._allEmployees;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            //clear list
            _myUserViewModel._allUsers.Clear();

            try
            {
                //get users from the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM user LEFT JOIN employee ON user.user_id = employee.user_id " +
                                              "LEFT JOIN customer ON user.user_id = customer.user_id ORDER BY user.user_id";

                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int userID;
                            Int32.TryParse((reader.GetString("user_id")), out userID);

                            string firstName = reader.GetString("firstname");
                            string lastName = reader.GetString("lastname");
                            string userType = reader.GetString("user_type");

                            //add user to the list based on the type of user
                            if(userType == "employee")
                            {
                                bool isManager = reader.GetBoolean("isManager");
                                string password = reader.GetString("password");
                                bool isEmployed = reader.GetBoolean("employed");

                                _myUserViewModel._allUsers.Add(new Employee(userID, firstName, lastName, userType, isManager, password, isEmployed));
                            }
                            else if (userType == "customer")
                            {
                                string email = reader.GetString("email");
                                string phone = reader.GetString("phone");

                                _myUserViewModel._allUsers.Add(new Customer(userID, firstName, lastName, userType, email, phone));
                            }
                        }
                    }
                }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not get users");
            }
            return _myUserViewModel.AllUsers;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            //clear list
            _myUserViewModel._allEmployees.Clear();

            try
            { 
                //get all employees
            using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
            {
                connection.Open();
                MySqlCommand getCommand = connection.CreateCommand();
                getCommand.CommandText = "SELECT * FROM user INNER JOIN employee ON user.user_id = employee.user_id";

                using (MySqlDataReader reader = getCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int uid;

                        Int32.TryParse((reader.GetString("user_id")), out uid);
                        string fName = reader.GetString("firstname");
                        string lName = reader.GetString("lastname");
                        string type = reader.GetString("user_type");
                        bool isManager = reader.GetBoolean("isManager");
                        string password = reader.GetString("password");
                        bool isEmployed = reader.GetBoolean("employed");

                        _myUserViewModel._allEmployees.Add(new Employee(uid, fName, lName, type, isManager, password, isEmployed));

                    }
                }

             }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not get employees");
            }
            return _myUserViewModel.AllEmployees;

        }

        public bool AddNewUser(User newUser)
        {
            try
            {
                //add new user to the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand insertCommand1 = connection.CreateCommand();
                    MySqlCommand insertCommand2 = connection.CreateCommand();

                    insertCommand1.CommandText = @"INSERT INTO user
                                                  (user_id, firstname, lastname, user_type)
                                                  VALUES
                                                  (@id, @first_name, @last_name, @type)";

                    insertCommand1.Parameters.AddWithValue("@id", newUser.UserID);
                    insertCommand1.Parameters.AddWithValue("@first_name", newUser.FirstName);
                    insertCommand1.Parameters.AddWithValue("@last_name", newUser.LastName);
                    insertCommand1.Parameters.AddWithValue("@type", newUser.UserType);

                    insertCommand1.ExecuteNonQuery();

                    //add user based on type of user
                    if(newUser is Employee)
                    {
                        Employee newEmployee = (Employee)newUser;

                        insertCommand2.CommandText = @"INSERT INTO employee
                                                  (user_id, isManager, password, employed)
                                                  VALUES
                                                  (@id, @isManager, @password, @employed)";

                        insertCommand2.Parameters.AddWithValue("@id", newEmployee.UserID);
                        insertCommand2.Parameters.AddWithValue("@isManager", newEmployee.IsManager);
                        insertCommand2.Parameters.AddWithValue("@password", newEmployee.Password);
                        insertCommand2.Parameters.AddWithValue("@employed", newEmployee.IsEmployed);

                    }
                    else if (newUser is Customer)
                    {
                        Customer newCustomer = (Customer)newUser;
                        insertCommand2.CommandText = @"INSERT INTO customer
                                                  (user_id, email, phone)
                                                  VALUES
                                                  (@id, @email, @phone)";

                        insertCommand2.Parameters.AddWithValue("@id", newCustomer.UserID);
                        insertCommand2.Parameters.AddWithValue("@email", newCustomer.Email);
                        insertCommand2.Parameters.AddWithValue("@phone", newCustomer.Phone);
                    }

                    insertCommand2.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not add user");
                return false;
            }
        }

        public bool UpdateEmployee(Employee employee)
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = @"UPDATE user
                                                  SET firstname = @fName, lastname = @lName
                                                  WHERE user_id = @id";

                    updateCommand.Parameters.AddWithValue("@id", employee.UserID);
                    updateCommand.Parameters.AddWithValue("@fName", employee.FirstName);
                    updateCommand.Parameters.AddWithValue("@lName", employee.LastName);

                    updateCommand.ExecuteNonQuery();

                    updateCommand.CommandText = @"UPDATE employee
                                                  SET isManager = @manager, password = @password
                                                  WHERE user_id = @id";
                    updateCommand.Parameters.AddWithValue("@manager", employee.IsManager);
                    updateCommand.Parameters.AddWithValue("@password", employee.Password);

                    updateCommand.ExecuteNonQuery();
                    return true;
                }

            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not update employee");
                return false;
            }
        }

        public bool UpdateEmploymentStatus(Employee employee)
        {
            try
            {
                //update employee's employed attribute in the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = @"UPDATE employee
                                                  SET employed = @employed
                                                  WHERE user_id = @id";

                    updateCommand.Parameters.AddWithValue("@id", employee.UserID);
                    updateCommand.Parameters.AddWithValue("@employed", employee.IsEmployed);
                    updateCommand.ExecuteNonQuery();
                    return true;
                }
            }
            catch(MySqlException)
            {
                Debug.WriteLine("Could not update Employee");
                return false;
            }
        }

        public bool DeleteUser(User user)
        {
            try
            {
                //delete user from the database
                using (MySqlConnection connection = new MySqlConnection(App.masterConnectionString))
                {
                    connection.Open();
                    MySqlCommand deleteCommand = connection.CreateCommand();
                    MySqlCommand getCommand = connection.CreateCommand();
                    bool exists;
                    //delete user based on type of user
                    if(user is Customer)
                    {
                        deleteCommand.CommandText = @"DELETE FROM customer WHERE user_id = @userID";
                        deleteCommand.Parameters.AddWithValue("@userID", user.UserID);

                        deleteCommand.ExecuteNonQuery();
                        deleteCommand.CommandText = @"DELETE FROM user WHERE user_id = @userID";

                        deleteCommand.ExecuteNonQuery();
                    }
                    else if (user is Employee)
                    {
                        Employee employee = (Employee)user;
                        getCommand.CommandText = @"SELECT * FROM sale WHERE saleby = @userID";
                        getCommand.Parameters.AddWithValue("@userID", user.UserID);

                        //if employee has made a sale
                        using (MySqlDataReader reader = getCommand.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                exists = true;
                            }
                            else
                            {
                                exists = false;
                            }
                        }

                        //update employee's employed attribute
                        if(exists == true)
                        {
                            deleteCommand.CommandText = @"UPDATE user 
                                                        SET employed = @employed
                                                        WHERE user_id = @userID";
                            deleteCommand.Parameters.AddWithValue("@employed", employee.IsEmployed);
                            deleteCommand.Parameters.AddWithValue("@userID", employee.UserID);
                            deleteCommand.ExecuteNonQuery();

                        }
                        else
                        {
                            //remove employee from database
                            deleteCommand.CommandText = @"DELETE FROM employee WHERE user_id = @userID";
                            deleteCommand.Parameters.AddWithValue("@userID", employee.UserID);

                            deleteCommand.ExecuteNonQuery();

                            deleteCommand.CommandText = @"DELETE FROM user WHERE user_id = @userID";

                            deleteCommand.ExecuteNonQuery();

                        }

                    }
                    return true;
                }
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Could not remove user");
                return false;
            }
        }
        
    }
}
