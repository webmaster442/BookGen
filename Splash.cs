//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen
{
    internal static class Splash
    {
        public static void DoSplash()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"______             _    _____            ");
            Console.WriteLine(@"| ___ \           | |  |  __ \           ");
            Console.WriteLine(@"| |_/ / ___   ___ | | _| |  \/ ___ _ __  ");
            Console.WriteLine(@"| ___ \/ _ \ / _ \| |/ / | __ / _ \ '_ \ ");
            Console.WriteLine(@"| |_/ / (_) | (_) |   <| |_\ \  __/ | | |");
            Console.WriteLine(@"\____/ \___/ \___/|_|\_\\____/\___|_| |_|");
            Console.WriteLine();
            Console.ForegroundColor = color;
        }

        public static void PressKeyToExit()
        {
            Console.WriteLine();
            Console.WriteLine(@"______                            _                _                    _ _           ");
            Console.WriteLine(@"| ___ \                          | |              | |                  (_) |          ");
            Console.WriteLine(@"| |_/ / __ ___  ___ ___    __ _  | | _____ _   _  | |_ ___     _____  ___| |_         ");
            Console.WriteLine(@"|  __/ '__/ _ \/ __/ __|  / _` | | |/ / _ \ | | | | __/ _ \   / _ \ \/ / | __|        ");
            Console.WriteLine(@"| |  | | |  __/\__ \__ \ | (_| | |   <  __/ |_| | | || (_) | |  __/>  <| | |_   _ _ _ ");
            Console.WriteLine(@"\_|  |_|  \___||___/___/  \__,_| |_|\_\___|\__, |  \__\___/   \___/_/\_\_|\__| (_|_|_)");
            Console.WriteLine(@"                                            __/ |                                     ");
            Console.WriteLine(@"                                           |___/                                      ");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
