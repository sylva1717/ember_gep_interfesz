﻿using HCI.Common;
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

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace HCI
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ConnectFinances cf = new ConnectFinances();

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

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            pageTitle.Text = "Dollar";
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
            
            cf.RefreshBalance();
            myBalance.Text = this.cf.Balance.ToString() + " HUF";
            if (this.cf.Balance > 0)
            {
                myBalance.Foreground = new SolidColorBrush(Windows.UI.Colors.Lime);
            }
            else
            {
                myBalance.Foreground = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
            }
            List<Finances> finances = new List<Finances>();
            List<Finances> newfinances = new List<Finances>();
            
            cf.RefreshAllFinances();
            finances = cf.AllFinances;
            itemGridView.ItemsSource = finances;

            

            List<string> ComboList = new List<string>();
            ComboList = cf.Types;
            comboTypes.ItemsSource = ComboList;
                 
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
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private void addNew(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(AddItem));
        }

        private void ItemSelected(object sender, RoutedEventArgs e)
        {
            if (!bottomAppBar.IsOpen) {
                bottomAppBar.IsOpen = true;
            }
        }

        

        private async void DeleteItem(object sender, RoutedEventArgs e)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Biztosan törli a kiválasztott elemet?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Igen",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(
                "Nem",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
           
            
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            var commandLabel = command.Label;
            switch (commandLabel)
            {
                case "Igen":
                    Finances f = new Finances();
                    if (itemGridView.SelectedItem != null)
                    {
                        // Create the message dialog and set its content

                        itemGridView.Items.Remove(itemGridView.SelectedIndex);
                        f = (Finances)itemGridView.SelectedItem;
                        cf.DeleteRecord(f);
                        this.Reload();
                    }
                    break;
            }
             
        }

        public bool Reload() { return Reload(null); }

        /*
         * Oldal frissítése törlés után
         */ 
        private bool Reload(object param)
        {
            Type type = this.Frame.CurrentSourcePageType;
            if (this.Frame.BackStack.Any())
            {
                type = this.Frame.BackStack.Last().SourcePageType;
                param = this.Frame.BackStack.Last().Parameter;
            }
            try { return this.Frame.Navigate(type, param); }
            finally { this.Frame.BackStack.Remove(this.Frame.BackStack.Last()); }
        }

        /*
         * Kategória szerinti szűrés
         */
        private void comboTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string type = comboTypes.SelectedItem.ToString();
            cf.RefreshTypedBalance(type);
            myBalance.Text = this.cf.TypedBalance.ToString() + " HUF";
            if (this.cf.TypedBalance > 0)
            {
                myBalance.Foreground = new SolidColorBrush(Windows.UI.Colors.Lime);
            }
            else
            {
                myBalance.Foreground = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
            }

            List<Finances> finances = new List<Finances>();

            cf.RefreshTypedFinances(type);
            finances = cf.TypedFinances;
            
            itemGridView.ItemsSource = finances;
        }
    }
}
