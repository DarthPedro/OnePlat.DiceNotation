// <copyright file="DiceSettingsViewModel.cs" company="DarthPedro">
// Copyright (c) 2017 DarthPedro. All rights reserved.
// </copyright>

using DiceRoller.Mvc.Services;
using OnePlat.DiceNotation;
using OnePlat.DiceNotation.DieRoller;
using System.Collections.Generic;
using System.Linq;

namespace DiceRoller.Mvc.ViewModels
{
    /// <summary>
    /// View model for the dice settings
    /// </summary>
    public class DiceSettingsViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DiceSettingsViewModel"/> class.
        /// </summary>
        public DiceSettingsViewModel()
        {
            // first set up roller types list and current roller
            this.DieRollerTypes = new List<DieRollerType>
            {
                new DieRollerType { DisplayText = "Pseudo Random [default]", Type = typeof(RandomDieRoller).ToString() },
                new DieRollerType { DisplayText = "Crypto Random", Type = typeof(CryptoDieRoller).ToString() },
                new DieRollerType { DisplayText = "MathNet Random", Type = typeof(MathNetDieRoller).ToString() },
            };

            this.SelectedDieRollerType = this.DieRollerTypes.First(t => t.Type == this.Settings.CurrentDieRollerType);

            // then set up dice sides list and current dice sides setting
            this.DiceSides = new List<int> { 4, 6, 8, 10, 12, 20, 100 };
            this.SelectedDefaultDiceSides = this.Settings.DefaultDiceSides;

            // finally set up the data frequency limits and current setting for limit.
            this.DataFrequencyLimits = new List<int> { 50000, 100000, 250000, 500000, 750000, 1000000 };
            this.SelectedDataFrequencyLimit = this.Settings.CachedTrackerDataLimit;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the app settings service.
        /// </summary>
        public AppSettingsService Settings { get; } = AppServices.Instance.AppSettingsService;

        /// <summary>
        /// Gets the list of die rollers that can be used by the app.
        /// </summary>
        public List<DieRollerType> DieRollerTypes { get; private set; }

        /// <summary>
        /// Gets or sets the selected die roller types.
        /// </summary>
        public DieRollerType SelectedDieRollerType { get; set; }

        /// <summary>
        /// Gets the dice sides list.
        /// </summary>
        public List<int> DiceSides { get; private set; }

        /// <summary>
        /// Gets or sets the default number of dice sides to use.
        /// </summary>
        public int SelectedDefaultDiceSides { get; set; }

        /// <summary>
        /// Gets the list of data frequency items that can be selected.
        /// </summary>
        public List<int> DataFrequencyLimits { get; private set; }

        /// <summary>
        /// Gets or sets the selected limit for frequency data.
        /// </summary>
        public int SelectedDataFrequencyLimit { get; set; }
        #endregion

        #region Commands

        /// <summary>
        /// Saves the settings changes.
        /// </summary>
        /// <param name="useUnbounded">Unbounded use</param>
        /// <param name="rollerName">Selected dice roller type</param>
        public void SaveSettingsCommand(bool useUnbounded, string rollerName)
        {
            // find the selected roller type based on the specified name.
            this.SelectedDieRollerType = this.DieRollerTypes.First(t => t.DisplayText == rollerName);

            // update all of the properties in the AppSettingsService.
            this.Settings.UseUnboundedResults = useUnbounded;
            this.Settings.CurrentDieRollerType = this.SelectedDieRollerType.Type.ToString();
            this.Settings.DefaultDiceSides = this.SelectedDefaultDiceSides;
            this.Settings.CachedTrackerDataLimit = this.SelectedDataFrequencyLimit;

            // finally update the DiceService configuration as well.
            DiceConfiguration diceConfig = AppServices.Instance.DiceService.Configuration;
            diceConfig.HasBoundedResult = !useUnbounded;
            diceConfig.DefaultDieSides = this.SelectedDefaultDiceSides;
            AppServices.Instance.DiceFrequencyTracker.TrackerDataLimit = this.SelectedDataFrequencyLimit;
        }
        #endregion
    }
}