namespace Febe.ConfigFilter
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml;
    using System.Xml.Linq;

    public class Filter
    {
        private readonly Stream input;
        private readonly Stream output;

        private MemoryStream? buffer = null;
        private XDocument? document = null;

        public Filter(Stream input, Stream output)
        {
            this.input = input;
            this.output = output;
        }

        public void Clean()
        {
            BufferInput();
            ParseDocument();
            if (document == null)
            {
                CopyInputToOutput();
            }
            else
            {
                var root = document.Root;
                foreach (var e in root!.Elements())
                {
                    e.Value = string.Empty;
                }

                document.Save(output);
            }
        }

        public void Smudge(ConfigLoader configLoader)
        {
            BufferInput();
            ParseDocument();
            if (document == null)
            {
                CopyInputToOutput();
            }
            else
            {
                if(configLoader == null)
                {
                    throw new NotImplementedException("Missing config loader");
                }

                configLoader.Load();

                var root = document.Root;
                foreach (var e in root!.Elements())
                {
                    var name = e.Name.LocalName;
                    if(configLoader.Values.TryGetValue(name, out var value))
                    {
                        e.Value = value;
                    }
                }

                document.Save(output);
            }
        }

        private void ParseDocument()
        {
            Debug.Assert(buffer != null);

            document = null;
            try
            {
                if (buffer.Length == 0)
                {
                    return;
                }

                document = XDocument.Load(buffer);
            }
            catch (XmlException)
            {
                document = null;
            }
        }

        private void BufferInput()
        {
            buffer = new MemoryStream();
            input.CopyTo(buffer);
            buffer.Position = 0;
        }

        private void CopyInputToOutput()
        {
            Debug.Assert(buffer != null);

            buffer.Position = 0;
            buffer.CopyTo(output);
        }
    }

    public class ConfigLoader
    {
        private readonly string sourcePath;

        public IReadOnlyDictionary<string, string>? Values { get; private set; }
        public Func<string, IEnumerable<string>> ReadLines { get; set; } = File.ReadLines;

        public ConfigLoader(string sourcePath)
        {
            this.sourcePath = sourcePath;
        }

        [MemberNotNull(nameof(Values))]
        public void Load()
        {
            var result = new Dictionary<string, string>();
            foreach(var l in ReadLines(sourcePath))
            {
                var parts = l.Split(new char[] { '=' }, 2);
                if(parts.Length == 2)
                {
                    result.Add(parts[0], parts[1]);
                }
            }

            Values = result;
        }
    }
}
