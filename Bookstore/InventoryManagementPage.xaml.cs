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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bookstore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InventoryManagementPage : Page
    {
        private ObservableCollection<Item> allItems = new ObservableCollection<Item>();
        private string currentItemType = "";
        private string currentImage = "";

        public InventoryManagementPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //display logged employee's ID and name in textblocks
            txtEmployeeID.Text = App.employeeLogged.UserID.ToString();
            txtEmployeeName.Text = App.employeeLogged.EmployeeName();
            //refresh view
            Refresh_View();
            //set default image
            currentImage = "/Images/image-default.png";
            //set default item type to book
            currentItemType = "book";
            //change fields display
            Change_Fields(currentItemType);

        }

        private void Refresh_View()
        {
            //get all items that are in inventory
            App.MY_ITEMVIEWMODEL.GetItems();
            var query = from item in App.MY_ITEMVIEWMODEL.AllItems
                        where item.InInventory == true
                        select item;
            //populate listInventory
            listInventory.ItemsSource = query;
            //clear fields
            Clear_Fields();
            //get number of stock
            Get_NumStock();
        }

        private void Get_NumStock()
        {
            //get all items
            App.MY_ITEMVIEWMODEL.GetItems();
            //get total number of books
            var getNumBooks = (from item in App.MY_ITEMVIEWMODEL.AllItems
                               where item.ItemType == "book" &&
                               item.InInventory == true
                               select item.Stock).Sum();
            //get total number of magazines
            var getNumMags = (from item in App.MY_ITEMVIEWMODEL.AllItems
                               where item.ItemType == "magazine" &&
                               item.InInventory == true
                              select item.Stock).Sum();
            //get total number of stationery
            var getNumStats = (from item in App.MY_ITEMVIEWMODEL.AllItems
                               where item.ItemType == "stationery" &&
                               item.InInventory == true
                               select item.Stock).Sum();
            //get total stock
            var getTotalNumStock = (from item in App.MY_ITEMVIEWMODEL.AllItems where item.InInventory == true
                           select item.Stock).Sum();
            //display stock
            txtNumBooks.Text = getNumBooks.ToString();
            txtNumMags.Text = getNumMags.ToString();
            txtNumStats.Text = getNumStats.ToString();
            txtTotalStock.Text = getTotalNumStock.ToString();

        }

        private int Get_Last_ID()
        {
            //get all items
            App.MY_ITEMVIEWMODEL.GetItems();
            int lastItem;
            //if there are no items
            if(App.MY_ITEMVIEWMODEL.AllItems.Count == 0)
            {
                //set item ID to 0
                lastItem = 0;
            }
            else
            {
                //get last ID
                lastItem = App.MY_ITEMVIEWMODEL.AllItems.Max(i => i.ItemID);
            }

            return lastItem;
        }

        private void Change_Fields(string itemType)
        {
            //change visibility and names of fields based on itemType
            if(itemType == "book")
            {
                lblGenre.Visibility = Visibility.Visible;
                txtGenre.Visibility = Visibility.Visible;
                lblAuPuCol.Text = "Author";
                txtItemType.Text = "Book";
            }
            else if(itemType == "magazine")
            {
                lblGenre.Visibility = Visibility.Collapsed;
                txtGenre.Visibility = Visibility.Collapsed;
                lblAuPuCol.Text = "Publisher";
                txtItemType.Text = "Magazine";
            }
            else if (itemType == "stationery")
            {
                lblGenre.Visibility = Visibility.Collapsed;
                txtGenre.Visibility = Visibility.Collapsed;
                lblAuPuCol.Text = "Colour";
                txtItemType.Text = "Stationery";
            }
        }

        private void Clear_Fields()
        {
            // clear fields
            txtID.Text = "";
            txtName.Text = "";
            txtDesc.Text = "";
            txtStock.Text = "";
            txtPrice.Text = "";
            txtAuPuCol.Text = "";
            txtGenre.Text = "";
            //set to default image
            currentImage = "/Images/image-default.png";
            BitmapImage itemImage = new BitmapImage();
            itemImage.UriSource = new Uri("ms-appx://" + currentImage);
            imgItem.Source = itemImage;

        }

        private void BtnAll_Click(object sender, RoutedEventArgs e)
        {
            //get all items that are in inventory
            var query = from item in App.MY_ITEMVIEWMODEL.AllItems
                        where item.InInventory == true
                        select item;
            //deselect item in listInventory
            listInventory.SelectedIndex = -1;
            //repopulate listInventory
            listInventory.ItemsSource = query;
            //clear fields
            Clear_Fields();
            //set item type to book
            currentItemType = "book";
            //change fields
            Change_Fields(currentItemType);
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            //get all books that are in inventory
            var getBooks = from item in App.MY_ITEMVIEWMODEL.AllItems
                           where item.ItemType == "book" &&
                           item.InInventory == true
                           select item;
            listInventory.SelectedIndex = -1;
            listInventory.ItemsSource = getBooks;
            Clear_Fields();
            //set item type to book
            currentItemType = "book";
            //change fields
            Change_Fields(currentItemType);
        }

        private void BtnMagazine_Click(object sender, RoutedEventArgs e)
        {
            //get all magazines that are in inventory
            var getMagazines = from item in App.MY_ITEMVIEWMODEL.AllItems
                           where item.ItemType == "magazine" &&
                           item.InInventory == true
                           select item;
            listInventory.SelectedIndex = -1;
            listInventory.ItemsSource = getMagazines;
            Clear_Fields();
            //set item type to magazine
            currentItemType = "magazine";
            //change fields
            Change_Fields(currentItemType);
        }

        private void BtnStationery_Click(object sender, RoutedEventArgs e)
        {
            //get all stationery that are in inventory
            var getStationery = from item in App.MY_ITEMVIEWMODEL.AllItems
                           where item.ItemType == "stationery" &&
                           item.InInventory == true
                           select item;
            listInventory.SelectedIndex = -1;
            listInventory.ItemsSource = getStationery;
            Clear_Fields();
            //set item type to stationery
            currentItemType = "stationery";
            //change fields
            Change_Fields(currentItemType);
        }

        private void ListInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if item is selected
            if(listInventory.SelectedIndex != -1)
            {
                //cast Item on selected item
                Item selectedItem = (Item)listInventory.SelectedItem;
                //change fields
                Change_Fields(selectedItem.ItemType);
                //set item type
                currentItemType = selectedItem.ItemType;
                //set image
                BitmapImage itemImage = new BitmapImage();
                itemImage.UriSource = new Uri("ms-appx://" + selectedItem.Picture);
                currentImage = selectedItem.Picture;
                imgItem.Source = itemImage;
                //set fields based on item
                txtID.Text = selectedItem.ItemID.ToString();
                txtName.Text = selectedItem.ItemName;
                txtDesc.Text = selectedItem.Description;
                txtStock.Text = selectedItem.Stock.ToString();
                txtPrice.Text = selectedItem.Price.ToString();
                
                //if item is a book
                if(selectedItem is Book)
                {
                    Book selectedBook = (Book)listInventory.SelectedItem;
                    currentItemType = "book";
                    Change_Fields(currentItemType);
                    txtAuPuCol.Text = selectedBook.Author;
                    txtGenre.Text = selectedBook.Genre;
   
                }
                //if item is a magazine
                else if(selectedItem is Magazine)
                {
                    Magazine selectedMag = (Magazine)listInventory.SelectedItem;
                    currentItemType = "magazine";
                    Change_Fields(currentItemType);
                    txtAuPuCol.Text = selectedMag.Publisher;
                }
                //if item is a stationery
                else if(selectedItem is Stationery)
                {
                    Stationery selectedStat = (Stationery)listInventory.SelectedItem;
                    currentItemType = "stationery";
                    Change_Fields(currentItemType);
                    txtAuPuCol.Text = selectedStat.Colour;
                }

            }
        }

        private void BtnAddBook_Click(object sender, RoutedEventArgs e)
        {
            //set required fields related to book
            txtID.Text = "";
            currentItemType = "book";
            Change_Fields(currentItemType);
            Clear_Fields();
        }

        private void BtnAddMagazine_Click(object sender, RoutedEventArgs e)
        {
            //set required fields related to magazine
            txtID.Text = "";
            currentItemType = "magazine";
            Change_Fields(currentItemType);
            Clear_Fields();
        }

        private void BtnAddStationery_Click(object sender, RoutedEventArgs e)
        {
            //set required fields related to stationery
            txtID.Text = "";
            currentItemType = "stationery";
            Change_Fields(currentItemType);
            Clear_Fields();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //get current ID
            int id = Get_Last_ID() + 1;
            string name = txtName.Text;
            string description = txtDesc.Text;
            int stock;
            double price;
            bool success1;
            bool success2;
            Book newBook;
            Magazine newMag;
            Stationery newStat;
            MessageDialog dialog;

            success1 = Int32.TryParse(txtStock.Text, out stock);
            success2 = Double.TryParse(txtPrice.Text, out price);
            //if fields are empty
            if(txtName.Text == string.Empty || txtDesc.Text == string.Empty || txtStock.Text == string.Empty || txtPrice.Text == string.Empty || txtAuPuCol.Text == string.Empty)
            {
                //display error message
                dialog = new MessageDialog("Please fill all fields", "Fill All Fields");
                await dialog.ShowAsync();
            }
            //if item is a book and genre field is empty
            else if (currentItemType == "book" && txtGenre.Text == string.Empty)
            {
                //display error message
                dialog = new MessageDialog("Please fill all fields", "Fill All Fields");
                await dialog.ShowAsync();
            }
            //if stock is not correct
            else if(success1 == false)
            {
                //display error message
                dialog = new MessageDialog("Please enter a valid stock amount", "Invalid Stock");
                await dialog.ShowAsync();
            }
            //if price is not correct
            else if(success2 == false)
            {
                //display error message
                dialog = new MessageDialog("Please enter a valid price", "Invalid Price");
                await dialog.ShowAsync();
            }
            else
            {
                //if item is a book
                if (currentItemType == "book")
                {
                    string author = txtAuPuCol.Text;
                    string genre = txtGenre.Text;

                    newBook = new Book(id, name, description, stock, currentItemType, currentImage, price, author, genre);
                    //add book to database
                    App.MY_ITEMVIEWMODEL.AddNewItem(newBook);
                }
                //if item is a magazine
                else if (currentItemType == "magazine")
                {
                    string publisher = txtAuPuCol.Text;

                    newMag = new Magazine(id, name, description, stock, currentItemType, currentImage, price, publisher);
                    //add magazine to the database
                    App.MY_ITEMVIEWMODEL.AddNewItem(newMag);

                }
                //if item is a stationery
                else if (currentItemType == "stationery")
                {
                    string colour = txtAuPuCol.Text;

                    newStat = new Stationery(id, name, description, stock, currentItemType, currentImage, price, colour);
                    //add stationery to the database
                    App.MY_ITEMVIEWMODEL.AddNewItem(newStat);

                }
                //refresh view
                Refresh_View();
            }
           
        }


        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            //clear fields
            Clear_Fields();
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog;
            //if item is selected
            if (listInventory.SelectedIndex != -1)
            {
                //cast Item to selected item
                Item selectedItem = (Item)listInventory.SelectedItem;
                dialog = new MessageDialog("Remove this Item?", "Remove Item");
                bool success = false;
                var yes = new UICommand("Yes");
                var no = new UICommand("No");
                dialog.Commands.Add(yes);
                dialog.Commands.Add(no);

                var command = await dialog.ShowAsync();
               
                if(command == yes)
                {
                    //remove item from database
                    success =  App.MY_ITEMVIEWMODEL.DeleteItem(selectedItem.ItemID, selectedItem.ItemType);
            
                    //if successfully removed
                    if (success == true)
                    {
                        //display message
                        dialog = new MessageDialog("Successfully removed item");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        //set items inInventory to false
                        selectedItem.InInventory = false;
                        //update item's status in database
                        App.MY_ITEMVIEWMODEL.UpdateItemStatus(selectedItem);
                        //display message
                        dialog = new MessageDialog("Successfully removed item");
                        await dialog.ShowAsync();
                    }
                    //refresh view
                    Refresh_View();

                }

            }
            else
            {
                //display error message
                dialog = new MessageDialog("Please select an item to delete", "Select Item");
                await dialog.ShowAsync();
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog;
            //if item is selected
            if (listInventory.SelectedIndex != -1)
            {
                int id = Int32.Parse(txtID.Text);
                string name = txtName.Text;
                string description = txtDesc.Text;
                int stock;
                double price;
                Book updatedBook;
                Magazine updatedMag;
                Stationery updatedStat;
                bool success1;
                bool success2;

                success1 = Int32.TryParse(txtStock.Text, out stock);
                success2 = Double.TryParse(txtPrice.Text, out price);
                //if fields are empty
                if (txtName.Text == string.Empty || txtDesc.Text == string.Empty || txtStock.Text == string.Empty || txtPrice.Text == string.Empty || txtAuPuCol.Text == string.Empty)
                {
                    //display error message
                    dialog = new MessageDialog("Please fill all fields", "Fill All Fields");
                    await dialog.ShowAsync();
                }
                //if item is book and genre field is empty
                else if (currentItemType == "book" && txtGenre.Text == string.Empty)
                {
                    //display error message
                    dialog = new MessageDialog("Please fill all fields", "Fill All Fields");
                    await dialog.ShowAsync();
                }
                //if stock is incorrect
                else if (success1 == false)
                {
                    //display error message
                    dialog = new MessageDialog("Please enter a valid stock amount", "Invalid Stock");
                    await dialog.ShowAsync();
                }
                //if price is incorrect
                else if (success2 == false)
                {
                    //display error message
                    dialog = new MessageDialog("Please enter a valid price", "Invalid Price");
                    await dialog.ShowAsync();
                }
                else
                {
                    //if item is a book
                    if (currentItemType == "book")
                    {
                        string author = txtAuPuCol.Text;
                        string genre = txtGenre.Text;

                        updatedBook = new Book(id, name, description, stock, currentItemType, currentImage, price, author, genre);
                        //update book in database
                        App.MY_ITEMVIEWMODEL.UpdateItem(updatedBook);
                    }
                    //if item is a magazine
                    else if (currentItemType == "magazine")
                    {
                        string publisher = txtAuPuCol.Text;

                        updatedMag = new Magazine(id, name, description, stock, currentItemType, currentImage, price, publisher);
                        //update magazine in database
                        App.MY_ITEMVIEWMODEL.UpdateItem(updatedMag);

                    }
                    //if item is a stationery
                    else if (currentItemType == "stationery")
                    {
                        string colour = txtAuPuCol.Text;

                        updatedStat = new Stationery(id, name, description, stock, currentItemType, currentImage, price, colour);
                        //update stationery in database
                        App.MY_ITEMVIEWMODEL.UpdateItem(updatedStat);

                    }
                    //refresh view
                    Refresh_View();
                }
                
            }
            else
            {
                //display error message
                dialog = new MessageDialog("Please select an item to update", "Select Item");
                await dialog.ShowAsync();
            }
        }

        private async void BtnChangeImg_Click(object sender, RoutedEventArgs e)
        {
            //create ContentDialog called PicturePicker
            PicturePicker picturePicker = new PicturePicker();
            BitmapImage item = new BitmapImage();

            //show the ContentDialog
            var optionPicked = await picturePicker.ShowAsync();
            switch (optionPicked)
            {
                //if he user clicks the Primary (OK) button
                case ContentDialogResult.Primary:

                    // display image picked
                    if(App.IsPictureSelected == true)
                    {
                        item.UriSource = new Uri("ms-appx://" + App.picturePicked);
                        imgItem.Source = item;
                        currentImage = App.picturePicked;
                    }

                    //close ContentDialog
                    break;
                //if Secondary (Cancel) button is clicked
                //close ContentDialog
                case ContentDialogResult.Secondary:
                    break;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //navigate to Employee Main Menu
            this.Frame.Navigate(typeof(EmployeeMainMenu));
        }

        private async void BtnViewDeleted_Click(object sender, RoutedEventArgs e)
        {
            //Display Removed Items Content Dialog
            RemovedItems removedItems = new RemovedItems();
            await removedItems.ShowAsync();
            //if process is unfinished
            while (removedItems.success == false)
            {
                //show content dialog
                await removedItems.ShowAsync();
            }
            //refresh view
            Refresh_View();

        }
    }
}
