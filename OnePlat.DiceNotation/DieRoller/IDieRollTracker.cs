// <copyright file="IDieRollTracker.cs" company="DarthPedro">
// Copyright (c) 2017 DarthPedro. All rights reserved.
// </copyright>

//-----------------------------------------------------------------------
// Assembly         : OnePlat.DiceNotation
// Author           : DarthPedro
// Created          : 8/30/2017
//
// Last Modified By : DarthPedro
// Last Modified On : 9/3/2017
//-----------------------------------------------------------------------
// <summary>
//       This project is licensed under the MIT license.
//
//       OnePlat.DiceNotation is an open source project that parses,
//  evalutes, and rolls dice that conform to the defined Dice notiation.
//  This notation is usable in any form of random dice games and role-playing
//  games like D&D and d20.
// </summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnePlat.DiceNotation.DieRoller
{
    /// <summary>
    /// Interface to define in-memory statistical tracking service for die rolls.
    /// </summary>
    public interface IDieRollTracker
    {
        /// <summary>
        /// Gets the limit of roll data kept in the tracker.
        /// </summary>
        int TrackerDataLimit { get; }

        /// <summary>
        /// Adds the current roll to the tracking service.
        /// </summary>
        /// <param name="dieSides">Die being rolled</param>
        /// <param name="result">Result of the die roll</param>
        /// <param name="dieRoller">Type of die roller used</param>
        void AddDieRoll(int dieSides, int result, Type dieRoller);

        /// <summary>
        /// Gets the roll tracking data based on specified filters. Not specifying a filter
        /// means that filter is not applied to the results.
        /// </summary>
        /// <param name="dieType">Filter results by die type</param>
        /// <param name="dieSides">Filter results by die sides</param>
        /// <returns>List of roll tracking data.</returns>
        Task<IList<DieTrackingData>> GetTrackingDataAsync(string dieType = null, string dieSides = null);

        /// <summary>
        /// Gets a frequency view of the tracking data in memory for reporting
        /// purposes.
        /// </summary>
        /// <returns>Frequency of rolls per roller type, sides, and results.</returns>
        Task<IList<AggregateDieTrackingData>> GetFrequencyDataViewAsync();

        /// <summary>
        /// Clears the current set of die tracking data.
        /// </summary>
        void Clear();

        /// <summary>
        /// Converts this set of tracker data to a json string representation.
        /// </summary>
        /// <returns>Json text representation.</returns>
        Task<string> ToJsonAsync();

        /// <summary>
        /// Loads this object data from a json string.
        /// </summary>
        /// <param name="jsonText">text to load</param>
        /// <returns>Task</returns>
        Task LoadFromJsonAsync(string jsonText);
    }
}
