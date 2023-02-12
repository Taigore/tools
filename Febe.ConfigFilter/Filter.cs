namespace Febe.ConfigFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    public class Filter
    {
        private readonly Stream input;
        private readonly Stream output;

        private XDocument? document = null;

        public Filter(Stream input, Stream output)
        {
            this.input = input;
            this.output = output;
        }

        public void Clean()
        {
            ParseDocument();
            if (document == null)
            {
                input.CopyTo(output);
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

        }

        private void ParseDocument()
        {
            document = null;

            try
            {
                var buffer = new MemoryStream();
                input.CopyTo(buffer);
                buffer.Position = 0;

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
    }
}
