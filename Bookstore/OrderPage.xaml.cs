using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bookstore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {
        //make a temporary order items list
        List<Item> orderItems;
        public OrderPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //retrieve items items from the database
            App.MY_ITEMVIEWMODEL.GetItems();
            //get items that are still in the inventory
            var items = from item in App.MY_ITEMVIEWMODEL.AllItems
                        where item.InInventory == true
                        select item;
            //populate the gridview
            gridItems.ItemsSource = items;
            //
            orderItems = new List<Item>();
            //display a textbox
            txtLeft.Visibility = Visibility.Collapsed;

            //if an order has been picked 
            if (App.currentOrder != null)
            {
                //and an employee is logged in
                if(App.employeeLogged != null)
                {
                    //loop through the current order's items
                    foreach (Item i in App.currentOrder.OrderItems)
                    {
                        //add unique items to the temporary order items list
                        if (orderItems.IndexOf(i) == -1)
                        {
                            orderItems.Add(i);
                        }
                    }
                }
                //else if a customer has accessed this order
                else if(App.customerLogged != null)
                {
                    //loop through the current order
                    foreach (Item i in App.currentOrder.OrderItems)
                    {
                        //increase the number of each item in the order
                        i.NumInEachOrder++;
                        //add unique items to the temporary order items list
                        if (orderItems.IndexOf(i) == -1)
                        {
                            orderItems.Add(i);
                        }
                    }
                }

                //populate the listOrderItems with the temporary order items list
                listOrderItems.ItemsSource = orderItems;
                //get the total price and total number of items in the order
                Get_Totals();
                //display the current order ID
                txtOrderNum.Text = App.currentOrder.OrderID.ToString();
                //change the text on the buttons
                btnConfirmOrder.Content = "Update Order";
                btnCancelOrder.Content = "Cancel Update";
            }
            else
            {
                //get the total price and total number of items in the order
                Get_Totals();
                //get the current order number
                txtOrderNum.Text = (Get_Last_OrderID() + 1).ToString();
            }

        }

        public int Get_Last_OrderID()
        {
            int lastOrder;
            //retrieve orders from the database
            App.MY_ORDERVIEWMODEL.GetOrders();
            //if there are previous orders
            if (App.MY_ORDERVIEWMODEL.AllOrders.Count != 0)
            {
                //get the the last order ID
                lastOrder = App.MY_ORDERVIEWMODEL.AllOrders.Max(o => o.OrderID);
            }
            else
            {
                //set the last order ID to 0
                lastOrder = 0;
            }

            
            return lastOrder;
        }

        private void Refresh_NumInOrder()
        {
            //loop through all items 
            foreach(Item i in App.MY_ITEMVIEWMODEL.AllItems)
            {
                //reset the NumInEachOrder value to 0
                i.ResetItemInOrder();
            }
        }


        private void Get_Totals()
        {
            double total = 0;
            int numItems = 0;
            //if the temporary order items list is not empty
            if(orderItems.Count != 0)
            {
                //loop through the list
                foreach(Item i in orderItems)
                {
                    //increase the total price
                    total += (i.Price * i.NumInEachOrder);
                    //increase the total number of items
                    numItems += i.NumInEachOrder;
                }

            }

            //display the values
            txtNumItems.Text = numItems.ToString();
            txtTotalAmount.Text = total.ToString("c");
          
        }

        private void Filter_ItemTypes(string type)
        {
            IEnumerable<Item> query;
            //if the the item type is all
            if (type == "all")
            {
                //get all items that are in the inventory
                query = from item in App.MY_ITEMVIEWMODEL.AllItems
                            where item.InInventory == true
                            select item;
            }
            else
            {
                //else get all items that are in the inventory based on the item type parsed
                query = from item in App.MY_ITEMVIEWMODEL.AllItems
                            where item.InInventory == true &&
                            item.ItemType == type
                            select item;
            }

            //repopulate the gridview
            gridItems.ItemsSource = null;
            gridItems.ItemsSource = query;
        }

        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            //cast the selected button on the grid item item as an Item
            Item selectedItem = (sender as FrameworkElement).Tag as Item;

            //if the NumInEachOrder of the selected item is less than the item's stock
            if (selectedItem.NumInEachOrder < selectedItem.Stock)
            {
                //increase the NumInEachOrder
                selectedItem.NumInEachOrder++;

                //if the temporary order items list does not have this item
                if(orderItems.IndexOf(selectedItem) == -1)
                {
                    //add the item to the list
                    orderItems.Add(selectedItem);
                }
                //repopulate listOrderItems
                listOrderItems.ItemsSource = null;
                listOrderItems.ItemsSource = orderItems;
            }
            //get the total price and total number of items
            Get_Totals();
        }

        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            //Cast the selected remove button on the listitem as an Item
            Item selectedItem = (sender as FrameworkElement).Tag as Item;
            //reset the NumInEachOrder of the item to 0
            selectedItem.ResetItemInOrder();
            //remove the item from the temporary order items list
            orderItems.Remove(selectedItem);
            //repopulate the listOrderItems
            listOrderItems.ItemsSource = null;
            listOrderItems.ItemsSource = orderItems;
            //get the total price and total number of items
            Get_Totals();

        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            //cast the selected minus button on the list item as an Item
            Item selectedItem = (sender as FrameworkElement).Tag as Item;
            //if the item's NumInEachOrder is not equals to 1
            if(selectedItem.NumInEachOrder != 1)
            {
                //minus the item's NumInEachOrder by 1
                selectedItem.NumInEachOrder--;
            }
            else
            {
                //minus the item's NumInEachOrder by 1
                selectedItem.NumInEachOrder--;
                //remove the selected item from the temporary order items list
                orderItems.Remove(selectedItem);
            }

            //repopulate the listOrderItems
            listOrderItems.ItemsSource = null;
            listOrderItems.ItemsSource = orderItems;
            //get the total price and total items
            Get_Totals();

        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            //cast the selected plus button on the list item as an Item
            Item selectedItem = (sender as FrameworkElement).Tag as Item;
            //if the selected item's NumInEachOrder is not equals or more than its stock
            if (selectedItem.NumInEachOrder != selectedItem.Stock && !(selectedItem.NumInEachOrder > selectedItem.Stock))
            {
                //add the item's NumInEachOrder by 1
                selectedItem.NumInEachOrder++;
            }
            //repopulate listOrderItems
            listOrderItems.ItemsSource = null;
            listOrderItems.ItemsSource = orderItems;
            //get the total price and number of items
            Get_Totals();

        }

        private void BorderStat_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set the borderStat background colour when hovered over
            borderStat.Background = (SolidColorBrush)Resources["LightSeaGreen"];
        }

        private void BorderStat_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set the borderStat background colour when hovered off
            borderStat.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BorderMag_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set the borderMag background colour when hovered over
            borderMag.Background = (SolidColorBrush)Resources["LightSeaGreen"];
        }

        private void BorderMag_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set the borderMag background colour when hovered off
            borderMag.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BorderBook_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set the borderBook background colour when hovered over
            borderBook.Background = (SolidColorBrush)Resources["LightSeaGreen"];
        }

        private void BorderBook_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set the borderBook background colour when hovered off
            borderBook.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BorderAll_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set the borderAll background colour when hovered over
            borderAll.Background = (SolidColorBrush)Resources["LightSeaGreen"];
        }

        private void BorderAll_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set the borderAll background colour when hovered off
            borderAll.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BtnTxtAllItems_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //populate gridview with all items
            Filter_ItemTypes("all");
        }

        private void BtnTxtBooks_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //populate gridview with book items
            Filter_ItemTypes("book");
        }

        private void BtnTxtMag_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //populate gridview with magazine items
            Filter_ItemTypes("magazine");
        }

        private void BtnTxtStat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //populate gridview with stationery items
            Filter_ItemTypes("stationery");
        }

        private void GridItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cast the selected grid item as an Item
            Item selectedItem = (Item)e.ClickedItem;
            Book selectedBook;
            Magazine selectedMag;
            Stationery selectedStat;

            //display relative information of the item
            txtName.Text = selectedItem.ItemName;
            txtDesc.Text = selectedItem.Description;
            txtPrice.Text = selectedItem.Price.ToString("c");
            txtStock.Text = selectedItem.Stock.ToString();
            txtLeft.Visibility = Visibility.Visible;

            //display the item's image
            BitmapImage itemImage = new BitmapImage();
            itemImage.UriSource = new Uri("ms-appx://" + selectedItem.Picture);
            imgItem.Source = itemImage;

            //if the selected item is a Book
            if (selectedItem is Book)
            {
                //get the book item and display the relative fields
                selectedBook = (Book)selectedItem;
                txtAuPubCol.Text = selectedBook.Author;
                txtGenre.Text = selectedBook.Genre;
            }
            else if(selectedItem is Magazine)
            {
                //get the magazine item and display the relative fields
                selectedMag = (Magazine)selectedItem;
                txtAuPubCol.Text = selectedMag.Publisher;
                txtGenre.Text = "";
            }
            else if(selectedItem is Stationery)
            {
                //get the magazine item and display the relative fields
                selectedStat = (Stationery)selectedItem;
                txtAuPubCol.Text = selectedStat.Colour;
                txtGenre.Text = "";
            }
        }

        private async void BtnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog d;
            //if an employee is logged or customer is updating an order
            if(App.employeeLogged != null || App.customerLogged != null)
            {
                //display confirmation dialog
                d = new MessageDialog("Cancel updates to order?", "Cancel Order Update");
                var yes = new UICommand("Yes");
                var no = new UICommand("No");
                d.Commands.Add(yes);
                d.Commands.Add(no);

                var command = await d.ShowAsync();
                //if user selects 'yes'
                if(command == yes)
                {
                    //if an employee is logged
                    if(App.employeeLogged != null)
                    {
                        //navigate back to the Sale Page
                        this.Frame.Navigate(typeof(SalePage));
                    }
                    //if a customer is logged
                    else if(App.customerLogged != null)
                    {
                        //set logged customer null
                        App.customerLogged = null;
                        //set current order to null
                        App.currentOrder = null;
                        //navigate to the Main Menu
                        this.Frame.Navigate(typeof(MainMenu));
                    }
                }
            }
            else
            {
                //display a confirmation dialog
                d = new MessageDialog("Are you sure you want to cancel your order?", "Cancel Order?");
                var yes = new UICommand("Yes");
                var no = new UICommand("No");
                d.Commands.Add(yes);
                d.Commands.Add(no);

                var command = await d.ShowAsync();
                //if user selects 'yes'
                if (command == yes)
                {
                    //navigate to the Main Menu
                    this.Frame.Navigate(typeof(MainMenu));
                }
            }
            
        }

        private async void BtnConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog;
            //if the temporary order items list is not empty
            if(orderItems.Count != 0)
            {
                //if no employee or customer is logged
                if(App.employeeLogged != null || App.customerLogged != null)
                {
                    //default proceed value is true
                    bool proceed = true;
                    //loop through the list
                    foreach(Item item in orderItems)
                    {
                        //if the NumInEachOrder of an item is more than its stock
                        if(item.NumInEachOrder > item.Stock)
                        {
                            //set proceed to false
                            proceed = false;
                        }
                        
                    }
                    //if proceed is true
                   if(proceed == true)
                    {
                        //set the current order's order items to the temporary order items list
                        App.currentOrder.OrderItems = orderItems;
                        //update the current order in the database
                        App.MY_ORDERVIEWMODEL.UpdateOrder(App.currentOrder);
                        //if the employee is logged
                        if(App.employeeLogged != null)
                        {
                            //display message
                            dialog = new MessageDialog("Order has been updated", "Order Updated");
                            await dialog.ShowAsync();
                            //navigate to the Sale Page
                            this.Frame.Navigate(typeof(SalePage));
                        }
                        //if the customer is logged
                        else if (App.customerLogged != null)
                        {
                            //display message
                            dialog = new MessageDialog("Your order has been updated.\nPlease proceed to the counter to make your purchase", "Order Updated");
                            await dialog.ShowAsync();
                            //set logged customer and current order to null
                            App.customerLogged = null;
                            App.currentOrder = null;
                            //navigate to the Main Menu
                            this.Frame.Navigate(typeof(MainMenu));
                        }
                    }
                   else
                    {
                        //display error message
                        dialog = new MessageDialog("One or more of your order items is more than the stock we have.\nPlease change the number of order items in your cart.", "Cannot Update Order");
                        await dialog.ShowAsync();
                    }

                }
                else
                {
                    //get the current order ID
                    int currentID = Int32.Parse(txtOrderNum.Text);
                    //create a new order
                    App.currentOrder = new Order(currentID, "", new Customer(), false, orderItems);
                    //display the Add Customer Details Content Dialog
                    AddCustomerDetails customerDetails = new AddCustomerDetails();
                    await customerDetails.ShowAsync();
                }

            }
            else
            {
                //display error message
                dialog = new MessageDialog("Please select items you'd like to purchase", "Select Item");
                await dialog.ShowAsync();
            }
        }
    }
}
