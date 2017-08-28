﻿// <copyright file="MainPage.xaml.cs" company="DarthPedro">
// Copyright (c) 2017 DarthPedro. All rights reserved.
// </copyright>

using DiceRoller.Win10.Services;
using DiceRoller.Win10.ViewModels;
using DiceRoller.Win10.Views;
using OnePlat.DiceNotation;
using OnePlat.DiceNotation.DieRoller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
namespace DiceRoller.Win10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Members
        private IDice diceService = AppServices.Instance.DiceService;
        private IDieRoller dieRoller = new RandomDieRoller();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.InitializeData();
            this.DataContext = this;

            // set control default values.
            this.DiceTypeCombobox.SelectedIndex = 5;
            this.DiceExpresssionGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        #region Properties

        /// <summary>
        /// Gets a list of the dice types to use in this page.
        /// </summary>
        public List<DiceType> DiceTypes { get; private set; }

        /// <summary>
        /// Gets the list for rolled dice results.
        /// </summary>
        public ObservableCollection<DiceResult> DiceRollResults { get; } = new ObservableCollection<DiceResult>();
        #endregion

        /// <summary>
        /// Initializes the data elements for this page.
        /// </summary>
        private void InitializeData()
        {
            this.DiceTypes = new List<DiceType>
            {
                new DiceType { DisplayText = "d4", DiceSides = 4, ImageUri = "ms-appx:///Assets/Dice4.png" },
                new DiceType { DisplayText = "d6", DiceSides = 6, ImageUri = "ms-appx:///Assets/Dice6.png" },
                new DiceType { DisplayText = "d8", DiceSides = 8, ImageUri = "ms-appx:///Assets/Dice8.png" },
                new DiceType { DisplayText = "d10", DiceSides = 10, ImageUri = "ms-appx:///Assets/Dice10.png" },
                new DiceType { DisplayText = "d12", DiceSides = 12, ImageUri = "ms-appx:///Assets/Dice12.png" },
                new DiceType { DisplayText = "d20", DiceSides = 20, ImageUri = "ms-appx:///Assets/Dice20.png" },
                new DiceType { DisplayText = "d%", DiceSides = 100, ImageUri = "ms-appx:///Assets/Dice100.png" },
            };
        }

        #region Event handlers

        /// <summary>
        /// Click handler for the Roll button for basic dice definition.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void RollButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // todo: rewrite this code to use DiceService when IDice is fixed to have a Clear method.
            IDice diceTemp = new Dice();

            // setup the dice expression
            DiceType diceType = (DiceType)this.DiceTypeCombobox.SelectedItem;
            diceTemp.Dice(diceType.DiceSides, (int)this.DiceNumberNumeric.Value);
            diceTemp.Constant((int)this.DiceModifierNumeric.Value);

            // roll the dice and save the results
            DiceResult result = diceTemp.Roll(this.dieRoller);
            this.DiceRollResults.Insert(0, result);
        }

        /// <summary>
        /// Click handler for the Roll button to handle dice expression rolls.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private async void RollExpressionButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                // roll the dice (based on dice expression string) and save the results.
                DiceResult result = this.diceService.Roll(this.DiceExpressionTextbox.Text, this.dieRoller);
                this.DiceRollResults.Insert(0, result);
            }
            catch (Exception ex)
            {
                // if there's an error in parsing the expression string, show an error message.
                string message = "There was a error parsing the dice expression: " +
                                 this.DiceExpressionTextbox.Text +
                                 "\r\nException Text: " + ex.Message +
                                 "\r\nPlease correct the expression and try again.";

                MessageDialog dialog = new MessageDialog(message, "Dice Parsing Error");
                await dialog.ShowAsync();
            }
        }

        /// <summary>
        /// Click handler toggle which dice roller view to show.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void ViewToggleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.ViewToggleButton.IsChecked.Value)
            {
                this.DiceRollGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.DiceExpresssionGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                this.DiceRollGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                this.DiceExpresssionGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Click handler for Settings page navigation.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void SettingsButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }
        #endregion
    }
}