namespace Febe.ConfigFilter.Tests
{
    using Febe.ConfigFilter.Tests.Stubs;

    internal class NoSeekStream : StreamStub
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