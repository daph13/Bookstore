using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bookstore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmployeeMainMenu : Page
    {
        public EmployeeMainMenu()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //display the logged employee's ID and name in the textblocks
            txtEmployeeID.Text = App.employeeLogged.UserID.ToString();
            txtEmployeeName.Text = App.employeeLogged.EmployeeName();
        }

        private void BtnStock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //navigate to Inventory Management Page
            this.Frame.Navigate(typeof(InventoryManagementPage));
        }

        private void BtnSale_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //navigate to Sale Page
            this.Frame.Navigate(typeof(SalePage));
        }

        private async void BtnAdmin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //if the employee is a manager
            if(App.employeeLogged.IsManager == true)
            {
                //navigate to Employee Management Page
                this.Frame.Navigate(typeof(EmployeeManagementPage));
            }
            else
            {
                //display message
                MessageDialog dialog = new MessageDialog("You cannot access this page if you are not a manager", "Cannot Access");
                await dialog.ShowAsync();
            }
        }

        private void BtnSalesStats_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //navigate to Sales Stats Page
            this.Frame.Navigate(typeof(SalesStatsPage));
        }

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //display confirmation message
            MessageDialog dialog = new MessageDialog("Are you sure you want to logout?", "Logout");
            var yes = new UICommand("Yes");
            var no = new UICommand("No");
            dialog.Commands.Add(yes);
            dialog.Commands.Add(no);

            var command = await dialog.ShowAsync();
            //if user selects 'yes'
            if (command == yes)
            {
                //set employeeLogged to null
                App.employeeLogged = null;
                //navigate to Main Menu
                this.Frame.Navigate(typeof(MainMenu));
            }
        }

        private void BtnSale_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set btnSale background colour when hovered over
            btnSale.Background = (SolidColorBrush)Resources["DodgerBlue"];
        }

        private void BtnSale_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set btnSale background colour when hovered off
            btnSale.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BtnStock_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set btnStock background colour when hovered over
            btnStock.Background = (SolidColorBrush)Resources["DodgerBlue"];
        }

        private void BtnStock_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set btnStock background colour when hovered off
            btnStock.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BtnAdmin_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set btnAdmin background colour when hovered over
            btnAdmin.Background = (SolidColorBrush)Resources["DodgerBlue"];
        }

        private void BtnAdmin_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set btnAdmin background colour when hovered off
            btnAdmin.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BtnSalesStats_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //set btnSalesStats background colour when hovered over
            btnSalesStats.Background = (SolidColorBrush)Resources["DodgerBlue"];
        }

        private void BtnSalesStats_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //set btnSalesStats background colour when hovered off
            btnSalesStats.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
