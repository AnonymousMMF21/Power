﻿using System;
using System.Runtime.InteropServices;

using Autodesk.Windows;

namespace KeLi.Power.Revit.Widgets
{
    /// <summary>
    ///     Key sender.
    /// </summary>
    public class KeySender
    {
        /// <summary>
        ///     Sets foreground window.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        ///     Sends key.
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        /// <summary>
        ///     Sends key.
        /// </summary>
        public void SendKey(byte key, int times = 1)
        {
            var revit = ComponentManager.ApplicationWindow;

            SetForegroundWindow(revit);

            for (var i = 0; i < times; i++)
            {
                keybd_event(key, 0, 0, 0);
                keybd_event(key, 0, 2, 0);
            }
        }
    }
}