namespace Febe.ConfigFilter.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class FilterSmudge
    {
        private readonly MemoryStream output = new MemoryStream();

        [Fact]
        public void IfInputStreamIsEmptyOutputStreamIsEmpty()
        {
            var input = StreamString("");

            var filter = new Filter(input, output);
            filter.Smudge();

            output.Position = 0;
            var reader = new StreamReader(output);
            Assert.Equal("", reader.ReadToEnd());
        }

        [Fact]
        public void RootElementRemainsTheSame()
        {
            var input = StreamString("<myRoot></myRoot>");

            var filter = new Filter(input, output);
            filter.Smudge();

            output.Position = 0;

            var result = XDocument.Load(output);
            var root = result.Root;

            Assert.NotNull(root);
            Assert.Equal("myRoot", root.Name.LocalName);
        }

        [Fact]
        public void ChildElementRemainsTheSame()
        {
            var input = StreamString(@"<root>
  <child></child>
</root>");

            var filter = new Filter(input, output);
            filter.Smudge();

            output.Position = 0;

            var result = XDocument.Load(output);
            var root = result.Root;
            Assert.Collection(root!.Elements(),
                x => Assert.Equal("child", x.Name.LocalName)
            );
        }

        [Fact]
        public void ChildElementIsFilledWithValue()
        {
            var input = StreamString(@"<root>
  <child></child>
</root>");

            var filter = new Filter(input, output);
            filter.Smudge();

            output.Position = 0;

            var result = XDocument.Load(output);
            var root = result.Root;
            Assert.Collection(root!.Elements(),
                x => Assert.NotEmpty(x.Value)
            );
        }

        private NoSeekStream StreamString(string content)
        {
            var byteCount = Encoding.UTF8.GetByteCount(content);
            var bomLength = Encoding.UTF8.Preamble.Length;

            var buffer = new byte[bomLength + byteCount];
            var stream = new MemoryStream(buffer);
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(content);
            writer.Flush();

            var result = new MemoryStream(buffer, writable: false);

            return new NoSeekStream(result);
        }
    }
}
