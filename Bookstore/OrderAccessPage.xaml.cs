using Bookstore.Classes;
using System;
using System.Collections.Generic;
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
    public sealed partial class OrderAccessPage : ContentDialog
    {
        IEnumerable<Order> getOrders;
        public OrderAccessPage()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //get all orders that are not complete
            App.MY_ORDERVIEWMODEL.GetOrders();
            getOrders = from order in App.MY_ORDERVIEWMODEL.AllOrders
                        where order.IsComplete == false
                        select order;
            //get users
            App.MY_USERVIEWMODEL.GetUsers();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            int id;
            string password = passCode.Password.ToString();
            MessageDialog d;
            bool isFound = false;

            //if field order number field is empty and orderID is incorrect
            if (txtOrderNo.Text == "" || Int32.TryParse(txtOrderNo.Text, out id) == false)
            {
                //disply error message
                d = new MessageDialog("Please enter a valid order number", "Invalid Order Number");
                await d.ShowAsync();
                //redisplay Order Access Page Content Dialog
                await this.ShowAsync();
            }
            //if password field is empty
            else if (password == "")
            {
                //display error message
                d = new MessageDialog("Please enter a passcode", "Invalid Pass Code");
                await d.ShowAsync();
                //redisplay Order Access Page Content Dialog
                await this.ShowAsync();
            }
            else
            {
                //loop through orders
                foreach(Order order in getOrders)
                {
                    //if order exists
                    if(id == order.OrderID && password == order.Code)
                    {
                        //isFound is true
                        isFound = true;
                        //set current order
                        App.currentOrder = order;
                    }
                    
                }
                //if isFound is true
                if(isFound == true)
                {
                    //get customer of the order
                    var getCustomer = from user in App.MY_USERVIEWMODEL.AllUsers
                                      where user.UserID == App.currentOrder.Customer.UserID
                                      select user;
                    App.customerLogged = (Customer)getCustomer.First();
                    //display message
                    d = new MessageDialog("Order found!\nWelcome back, " + App.customerLogged.FirstName + " " + App.customerLogged.LastName, "Order Found");
                    await d.ShowAsync();
                    //navigate to Order Page
                    (Window.Current.Content as Frame)?.Navigate(typeof(OrderPage), null);
                }
                else
                {
                    //display error message
                    d = new MessageDialog("Could not find your order.", "Order Not Found");
                    await d.ShowAsync();
                    //redisplay Order Access Page Content Dialog
                    await this.ShowAsync();
                }
            }
        }



    }
}
