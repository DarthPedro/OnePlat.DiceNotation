using System;
using System.Security.Cryptography;

namespace OnePlat.DiceNotation.DieRoller
{
    public class CryptoDieRoller : RandomDieRollerBase
    {
        private static readonly RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoDieRoller"/> class.
        /// </summary>
        /// <param name="tracker">Die roll tracker to use; null means don't track roll data</param>
        public CryptoDieRoller(IDieRollTracker tracker = null)
            : base(tracker)
        {
        }

        /// <inheritdoc/>
        protected override int GetNextRandom(int sides)
        {
            return NumberBetween(1, sides);
        }

        #region Crypto random implementation

        /// <summary>
        /// Uses cryptography library to calculate a random number between two numbers.
        /// </summary>
        /// <param name="minValue">minimum value in range</param>
        /// <param name="maxValue">maximum value in range</param>
        /// <returns>Random number between minValue and maxValue</returns>
        private int NumberBetween(int minValue, int maxValue)
        {
            var randomNumber = new byte[1];
            generator.GetBytes(randomNumber);

            var asciiValue = Convert.ToDouble(randomNumber[0]);

            // using Math.Max and subtract 0.0000000000001 to ensure multiplier
            // is within expected range. Otherwise it could be 1 and cause rounding errors
            var multiplier = Math.Max(0, (asciiValue / 255d) - 0.00000000001d);

            // need to add one to range to allow for rounding with Math.Floor
            int range = maxValue - minValue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minValue + randomValueInRange);
        }
        #endregion
    }
}
