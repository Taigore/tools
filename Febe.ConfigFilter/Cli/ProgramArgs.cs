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

        public static ProgramArgs? Parse(string[] args)
        {
            var action = args[0];
            if(action != "clean" && action != "smudge")
            {
                return null;
            }

            return new ProgramArgs(action);
        }

        public ProgramArgs(string action)
        {
            Action = action;
        }
    }
}
