namespace Febe.ConfigFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    static internal class Log
    {
        private static TextWriter? _writer = null;

        internal static void SetPath(string path)
        {
            _writer?.Dispose();
            _writer = null;

            if(path != null)
            {
                _writer = new StreamWriter(path, append: true);
            }
        }

        internal static void Write(string message)
        {
            _writer?.WriteLine(message);
        }
    }
}
