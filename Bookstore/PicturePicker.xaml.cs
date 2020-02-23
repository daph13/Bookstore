using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class PicturePicker : ContentDialog
    {
        public PicturePicker()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //retrieve images saved in the project
            Get_Images();
            //populate gridPictures with images from the images list
            gridPictures.ItemsSource = App.images;
        }

        private void Get_Images()
        {
            //create a list of images
            App.images = new List<string>();

            //Get names of files in the Images folder
            string[] fileImages = System.IO.Directory.GetFiles("Images/");

            //Rename the files
            foreach (string file in fileImages)
            {
                App.images.Add("/" + file);
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            MessageDialog dialog;

            //if a gridItem is not selected
            if(gridPictures.SelectedIndex == -1)
            {
                //set IsPictureSelected to false
                App.IsPictureSelected = false;
                //display error message
                dialog = new MessageDialog("Please select an image first");
                await dialog.ShowAsync();

                //redisplay Picture Picker Content Dialog
                PicturePicker picker = new PicturePicker();
                await picker.ShowAsync();
            }
            else
            {
                //set IsPictureSelected to true
                App.IsPictureSelected = true;                
            }
        }


        private void GridPictures_ItemClick(object sender, ItemClickEventArgs e)
        {
            //get the clicked GridView item
            int index = gridPictures.Items.IndexOf(e.ClickedItem);
            //get the image from the images list
            App.picturePicked = App.images[index];
        }

        private async void BtnAddImage_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog d;

            //open a FilePicker
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;

            //set the default location to the Pictures Library
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //show files with the following extensions
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            //get solution's path
            string root = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            //get the path of the solution's Images folder
            string path = root + @"\Images";

            //open the Filepicker where one can only pick one file
            //get the selected file
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {

                try
                {
                    //get the Images folder as a StorageFolder to store the selected file
                    StorageFolder copyToFolder = await StorageFolder.GetFolderFromPathAsync(path);

                    //copy the selected file to the Images folder
                    StorageFile copiedFile = await file.CopyAsync(copyToFolder, file.Name.ToString(), NameCollisionOption.ReplaceExisting);

                    d = new MessageDialog("Picked photo: " + file.Name);
                    await d.ShowAsync();

                    //create a list of image strings from the Images folder
                    Get_Images();

                    //repopulate the gridview
                    gridPictures.ItemsSource = App.images;

                }

                //catch the error if the file clicked already exists
                catch (System.IO.FileLoadException)
                {
                    //display error message
                    d = new MessageDialog("This picture already exists in this application");
                    await d.ShowAsync();
                }
            }
        }
    }
}
