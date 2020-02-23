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
    public sealed partial class SalesStatsPage : Page
    {
        public SalesStatsPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //display the logged employees ID and name
            txtEmployeeID.Text = App.employeeLogged.UserID.ToString();
            txtEmployeeName.Text = App.employeeLogged.EmployeeName();
            //retrive sales from the databse
            App.MY_SALEVIEWMODEL.GetSales();
            //populate combobox with dates
            Populate_Dates();
            //populate listSales
            listSales.ItemsSource = App.MY_SALEVIEWMODEL.AllSales;
            //set selected combo item to the first item
            comboDates.SelectedIndex = 0;
        }

        private void Populate_Dates()
        {
            List<string> dates = new List<string>();
            string date;

            //loop through list of sales from the databse
            foreach (Sale s in App.MY_SALEVIEWMODEL.AllSales)
            {
                //get the date of each sale ina  string
                date = s.Date.Day + "-" + s.Date.Month + "-" + s.Date.Year;
                //if the dates list does not have the date string
                if (dates.IndexOf(date) == -1)
                {
                    //add the date string
                    dates.Add(date);
                    //add the date string to the combobox
                    comboDates.Items.Add(date);
                }
            }
        }

        private void ChangeTotal_SalesSummary(string date)
        {
            double totalAmount = 0;
            int numSales = 0;
            txtDate.Text = date;
            //if the date is set to 'All'
            if(date == "All")
            {
                //loop through the sales list from the database
                foreach (Sale s in App.MY_SALEVIEWMODEL.AllSales)
                {
                    //add the totalAmount
                    totalAmount += s.TotalAmount;
                }
                
                //display number of sales made
                txtNumSales.Text = App.MY_SALEVIEWMODEL.AllSales.Count.ToString();

            }
            else
            {
                //loop through the sales list from the database
                foreach (Sale s in App.MY_SALEVIEWMODEL.AllSales)
                {
                    //if the sale's date string is equal to the selected date string
                    if(s.DateString() == date)
                    {
                        //add the total amount
                        totalAmount += s.TotalAmount;
                        //increase the number of sales
                        numSales++;
                    } 
                }
                //display the number of sales
                txtNumSales.Text = numSales.ToString();
            }

            //display the total amount
            txtTotalPriceSales.Text = totalAmount.ToString("c");

        }

        private void Populate_ItemsSold(string date)
        {
            List<Item> itemList = new List<Item>();
            IEnumerable<Sale> salesList;
            int totalItems = 0;
            
            //if the date selected is 'All'
            if (date == "All")
            {
                //set text to 'All Dates'
                txtItemsDate.Text = "All Dates";
                //get all sales 
                salesList = App.MY_SALEVIEWMODEL.AllSales;
            }
            else
            {
                //get the selected date string
                txtItemsDate.Text = date;
                //get all sales on the date
                salesList = from sales in App.MY_SALEVIEWMODEL.AllSales
                               where sales.DateString() == date
                               select sales;
            }

            //loop through the salesList
            foreach (Sale s in salesList)
            {
                //loop through the items in each sale
                foreach (Item i in s.Order.OrderItems)
                {
                    //reset the item's NumInEachOrder to 0
                    i.ResetItemInOrder();
                }
            }

            //loop through the salesList
            foreach (Sale s in salesList)
            {
                //loop through the items in each sale
                foreach(Item i in s.Order.OrderItems)
                {
                    //increase the item's NumInEachOrder
                    i.NumInEachOrder++;
                    //increase total number of items
                    totalItems++;
                    //if item does not exist in itemList
                    if(itemList.IndexOf(i) == -1)
                    {
                        //add item to itemList
                        itemList.Add(i);
                    }
                }
            }

            //get a sorted list of items based on the items' NumInEachOrder in descending order
            List<Item> sortedList = itemList.OrderByDescending(i => i.NumInEachOrder).ToList();

            //if the sortedList has 3 or more items
            if(sortedList.Count >= 3)
            {
                //populate texts with the first three items
                txtFirstPlace.Text = sortedList[0].ItemName;
                txtSecondPlace.Text = sortedList[1].ItemName;
                txtThirdPlace.Text = sortedList[2].ItemName;
            }
            //else if sortedList has 2 items
            else if(sortedList.Count == 2)
            {
                //populate texts with the first two items and the rest with 'None'
                txtFirstPlace.Text = sortedList[0].ItemName;
                txtSecondPlace.Text = sortedList[1].ItemName;
                txtThirdPlace.Text = "None";
            }
            //else if sortedList has 1 item
            else if(sortedList.Count == 1)
            {
                //populate texts with the first item and the rest with 'None'
                txtFirstPlace.Text = sortedList[0].ItemName;
                txtSecondPlace.Text = "None";
                txtThirdPlace.Text = "None";
            }
            //else if sortedList has no items
            else if(sortedList.Count == 0)
            {
                //populate texts with 'None'
                txtFirstPlace.Text = "None";
                txtSecondPlace.Text = "None";
                txtThirdPlace.Text = "None";
            }

            //populate listSoldItems
            listSoldItems.ItemsSource = null;
            listSoldItems.ItemsSource = itemList;
            //display the total number of items
            txtTotalItems.Text = totalItems.ToString();

        }

        private void ComboDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //clear listSaleItem
            listSaleItem.Items.Clear();
            //if 'All' is selected in combobox
            if (comboDates.SelectedIndex == 0)
            {
                //repopulate listSales
                listSales.ItemsSource = null;
                listSales.ItemsSource = App.MY_SALEVIEWMODEL.AllSales;
                //display the total amount and number or sales made
                ChangeTotal_SalesSummary("All");
                //display the items sold
                Populate_ItemsSold("All");
            }
            else
            {
                //get all sales based on selected date
                var getSales = from sale in App.MY_SALEVIEWMODEL.AllSales
                               where sale.DateString() == comboDates.SelectedItem.ToString()
                               select sale;
                //repopulate listSales
                listSales.ItemsSource = null;
                listSales.ItemsSource = getSales;
                //display the total amount and number or sales made based on date selected
                ChangeTotal_SalesSummary(comboDates.SelectedItem.ToString());
                //display the items sold based on the selected date
                Populate_ItemsSold(comboDates.SelectedItem.ToString());
            }
            
    }

        private void ListSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if an item is selected in listSales
            if(listSales.SelectedIndex != -1)
            {
                //clear listSaleItem
                listSaleItem.Items.Clear();
                //add selectedItem to listSaleItem
                listSaleItem.Items.Add(listSales.SelectedItem);
                //cast Sale on selected item
                Sale s = (Sale)listSales.SelectedItem;

            }
            else
            {
                //clear listSaleItem
                listSaleItem.Items.Clear();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //navigate to EmployeeMainMenu
            this.Frame.Navigate(typeof(EmployeeMainMenu));
        }
    }
}
