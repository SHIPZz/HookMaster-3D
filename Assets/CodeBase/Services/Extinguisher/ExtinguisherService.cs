using System;
using CodeBase.Data;
using CodeBase.Services.Fire;
using CodeBase.Services.Providers.Extinguisher;
using CodeBase.Services.Wallet;
using CodeBase.Services.Window;
using CodeBase.UI.PopupWindows;
using Zenject;

namespace CodeBase.Services.Extinguisher
{
    public class ExtinguisherService : IInitializable, IDisposable
    {
        private const int PutOutReward = 150;
        private readonly ExtinguisherProvider _extinguisherProvider;
        private readonly FireService _fireService;
        private readonly WalletService _walletService;
        private readonly WindowService _windowService;
        
        public ExtinguisherService(ExtinguisherProvider extinguisherProvider,
            FireService fireService,
            WalletService walletService,
            WindowService windowService)
        {
            _windowService = windowService;
            _walletService = walletService;
            _fireService = fireService;
            _extinguisherProvider = extinguisherProvider;
        }

        public void Initialize()
        {
            _fireService.FireStarted += SpawnExtinguishers;
            _fireService.FirePutOut += FirePutOutHandler;
        }

        public void Dispose()
        {
            _fireService.FireStarted -= SpawnExtinguishers;
            _fireService.FirePutOut -= FirePutOutHandler;
        }

        private void FirePutOutHandler()
        {
            _extinguisherProvider.ExtinguisherSpawners.ForEach(x => x.DestroyCreatedExtinguisher());
            _walletService.Set(ItemTypeId.Money, PutOutReward);
            
            var popupMessageWindow = _windowService.GetWithoutSettingToCurrentWindowAndCaching<PopupMessageWindow>();
            popupMessageWindow.Init(PopupMessageType.Green, $"{PutOutReward}$");
            popupMessageWindow.Open();
            popupMessageWindow.SetAutoDestroy(3f);
        }

        private void SpawnExtinguishers()
        {
            _extinguisherProvider.ExtinguisherSpawners.ForEach(x => x.Spawn());
        }
    }
}