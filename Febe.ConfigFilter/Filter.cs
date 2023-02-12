namespace Febe.ConfigFilter
{
    using System.Diagnostics;
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
                foreach(var e in root!.Elements())
                {
                    e.Value = string.Empty;
                }

                document.Save(output);
            }
        }

        public void Smudge()
        {
            BufferInput();
            ParseDocument();
            if(document == null)
            {
                CopyInputToOutput();
            }
            else
            {
                var root = document.Root;
                foreach(var e in root!.Elements())
                {
                    e.Value = "x";
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
}
