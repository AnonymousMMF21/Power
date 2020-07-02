using System;
using System.Diagnostics;

namespace KeLi.Power.Tool.Other
{
    /// <summary>
    ///     Performance tool.
    /// </summary>
    public static class PerformanceTool
    {
        /// <summary>
        ///     Gets using total milli second of the specified action.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static double GetTotalMs(this Action action)
        {
            var sw = new Stopwatch();

            sw.Start();

            action.Invoke();

            sw.Stop();

            return sw.Elapsed.TotalMilliseconds;
        }

        /// <summary>
        ///     Gets using total milli second of the specified func.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static double GetTotalMs(this Func<object> func, out object result)
        {
            var sw = new Stopwatch();

            sw.Start();

            result = func.Invoke();

            sw.Stop();

            return sw.Elapsed.TotalMilliseconds;
        }
    }
}