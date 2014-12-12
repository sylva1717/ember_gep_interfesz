using HCI.Common;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace HCI
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AddItem : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ConnectFinances cf = new ConnectFinances();
        private int isUpdate = 0;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public AddItem()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            pageTitle.Text = "Új bejegyzés hozzáadása";
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

           
         
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<string> ComboList = new List<string>();
            ComboList = cf.Types;
            comboTypes.ItemsSource = ComboList;

            if (e.Parameter != null)
            {
                this.isUpdate = 1;
                Finances item = (Finances)e.Parameter;
                txtTitle.Text = item.Title;
                if (item.Amount < 0)
                {
                    itemType.SelectedIndex = 0;
                }
                else
                {
                    itemType.SelectedIndex = 1;
                }
                if (item.Amount < 0)
                {
                    item.Amount *= -1;
                }
                txtAmount.Text = item.Amount.ToString();
                txtDate.Date = item.Date;
                txtTime.Time = item.Date.TimeOfDay;
                comboTypes.SelectedItem = item.Type;
                txtId.Text = item.Id.ToString();
                
                
            }
            else
            {
                this.isUpdate = 0;
            }
            
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

       
        /*
         * Add new item
         */ 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int amount = int.Parse(txtAmount.Text.ToString());
            if (itemType.SelectedIndex == 0)
            {
                amount *= -1;
            }
            Finances f = new Finances();
            f.Title = txtTitle.Text.ToString();
            f.Amount = amount;
            f.Date = txtDate.Date.LocalDateTime + txtTime.Time;
            f.Type = comboTypes.SelectedItem.ToString();
            if (this.isUpdate == 0)
            {
                cf.InsertRecord(f);
            }
            else
            {
                f.Id = int.Parse(txtId.Text.ToString());
                cf.UpdateRecord(f);
            }
            cf.UpdateRecord(f);
            
            this.Frame.Navigate(typeof(MainPage));
        }

        /**
         * Back to home
         **/
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }



        
        
    }
}
