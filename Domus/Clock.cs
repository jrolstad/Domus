using System;

namespace Domus
{
    /// <summary>
    /// A wrapper around the static operations on <see cref="DateTime"/> which allows time
    /// to be frozen.
    /// </summary>
    public static class Clock
    {
        private static bool IsFrozen { get; set; }
        private static DateTime FreezeTime { get; set; }

        /// <summary>
        /// The current time
        /// </summary>
        public static DateTime Now
        {
            get
            {
                var now = (IsFrozen) ? FreezeTime : DateTime.Now;
                return now;
            }
        }

        /// <summary>
        /// The current date
        /// </summary>
        public static DateTime Today
        {
            get { return Now.Date; }
        }

        /// <summary>
        /// Let the clock start flowing again
        /// </summary>
        public static void Thaw()
        {
            IsFrozen = false;
        }
        /// <summary>
        /// Freeze the clock to the current date / time
        /// </summary>
        /// <returns></returns>
        public static IDisposable Freeze()
        {
            return Freeze(DateTime.Now);
        }
        /// <summary>
        /// Freeze the clock to a given point in time
        /// </summary>
        /// <param name="dateTimeToFreezeTo">When to freeze the clock to</param>
        /// <returns></returns>
        public static IDisposable Freeze(DateTime dateTimeToFreezeTo)
        {
            if (IsFrozen)
                throw new InvalidOperationException("Clock already frozen.");
            FreezeTime = dateTimeToFreezeTo;
            IsFrozen = true;

            return new ClockThawer();
        }

        /// <summary>
        /// Thaws the clock when frozen
        /// </summary>
        private class ClockThawer : IDisposable
        {
            public void Dispose()
            {
                Thaw();
            }
        }
    }
}