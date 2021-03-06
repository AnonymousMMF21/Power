﻿/*
 * MIT License
 *
 * Copyright(c) 2019 KeLi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
             ,---------------------------------------------------,              ,---------,
        ,----------------------------------------------------------,          ,"        ,"|
      ,"                                                         ,"|        ,"        ,"  |
     +----------------------------------------------------------+  |      ,"        ,"    |
     |  .----------------------------------------------------.  |  |     +---------+      |
     |  | C:\>FILE -INFO                                     |  |  |     | -==----'|      |
     |  |                                                    |  |  |     |         |      |
     |  |                                                    |  |  |/----|`---=    |      |
     |  |              Author: KeLi                          |  |  |     |         |      |
     |  |              Email: kelistudy@163.com              |  |  |     |         |      |
     |  |              Creation Time: 10/30/2019 07:08:41 PM |  |  |     |         |      |
     |  | C:\>_                                              |  |  |     | -==----'|      |
     |  |                                                    |  |  |   ,/|==== ooo |      ;
     |  |                                                    |  |  |  // |(((( [66]|    ,"
     |  `----------------------------------------------------'  |," .;'| |((((     |  ,"
     +----------------------------------------------------------+  ;;  | |         |,"
        /_)_________________________________________________(_/  //'   | +---------+
           ___________________________/___  `,
          /  oooooooooooooooo  .o.  oooo /,   \,"-----------
         / ==ooooooooooooooo==.o.  ooo= //   ,`\--{)B     ,"
        /_==__==========__==_ooo__ooo=_/'   /___________,"
*/

using System;
using System.Configuration;
using System.Reflection;

namespace KeLi.Power.Tool.Other
{
    /// <summary>
    ///     Config utility.
    /// </summary>
    public static class ConfigUtil
    {
        /// <summary>
        ///     Local position.
        /// </summary>
        private static string _location;

        /// <summary>
        ///     To repeat creating setting instance by single thread.
        /// </summary>
        private static AppSettingsSection _setting;

        /// <summary>
        ///     Config utility.
        /// </summary>
        public static void Init(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (_location is null)
                _location = Assembly.GetAssembly(type).Location;

            if (_setting != null)
                return;

            var cfgName = _location + ".config";

            var map = new ExeConfigurationFileMap { ExeConfigFilename = cfgName };

            _setting = ConfigurationManager.OpenMappedExeConfiguration(map, 0).AppSettings;
        }

        /// <summary>
        ///     Get config file value by key.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetValue(string keyName)
        {
            if (keyName is null)
                throw new ArgumentNullException(nameof(keyName));

            if (_setting is null)
                throw new Exception("Please call Init() method firstly!");

            return _setting.Settings[keyName].Value;
        }
    }
}