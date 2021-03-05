using System;
using System.Collections.Generic;
using System.Text;
using VCCSharp.Models;

namespace VCCSharp
{
    public class CommandLineParser
    {
        public CmdLineArguments? Parse()
        {
            string[] arguments = Environment.GetCommandLineArgs();

            var args = new CmdLineArguments();

            for (int index = 1; index < arguments.Length; index++)
            {
                switch (arguments[index])
                {
                    case "/?":
                        Help();
                        return null;

                    case "-i":
                        args.IniFile = arguments[++index];
                        break;

                    default:
                        args.QLoadFile = arguments[index];
                        break;
                }
            }

            return args;
        }

        public void Help()
        {
            const string message = @"
Usage:
    VCCSharp.exe [-i ConfigFile] [QuickLoadFile] 
";
            Console.WriteLine(message);
        }
    }
}
