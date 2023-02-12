namespace Febe.ConfigFilter.Tests
{
    using Febe.ConfigFilter.Tests.Stubs;
    using System.Text;
    using System.Xml.Linq;

    public class FilterClean
    {
        private readonly MemoryStream output = new MemoryStream();

        [Fact]
        public void IfInputStreamIsEmptyOutputStreamIsEmpty()
        {
            var input = StreamString("");

            var filter = new Filter(input, output);
            filter.Clean();

            Assert.Equal(0, output.Length);
        }

        [Fact]
        public void RootElementRemainsTheSame()
        {
            var input = StreamString("<myRoot></myRoot>");

            var filter = new Filter(input, output);
            filter.Clean();

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
            filter.Clean();

            output.Position = 0;

            var result = XDocument.Load(output);
            var root = result.Root;
            Assert.Collection(root!.Elements(),
                x => Assert.Equal("child", x.Name.LocalName)
            );
        }

        [Fact]
        public void ContentsOfChildElementAreCleared()
        {
            var input = StreamString(@"<root>
  <value>XYZ</value>
</root>");

            var filter = new Filter(input, output);
            filter.Clean();

            output.Position = 0;

            var result = XDocument.Load(output);
            var root = result.Root;
            Assert.Collection(root!.Elements(),
                x => Assert.Equal(string.Empty, x.Value)
            );
        }

        [Fact]
        public void CleanOutputIsNotChanged()
        {
            var source = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <value></value>
</root>";

            var input = StreamString(source);

            var filter = new Filter(input, output);
            filter.Clean();

            output.Position = 0;

            var reader = new StreamReader(output);
            var result = reader.ReadToEnd();

            Assert.Equal(source, result);
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

        private class NoSeekStream : StreamStub
        {
            private readonly Stream wrapped;

            public override bool CanRead => wrapped.CanRead;

            internal NoSeekStream(Stream wrapped)
            {
                this.wrapped = wrapped;
            }

            public override int Read(byte[] buffer, int offset, int count)
                => wrapped.Read(buffer, offset, count);
        }
    }
}