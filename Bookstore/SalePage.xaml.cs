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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bookstore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SalePage : Page
    {
        double totalPrice;
        bool proceedToProcess;
        Customer customer;
        List<Item> tempList;
        string itemList;
        public SalePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //display logged employee's ID and name
            txtEmployeeID.Text = App.employeeLogged.UserID.ToString();
            txtEmployeeName.Text = App.employeeLogged.EmployeeName();
            tempList = new List<Item>();
            //retrieve users from the database
            App.MY_USERVIEWMODEL.GetUsers();
            //populate listBoxOrders and listStock
            Refresh_Lists();

            //if the current order is not null
            if (App.currentOrder != null)
            {
                //create a temporary orders list
                List<Order> tempOrders = new List<Order>();

                //loop through the orders in listBoxOrders
                foreach (Object o in listBoxOrders.Items)
                {
                    //add the order to the temporary orders list
                    tempOrders.Add((Order)o);
                }

                //get the index of the order that has the same ID as the current order
                int index = tempOrders.FindIndex(o => o.OrderID == App.currentOrder.OrderID);
                //select the order in listBoxOrders based on the index
                listBoxOrders.SelectedIndex = index;

            }

        }

        private void Refresh_Lists()
        {
            //retrieve orders and items from the database
            App.MY_ORDERVIEWMODEL.GetOrders();
            App.MY_ITEMVIEWMODEL.GetItems();

            //get orders that are incomplete
            var orders = from order in App.MY_ORDERVIEWMODEL.AllOrders
                         where order.IsComplete == false
                         select order;

            //get items that are in the inventory
            var items = from item in App.MY_ITEMVIEWMODEL.AllItems
                        where item.InInventory == true
                        select item;
            //populate listBoxOrders and listStock
            listBoxOrders.ItemsSource = orders;
            listStock.ItemsSource = items;

        }

        private int Get_LastSaleID()
        {
            int lastSale;
            //retrieve sales from the database
            App.MY_SALEVIEWMODEL.GetSales();
            //if there are previous sales made
            if(App.MY_SALEVIEWMODEL.AllSales.Count != 0)
            {
                //get the last sale ID
                lastSale = App.MY_SALEVIEWMODEL.AllSales.Max(i => i.SaleID);
            }
            else
            {
                //set the last sale ID to 0
                lastSale = 0;
            }
            
            return lastSale;
        }

        private void ListBoxOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if an item is selected
            if (listBoxOrders.SelectedIndex != -1)
            {
                //clear the listOrderDetails
                listOrderDetails.Items.Clear();
                //set current order to the selected item
                App.currentOrder = (Order)listBoxOrders.SelectedItem;

                customer = new Customer();
                string text = "";
                itemList = "";
                totalPrice = 0;
                //set proceedToProcess to true
                proceedToProcess = true;

                //if temporary list is not empty
                if(tempList.Count != 0)
                {
                    //loop through each item in the temporary list
                    foreach (Item i in tempList)
                    {
                        //reset the item's NumInEachOrder to 0
                        i.ResetItemInOrder();
                    }
                }

                //empty the temporary list
                tempList = new List<Item>();
                
                //loop through the current order's items
                foreach(Item i in App.currentOrder.OrderItems)
                {
                    //increase the total proce
                    totalPrice += i.Price;

                    //is the temporary list does not have the item
                    if(!tempList.Contains(i))
                    {
                        //add the item
                        tempList.Add(i);
                    }
                    //increase the item's NumInEachOrder
                    i.NumInEachOrder++;
                }

                //loop through the temporary list of items
                foreach(Item i in tempList)
                {
                    //loop through all the items from the database
                    foreach(Item item in App.MY_ITEMVIEWMODEL.AllItems)
                    {
                        //if the item in the temporary list is equals the item from the database
                        if (i.ItemID == item.ItemID)
                        {
                            //if the item's NumInEachOrder is more than its stock
                            if(i.NumInEachOrder > item.Stock)
                            {
                                //set proceedToProcess to false
                                proceedToProcess = false;
                                //display text of item that does not have enough stock
                                itemList += i.ItemName + " " + i.PriceDisplay() + "\t" + i.NumInEachOrder + " (Not Enough Stock)\n";
                            }
                            //if item is not in inventory
                            else if (i.InInventory == false)
                            {
                                //set proceedToProcess to false
                                proceedToProcess = false;
                                //display text of item that is not being sold
                                itemList += i.ItemName + " " + i.PriceDisplay() + "\t" + i.NumInEachOrder + " (Item is not being sold)\n";
                            }
                            else
                            {
                                //display item
                                itemList += i.ItemName + " " + i.PriceDisplay() + "\t" + i.NumInEachOrder + "\n";
                            }
                        }
                    }
                }

                //get order details
                text = "Order Number: " + App.currentOrder.OrderID + "\nCustomer: " + App.currentOrder.Customer.FirstName + " " + App.currentOrder.Customer.LastName
                        +"\n\n"+ itemList + "\nTotal Items: " + App.currentOrder.TotalItems().ToString() + "\nTotal Amount: " + totalPrice;
                //display order details in the list
                listOrderDetails.Items.Add(text);               
            }
        }

        private void Btn0_Click(object sender, RoutedEventArgs e)
        {
            //add 0 to the textblock
            txtAmountPaid.Text += "0";
        }

        private void Btn00_Click(object sender, RoutedEventArgs e)
        {
            //add 00 to the textblock
            txtAmountPaid.Text += "00";
        }

        private void BtnDot_Click(object sender, RoutedEventArgs e)
        {
            //add . to the textblock
            txtAmountPaid.Text += ".";
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //add 01 to the textblock
            txtAmountPaid.Text += "1";
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            //add 2 to the textblock
            txtAmountPaid.Text += "2";
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            //add 3 to the textblock
            txtAmountPaid.Text += "3";
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            //add 4 to the textblock
            txtAmountPaid.Text += "4";
        }

        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            //add 5 to the textblock
            txtAmountPaid.Text += "5";
        }

        private void Btn6_Click(object sender, RoutedEventArgs e)
        {
            //add 6 to the textblock
            txtAmountPaid.Text += "6";
        }

        private void Btn7_Click(object sender, RoutedEventArgs e)
        {
            //add 7 to the textblock
            txtAmountPaid.Text += "7";
        }

        private void Btn8_Click(object sender, RoutedEventArgs e)
        {
            //add 8 to the textblock
            txtAmountPaid.Text += "8";
        }

        private void Btn9_Click(object sender, RoutedEventArgs e)
        {
            //add 9 to the textblock
            txtAmountPaid.Text += "9";
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            //clear textblock
            txtAmountPaid.Text = "";
        }

        private async void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog;
            //get the current sale id
            int currentID = Get_LastSaleID() + 1;
            double amount;
            double change;
            //set default success to true
            bool success = true;

            success = Double.TryParse(txtAmountPaid.Text, out amount);

            //if order is selected
            if(listBoxOrders.SelectedIndex != -1)
            {
                //if no amount is entered
                if (txtAmountPaid.Text == "")
                {
                    //display error message
                    dialog = new MessageDialog("No amount entered.\nPlease enter a valid amount", "Invalid Amount Entered");
                    await dialog.ShowAsync();
                }
                //if succes is false
                else if (success == false)
                {
                    //display error message
                    dialog = new MessageDialog("Invalid value entered", "Invalid Amount Entered");
                    await dialog.ShowAsync();
                }
                //if amount entered is less than the total price
                else if(amount < totalPrice)
                {
                    //display error message
                    dialog = new MessageDialog("Amount entered is not enough", "Invalid Amount Entered");
                    await dialog.ShowAsync();
                }
                //if proceedToProcess is false
                else if(proceedToProcess == false)
                {
                    //display error message
                    dialog = new MessageDialog("Order cannot be processed.\nPlease make changes to the order before making a sale");
                    await dialog.ShowAsync();
                }
                else
                {
                    //add a new sale to the database
                    App.MY_SALEVIEWMODEL.AddNewSale(new Sale(currentID, App.currentOrder, totalPrice, amount, App.employeeLogged));
                    //set current order to complete
                    App.currentOrder.IsComplete = true;
                    //update the current order's status in the database
                    App.MY_ORDERVIEWMODEL.UpdateOrderStatus(App.currentOrder.OrderID);
                    //get the change
                    change = amount - totalPrice;

                    //loop through items in the temporary list of items
                    foreach(Item orderItem in tempList)
                    {
                        //get number of stock after sale
                        int stock = orderItem.Stock - orderItem.NumInEachOrder;
                        //set the item's current stock
                        orderItem.Stock = stock;
                        //update the item in the database
                        App.MY_ITEMVIEWMODEL.UpdateItem(orderItem);
                    }

                    //display message of saledetails
                    dialog = new MessageDialog("Order: " + App.currentOrder.OrderID + "\nCustomer: " + customer.FirstName + " " + customer.LastName + "\n"
                                                + itemList + "\nTotal Items: " + App.currentOrder.OrderItems.Count + "\nTotal Price: " + totalPrice.ToString("c") 
                                                + "\nAmount Paid: " + amount.ToString("c") + "\nChange: " + change.ToString(""), "Sale Successful");
                    await dialog.ShowAsync();

                    //clear listOrderDetails
                    listOrderDetails.Items.Clear();
                    //populate listBoxOrders and listStock
                    Refresh_Lists();
                }
                //clear amount entered
                txtAmountPaid.Text = "";
            }
            //if no item was selected
            else if(listBoxOrders.SelectedIndex == -1)
            {
                //display message
                dialog = new MessageDialog("Please select an order", "No Order Selected");
                await dialog.ShowAsync();
            }
        }

        private async void BtnRemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            //display confirmation dialog
            MessageDialog question = new MessageDialog("Are you sure you want to remove this order?", "Remove Order?");
            var yes = new UICommand("Yes");
            var no = new UICommand("No");
            question.Commands.Add(yes);
            question.Commands.Add(no);

            var command = await question.ShowAsync();
            //if user selects 'yes'
            if (command == yes)
            {
                //if an order is selected
                if (listBoxOrders.SelectedIndex != -1)
                {
                    //Cast Order on the selected order
                    Order currentOrder = (Order)listBoxOrders.SelectedItem;
                    //get the current order's customer
                    var getCustomer = from user in App.MY_USERVIEWMODEL.AllUsers
                                      where user.UserID == currentOrder.Customer.UserID
                                      select user;
                    Customer cust = (Customer)getCustomer.First();

                    //delete order from the database
                    App.MY_ORDERVIEWMODEL.DeleteOrder(currentOrder.OrderID);
                    //delete customer from the database
                    App.MY_USERVIEWMODEL.DeleteUser(cust);

                    //clear listOrderDetails
                    listOrderDetails.Items.Clear();
                    //get users from the database
                    App.MY_USERVIEWMODEL.GetUsers();
                    //populate listBoxOrders and listStock
                    Refresh_Lists();

                }
                else
                {
                    //display error message
                    MessageDialog dialog = new MessageDialog("Please select an order to remove", "No Order Selected");
                    await dialog.ShowAsync();
                }
            }

        }

        private async void BtnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            //if an order is selected
            if(listBoxOrders.SelectedIndex != -1)
            {
                //navigate to Order Page
                this.Frame.Navigate(typeof(OrderPage));
            }
            else
            {
                //display error message
                MessageDialog dialog = new MessageDialog("Please select an order to update", "No Order Selected");
                await dialog.ShowAsync();
            }
            
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //navigate to Employee Main Menu
            this.Frame.Navigate(typeof(EmployeeMainMenu));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //set current order to null
            App.currentOrder = null;
        }
    }
}
