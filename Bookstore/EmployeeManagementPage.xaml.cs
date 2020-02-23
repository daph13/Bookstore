using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bookstore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmployeeManagementPage : Page
    {
        public EmployeeManagementPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //display logged employee's ID and name in textblocks
            txtEmployeeID.Text = App.employeeLogged.UserID.ToString();
            txtEmployeeName.Text = App.employeeLogged.EmployeeName();
            //get the employees
            Get_Employees();
        }

        private void Get_Employees()
        {
            //retrieve employees from database
            App.MY_USERVIEWMODEL.GetEmployees();
            //get employees that are currently employed
            var employeeList = from employee in App.MY_USERVIEWMODEL.AllEmployees
                                        where employee.IsEmployed == true
                                        select employee;
            //populate listEmployees
            listEmployees.ItemsSource = employeeList;
        }

        private void Clear_Fields()
        {
            //deselect item in listEmployees
            listEmployees.SelectedIndex = -1;
            //clear textboxes
            txtID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            passwordEmp.Password = "";
            //deselect item in combobox
            comboIsManager.SelectedIndex = -1;
            //uncheck checkbox
            checkBoxShowPassword.IsChecked = false;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            //clear fields
            Clear_Fields();
        }
  

        private void ListEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if an item in listEmployees is selected
            if(listEmployees.SelectedIndex != -1)
            {
                //cast the item as an Employee
                Employee employee = (Employee)listEmployees.SelectedItem;
                //fill the required fields
                txtID.Text = employee.UserID.ToString();
                txtFirstName.Text = employee.FirstName;
                txtLastName.Text = employee.LastName;
                passwordEmp.Password = employee.Password;
                //if the employee is a manager
                if(employee.IsManager == true)
                {
                    //set combobox selection to the first selection
                    comboIsManager.SelectedIndex = 0;
                }
                else
                {
                    //set combobox selection to the second selection
                    comboIsManager.SelectedIndex = 1;
                }
            }
        }

        private void CheckBoxShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            //show password
            passwordEmp.PasswordRevealMode = PasswordRevealMode.Visible;

        }

        private void CheckBoxShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            //hide password
            passwordEmp.PasswordRevealMode = PasswordRevealMode.Hidden;
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog;
            //if fields are empty
            if(txtFirstName.Text == "" || txtLastName.Text == "" || passwordEmp.Password.ToString() == "")
            {
                //display error message
                dialog = new MessageDialog("Please fill all fields","Fill All Fields");
                await dialog.ShowAsync();
            }
            // if no selection is made in the combobox
            else if(comboIsManager.SelectedIndex == -1)
            {
                //display error message
                dialog = new MessageDialog("Please indicate if the employee is a manager", "Manager?");
                await dialog.ShowAsync();
            }
            //if the password is less than 6 charaters
            else if (passwordEmp.Password.ToString().Length < 6)
            {
                //display error message
                dialog = new MessageDialog("Password should be at least 6 characters long");
                await dialog.ShowAsync();
            }
            else
            {
                //get the current user ID
                int id = Get_LastUserID() + 1;
                //get required values from fields
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                string password = passwordEmp.Password.ToString();
                bool isManager;
                //if the first item in combobox is selected
                if(comboIsManager.SelectedIndex == 0)
                {
                    //isManager is set true
                    isManager = true;
                }
                else
                {
                    //isManager is set false
                    isManager = false;
                }

               //add employee to database
               bool success = App.MY_USERVIEWMODEL.AddNewUser(new Employee(id, firstName, lastName, "employee", isManager, password, true));

                //if employee is successfully added
                if(success == true)
                {
                    //clear fields
                    Clear_Fields();  
                    //get employees
                    Get_Employees();
                }
                
            }
        }

        private int Get_LastUserID()
        {
            int lastUser;
            //get all users
            App.MY_USERVIEWMODEL.GetUsers();
            //if there are users
            if (App.MY_USERVIEWMODEL.AllUsers.Count != 0)
            {
                //get the last user ID
                lastUser = App.MY_USERVIEWMODEL.AllUsers.Max(u => u.UserID);
            }
            else
            {
                //set the last user ID to 0
                lastUser = 0;
            }

            return lastUser;
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog;
            //if an item is selected in listEmployees
            if(listEmployees.SelectedIndex != -1)
            {
                //cast Employee on the item 
                Employee selectedEmployee = (Employee)listEmployees.SelectedItem;
                //if the logged employee is the same as the selected employee
                if (App.employeeLogged.UserID == selectedEmployee.UserID)
                {
                    //display error message
                    dialog = new MessageDialog("Can't remove yourself!", "Cannot Remove");
                    await dialog.ShowAsync();
                }
                else
                {
                    //display confirmation message
                    dialog = new MessageDialog("Remove this employee?","Remove Employee");
                    var yes = new UICommand("Yes");
                    var no = new UICommand("No");
                    dialog.Commands.Add(yes);
                    dialog.Commands.Add(no);

                    var command = await dialog.ShowAsync();
                    //if user selects 'yes'
                    if (command == yes)
                    {
                        //delete selected employee from database
                        bool success = App.MY_USERVIEWMODEL.DeleteUser(selectedEmployee);
                        //if employee cannot be deleted from databse
                        if (success == false)
                        {
                            //selected employee is set to unemployed 
                            selectedEmployee.IsEmployed = false;
                            //update employee's employment status
                            App.MY_USERVIEWMODEL.UpdateEmploymentStatus(selectedEmployee);
                        }

                        Clear_Fields();
                        Get_Employees();
                    }
                }
                
            }
            else
            {
                //display error message
                dialog = new MessageDialog("Select an employee to remove", "Select an Employee");
                await dialog.ShowAsync();
            }
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog;
            //if an item is selected in listEmployees
            if (listEmployees.SelectedIndex != -1)
            {
                Employee selectedEmployee = (Employee)listEmployees.SelectedItem;
                //if there are empty fields
                if(txtFirstName.Text == "" || txtLastName.Text == "" || passwordEmp.Password.ToString() == "")
                {
                    //display error messafe
                    dialog = new MessageDialog("Please fill all fields", "Fill All Fields");
                    await dialog.ShowAsync();
                }
                //if password is less than 6 characters
                else if (passwordEmp.Password.ToString().Length < 6)
                {
                    //display error message
                    dialog = new MessageDialog("Password should be at least 6 characters long", "Invalid Password");
                    await dialog.ShowAsync();
                }
                else
                {
                        //if selected employee is the same as the logged employee and isManager is set to false
                        if (App.employeeLogged.UserID == selectedEmployee.UserID && comboIsManager.SelectedIndex == 1)
                        {
                                //display error message
                                dialog = new MessageDialog("Cannot change your management status\nPlease ask another manager to change your management status", "Changing Management Status");
                                await dialog.ShowAsync();                            
                        }
                        else
                        {
                            //set required attributes
                            selectedEmployee.FirstName = txtFirstName.Text;
                            selectedEmployee.LastName = txtLastName.Text;
                            selectedEmployee.Password = passwordEmp.Password.ToString();
                            if (comboIsManager.SelectedIndex == 0)
                            {
                                selectedEmployee.IsManager = true;
                            }
                            else
                            {
                                selectedEmployee.IsManager = false;
                            }

                        //if selected employee is the same as the logged employee
                        if (App.employeeLogged.UserID == selectedEmployee.UserID)
                            {
                                //set the logged employee to the selected employee
                                App.employeeLogged = selectedEmployee;
                                //redisplay employee name
                                txtEmployeeName.Text = App.employeeLogged.EmployeeName();
                            }
                            //update employee in database
                            App.MY_USERVIEWMODEL.UpdateEmployee(selectedEmployee);
                            Clear_Fields();
                            Get_Employees();

                        }

                }
                

            }
            else
            {
                dialog = new MessageDialog("Select an employee to update", "Select Employee");
                await dialog.ShowAsync();
            }
        }

        private async void BtnViewPrevEmployees_Click(object sender, RoutedEventArgs e)
        {
            //display Previous Employees COntent Dialog
            PreviousEmployees previousEmployees = new PreviousEmployees();
            await previousEmployees.ShowAsync();
            
           while (previousEmployees.success == false)
            {
                await previousEmployees.ShowAsync();
            }
           //get employees
            Get_Employees();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //navigate to Employee Main Menu
            this.Frame.Navigate(typeof(EmployeeMainMenu));
        }
    }
}
