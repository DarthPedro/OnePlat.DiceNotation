// <copyright file="MathNetDieRoller.cs" company="DarthPedro">
// Copyright (c) 2017 DarthPedro. All rights reserved.
// </copyright>

//-----------------------------------------------------------------------
// Assembly         : OnePlat.Mvvm.Core
// Author           : DarthPedro
// Created          : 9/3/2017
//
// Last Modified By : DarthPedro
// Last Modified On : 9/5/2017
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
using MathNet.Numerics.Random;
using System;

namespace OnePlat.DiceNotation.DieRoller
{
    /// <summary>
    /// Die roller class that uses MathNet numerics library to generate random numbers.
    /// </summary>
    public class MathNetDieRoller : RandomDieRollerBase
    {
        private static RandomSource randomSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="MathNetDieRoller"/> class.
        /// </summary>
        /// <param name="tracker">Die roll tracker to use; null means don't track roll data</param>
        public MathNetDieRoller(IDieRollTracker tracker = null)
            : this(new MersenneTwister(), tracker)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MathNetDieRoller"/> class.
        /// </summary>
        /// <param name="source">Random source to use</param>
        /// <param name="tracker">Die roll tracker to use; null means don't track roll data</param>
        public MathNetDieRoller(RandomSource source, IDieRollTracker tracker = null)
            : base(tracker)
        {
            randomSource = source ?? throw new ArgumentNullException(nameof(source));
        }

        #region Secure random implementation

        /// <inheritdoc/>
        protected override int GetNextRandom(int sides)
        {
            if (sides < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(sides));
            }

            return randomSource.Next(0, sides) + 1;
        }
        #endregion
    }
}
