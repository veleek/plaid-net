using Ben.Plaid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PlaidBrowserModern
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            MainViewModel.Create().ContinueWith(
                async t =>
                {
                    this.ViewModel = await t;
                    await this.RefreshInstitutionsAsync();
                });
        }

        public MainViewModel ViewModel { get; set; }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await this.RefreshInstitutionsAsync();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Institution institution = null;
            try
            {
                institution = this.InstitutionsComboBox.SelectedItem as Institution;
                
                PlaidResponse authResponse = await this.ViewModel.Client.AddAuthAsync(
                    this.UsernameTextBox.Text, 
                    this.PasswordTextBox.Password, 
                    institution.Type, 
                    institution.Type == "usaa" ? this.PinCodeTextBox.Text : null);
                PlaidResponse transactionsResponse = await this.ViewModel.Client.GetTransactionsAsync(authResponse.AccessToken);

                await this.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.Institutions.ItemsSource = transactionsResponse.Transactions;
                    });
            }
            catch (PlaidException pe) when (institution != null && pe.Error.Code == 1601)
            {
                MessageDialog dialog = new MessageDialog(string.Format("{0} does not support getting transactions.", institution.Name));
                await dialog.ShowAsync();
            }
            catch (PlaidException pe) when (institution != null && pe.Error.Code == 1005)
            {
                MessageDialog dialog = new MessageDialog(string.Format("{0} requires MFA auth which is not supported yet.", institution.Name));
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
            }
        }

        private async Task RefreshInstitutionsAsync()
        {
            var insts = await this.ViewModel.RefreshInstitutionsAsync();
            await this.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, 
                () => 
                {
                    this.InstitutionsComboBox.ItemsSource = insts;
                    this.InstitutionsComboBox.SelectedIndex = 0;
                });
        }
    }
}
