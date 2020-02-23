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
    public sealed partial class AddCustomerDetails : ContentDialog
    {

        public AddCustomerDetails()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //Get all users
            App.MY_USERVIEWMODEL.GetUsers();
        }


        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            MessageDialog d;
            //if fields are not empty
            if (txtFirstName.Text != "" && txtLastName.Text != "" && txtPhone.Text != "" && txtEmail.Text != "" && txtCode.Password.ToString() != "")
            {
                float phone;
                int currentID;
                //check if phone text field consists of numbers
                    if(!float.TryParse(txtPhone.Text, out phone))
                {
                    //display error message
                    d = new MessageDialog("Please enter a valid phone number", "Invalif Phone Number");
                    await d.ShowAsync();
                    //redisplay AddCustomerDetails Content Dialog
                    await this.ShowAsync();
                }
                    //if phone numbe has less than 10 numbers
                    else if (txtPhone.Text.Length < 10)
                {
                    //display error message
                    d = new MessageDialog("Phone number should have at least 10 numbers", "Invalid Phone Number");
                    await d.ShowAsync();
                    //redisplay AddCustomerDetails Content Dialog
                    await this.ShowAsync();
                }
                    //if password has less than 6 characters
                    else if(txtCode.Password.ToString().Length < 6)
                {
                    //display error message
                    d = new MessageDialog("Code should be at least 6 characters long", "Invalid Code");
                    await d.ShowAsync();
                    //redisplay AddCustomerDetails Content Dialog
                    await this.ShowAsync();
                }
                    else
                {
                    string fName = txtFirstName.Text;
                    string lName = txtLastName.Text;
                    string email = txtEmail.Text;
                    string code = txtCode.Password.ToString();

                    string phoneNum = txtPhone.Text;
                    //if there are no users
                    if(App.MY_USERVIEWMODEL.AllUsers.Count == 0)
                    {
                        //the current user id is set to 1
                        currentID = 1;
                    }
                    else
                    {
                        //get the last user id
                        currentID = App.MY_USERVIEWMODEL.AllUsers.Max(u => u.UserID) + 1;
                    }

                    //create a new Customer object with the details 
                    Customer customer = new Customer(currentID, fName, lName, "customer", email, phoneNum);
                    //set the current order's customer
                    App.currentOrder.Customer = customer;
                    //set the current order's code
                    App.currentOrder.Code = code;

                    //add the customer to the database
                    bool success = App.MY_USERVIEWMODEL.AddNewUser(customer);
                    //if the user is successfully added
                    if(success == true)
                    {
                        //add the orderto the database
                        App.MY_ORDERVIEWMODEL.AddNewOrder(App.currentOrder);
                    }

                    //display order details
                    d = new MessageDialog("Your order number is " + App.currentOrder.OrderID + "\nCode: " + App.currentOrder.Code + "\nPlease proceed to the counter to purchase your items");
                    await d.ShowAsync();
                    // set the current order to null
                    App.currentOrder = null;
                    // navigate to the Main Menu Page
                    (Window.Current.Content as Frame)?.Navigate(typeof(MainMenu), null);
                }

            }
            else
            {
                //display error message if not all fields are filled
                d = new MessageDialog("Please fill all fields", "Error");
                await d.ShowAsync();
                //redisplay AddCustomerDetails Content Dialog
                await this.ShowAsync();

            }
        }

    }
}
