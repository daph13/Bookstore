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
    public sealed partial class RemovedItems : ContentDialog
    {
        //make a public success variable
        public bool success;

        public RemovedItems()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // set success to false by default
            success = false;
            MessageDialog dialog;
            //if at least one item is selected
            if (listRemovedItems.SelectedIndex != -1)
            {
                //loop through the selected items
                foreach (object i in listRemovedItems.SelectedItems)
                {
                    //cast the item as an Item
                    Item item = (Item)i;
                    //set the items inInventory to true
                    item.InInventory= true;
                    //update the item's status in the database
                    App.MY_ITEMVIEWMODEL.UpdateItemStatus(item);
                }
                //set success to true
                success = true;

            }
            else
            {
                //display error message
                dialog = new MessageDialog("Please select an item", "Select Item");
                await dialog.ShowAsync();
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //set success to true
            success = true;
        }

        private void ListRemovedItems_Loaded(object sender, RoutedEventArgs e)
        {
            //get all items that are not in the inventory
            App.MY_ITEMVIEWMODEL.GetItems();
            listRemovedItems.ItemsSource = from item in App.MY_ITEMVIEWMODEL.AllItems
                                          where item.InInventory == false
                                          select item;
        }
    }
}
