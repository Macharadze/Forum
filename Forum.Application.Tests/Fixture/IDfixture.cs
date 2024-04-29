namespace Forum.Application.Tests.Fixture
{
    public class IDfixture : IDisposable
    {
        public Guid _guid { get; }
        public IDfixture()
        {
            _guid = Guid.NewGuid();
        }
        public void Dispose()
        {
        }
    }
}
