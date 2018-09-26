// <copyright file="SettingsPage.xaml.cs" company="DarthPedro">
// Copyright (c) 2017 DarthPedro. All rights reserved.
// </copyright>

using DiceRoller.Win10.Services;
using DiceRoller.Win10.ViewModels;
using OnePlat.DiceNotation.DieRoller;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace DiceRoller.Win10.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        #region Members
        private const int DataLimitFactor = 1000;
        private IDieRollTracker frequencyTracker = AppServices.Instance.DiceFrequencyTracker;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
            this.InitializeData();

            this.DiceFrequencyLimitSlider.Value = this.frequencyTracker.TrackerDataLimit / DataLimitFactor;
        }

        #region Properties

        /// <summary>
        /// Gets the app settings for this page.
        /// </summary>
        public AppSettingsService Settings { get; } = AppServices.Instance.AppSettingsService;

        /// <summary>
        /// Gets the list of die rollers that can be used by the app.
        /// </summary>
        public List<DieRollerType> DieRollerTypes { get; private set; }
        #endregion

        #region Helper methods

        /// <summary>
        /// Initializes the data elements for this page.
        /// </summary>
        private void InitializeData()
        {
            this.DieRollerTypes = new List<DieRollerType>
            {
                new DieRollerType { DisplayText = "Pseudo Random [default]", Type = typeof(RandomDieRoller).ToString() },
                new DieRollerType { DisplayText = "Crypto Random", Type = typeof(CryptoDieRoller).ToString() },
                new DieRollerType { DisplayText = "MathNet Random", Type = typeof(MathNetDieRoller).ToString() },
            };

            this.DieRollTypeCombobox.ItemsSource = this.DieRollerTypes;
            DieRollerType selectedType = this.DieRollerTypes.First(t => t.Type == this.Settings.CurrentDieRollerType);
            this.DieRollTypeCombobox.SelectedItem = selectedType;
        }
        #endregion

        #region Event handlers

        /// <summary>
        /// Click handler to clear the current results list.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void ClearResultsButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Settings.ClearResultsList = true;
        }

        /// <summary>
        /// Click handler to clear the dice frequency data.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void ClearFrequencyButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AppServices.Instance.DiceFrequencyTracker.Clear();
        }

        /// <summary>
        /// Event handler for frequency data limit slider changes.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void DiceFrequencyLimitSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.OldValue != 0)
            {
                int limit = (int)this.DiceFrequencyLimitSlider.Value * DataLimitFactor;
                this.Settings.CachedTrackerDataLimit = limit;
                this.frequencyTracker.TrackerDataLimit = limit;
            }
        }

        /// <summary>
        /// Event handler for when the die roller type selection changes.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void DieRollTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Settings.CurrentDieRollerType = this.DieRollTypeCombobox.SelectedValue.ToString();
        }
        #endregion
    }
}
