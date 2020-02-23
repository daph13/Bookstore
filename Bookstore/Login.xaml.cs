using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Popups;
using Bookstore.Classes;


// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bookstore
{
    public sealed partial class Login : ContentDialog
    {
        IEnumerable<Employee> getEmployees;
        public Login()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //get all employees that are employed
            App.MY_USERVIEWMODEL.GetEmployees();
            getEmployees = from employee in App.MY_USERVIEWMODEL.AllEmployees
                           where employee.IsEmployed == true
                           select employee;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string password = passwordBox.Password.ToString();
            MessageDialog d;
            
            //if fields are empty
            if(firstName == "" || lastName == "" || password == "")
            {
                //display error message
                d = new MessageDialog("Please fill fields", "Fields Not Filled");
                await d.ShowAsync();
                //redisplay the Login Content Dialog
                await this.ShowAsync();
            }
            else
            {
                bool isFound = false;

                foreach(Employee e in getEmployees)
                {
                    //check if an employee with the following details match any of the retrieved employees
                   if(firstName == e.FirstName && lastName == e.LastName && password == e.Password)
                    {
                        //isFound is true
                        isFound = true;
                        //set log employee
                        App.employeeLogged = e;
                    }
                }
                //if isFound is true
                if(isFound == true)
                {
                    //display message
                    d = new MessageDialog("Welcome, " + App.employeeLogged.FirstName + " " + App.employeeLogged.LastName, "Employee Found");
                    await d.ShowAsync();
                    //navigate to Employee Main Menu
                    (Window.Current.Content as Frame)?.Navigate(typeof(EmployeeMainMenu), null);

                }
                else
                {
                    //display error message
                    d = new MessageDialog("Could not find employee", "Employee Not Found");
                    await d.ShowAsync();
                    //redisplay Login content dialog
                    await this.ShowAsync();
                }
            }

        }

     
    }
}
