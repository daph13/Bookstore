using System;
using System.Collections.Generic;
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
    public sealed partial class CustomerMainMenu : Page
    {
        public CustomerMainMenu()
        {
            this.InitializeComponent();
        }

        private void BtnOrder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
           //set btnOrder background when hovered over
           btnOrder.Background = (SolidColorBrush)Resources["DodgerBlue"];
        }

        private void BtnOrder_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set btnOrder background when hovered off
            btnOrder.Background = (SolidColorBrush)Resources["LightSeaGreen"];
        }

        private void BtnChange_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set btnChange background when hovered over
            btnChange.Background = (SolidColorBrush)Resources["DodgerBlue"];
        }

        private void BtnChange_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set btnChange background when hovered off
            btnChange.Background = (SolidColorBrush)Resources["LightSeaGreen"];
        }

        private void BtnOrder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //navigate to Order Page
            this.Frame.Navigate(typeof(OrderPage));
        }

        private async void BtnChange_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //display OrderAccessPage Content Dialog
            OrderAccessPage orderDetails = new OrderAccessPage();
            await orderDetails.ShowAsync();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //navigate back to Main Menu
            this.Frame.Navigate(typeof(MainMenu));
        }
    }
}
