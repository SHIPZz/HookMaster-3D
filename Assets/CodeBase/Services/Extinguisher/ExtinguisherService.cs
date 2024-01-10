using CodeBase.Services.Providers.Extinguisher;

namespace CodeBase.Services.Extinguisher
{
    public class ExtinguisherService
    {
        private readonly ExtinguisherProvider _extinguisherProvider;

        public ExtinguisherService(ExtinguisherProvider extinguisherProvider)
        {
            _extinguisherProvider = extinguisherProvider;
        }

        public void Init()
        {
            _extinguisherProvider.ExtinguisherSpawners.ForEach(x => x.Spawn());
        }
    }
}