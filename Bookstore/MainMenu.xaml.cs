using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            this.InitializeComponent();
        }


        private void OrderBtn_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set orderBtn background colour when hovered over
            orderBtn.Background = (SolidColorBrush)Resources["DodgerBlue"];
        }

        private void OrderBtn_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set orderBtn background colour when hovered off
            orderBtn.Background = (SolidColorBrush)Resources["LightSeaGreen"];

        }

        private void StaffLoginBtn_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set staffBtn background colour when hovered over
            staffLoginBtn.Background = (SolidColorBrush)Resources["DodgerBlue"];
        }

        private void StaffLoginBtn_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set staffBtn background colour when hovered off
            staffLoginBtn.Background = (SolidColorBrush)Resources["LightSeaGreen"];
        }

        private async void StaffLoginBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //show the Login content dialog
            Login login = new Login();
            await login.ShowAsync();

        }

        private void OrderBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //navigate to Customer Main Menu
            this.Frame.Navigate(typeof(CustomerMainMenu));
        }
    }
}
