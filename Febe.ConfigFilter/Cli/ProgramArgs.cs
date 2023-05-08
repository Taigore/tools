namespace Febe.ConfigFilter.Cli
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProgramArgs
    {
        public string Action { get; }
        public string? ConfigPath { get; }

        public static ProgramArgs? Parse(string[] args)
        {
            var action = args[0];
            if(action != "clean" && action != "smudge")
            {
                return null;
            }

            var configPath = (string?)null;
            for (int i = 1; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case "--config":
                        configPath = args[++i];
                        continue;
                    default:
                        Console.WriteLine("Unrecognized argument: {0}", args[i]);
                        return null;
                }
            }

            return new ProgramArgs(action, configPath);
        }

        public ProgramArgs(string action, string? configPath)
        {
            Action = action;
            ConfigPath = configPath;
        }
    }
}
