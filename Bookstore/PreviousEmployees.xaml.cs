using Bookstore.Classes;
using System;
using System.Collections.Generic;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bookstore
{
    public sealed partial class PreviousEmployees : ContentDialog
    {
        //make a public success variable
        public bool success;
        public PreviousEmployees()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //get all employees that are not employed
            App.MY_USERVIEWMODEL.GetEmployees();
            listPrevEmployees.ItemsSource = from employee in App.MY_USERVIEWMODEL.AllEmployees
                                            where employee.IsEmployed == false
                                            select employee;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //set success to false by default
            success = false;
            MessageDialog dialog;
            //if at least one employee is selected
            if(listPrevEmployees.SelectedIndex != -1)
            {
                //loop through the selected employees
                foreach (object e in listPrevEmployees.SelectedItems)
                {
                    //cast Employee on selected items
                    Employee emp = (Employee)e;
                    //set the employee's isEmployed to true
                    emp.IsEmployed = true;
                    //update the employee's employement status
                    App.MY_USERVIEWMODEL.UpdateEmploymentStatus(emp);
                }
                //set success to true
                success = true;

            }
            else
            {
                //display error message
                dialog = new MessageDialog("Please select an employee", "Select Employee");
                await dialog.ShowAsync();
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //set success to true
            success = true;
        }

    }
}
