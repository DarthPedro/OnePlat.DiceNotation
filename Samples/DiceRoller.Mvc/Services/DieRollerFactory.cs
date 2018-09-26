// <copyright file="DieRollerFactory.cs" company="DarthPedro">
// Copyright (c) 2017 DarthPedro. All rights reserved.
// </copyright>

using OnePlat.DiceNotation.DieRoller;
using System;

namespace DiceRoller.Mvc.Services
{
    /// <summary>
    /// Factory class for getting the appropriate die roller.
    /// </summary>
    public class DieRollerFactory
    {
        /// <summary>
        /// Gets the die roller based on specified type.
        /// </summary>
        /// <param name="type">Type string</param>
        /// <param name="tracker">Die roll tracker to use</param>
        /// <returns>Instance of die roller, or null if none found.</returns>
        public IDieRoller GetDieRoller(string type, IDieRollTracker tracker = null)
        {
            IDieRoller roller = null;

            if (type == typeof(ConstantDieRoller).ToString())
            {
                roller = new ConstantDieRoller();
            }
            else if (type == typeof(RandomDieRoller).ToString())
            {
                roller = new RandomDieRoller(tracker);
            }
            else if (type == typeof(CryptoDieRoller).ToString())
            {
                roller = new CryptoDieRoller(tracker);
            }
            else if (type == typeof(MathNetDieRoller).ToString())
            {
                roller = new MathNetDieRoller(tracker);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            return roller;
        }
    }
}
