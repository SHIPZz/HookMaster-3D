using System.Collections.Generic;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.DataService;
using CodeBase.Services.Factories.GameItem;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Time;
using CodeBase.Services.Wallet;
using CodeBase.Services.WorldData;
using CodeBase.SO.GameItem.Mining;
using UnityEngine;

namespace CodeBase.Services.Mining
{
    public class MiningFarmService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly WorldTimeService _worldTimeService;
        private readonly WalletService _walletService;
        private readonly GameItemFactory _gameItemFactory;
        private readonly LocationProvider _locationProvider;
        private readonly GameStaticDataService _gameStaticDataService;

        private Dictionary<string, MiningFarmItem> _createdFarms = new();

        public MiningFarmService(WorldTimeService worldTimeService,
            IWorldDataService worldDataService,
            WalletService walletService,
            GameItemFactory gameItemFactory,
            LocationProvider locationProvider,
            GameStaticDataService gameStaticDataService)
        {
            _gameStaticDataService = gameStaticDataService;
            _locationProvider = locationProvider;
            _gameItemFactory = gameItemFactory;
            _walletService = walletService;
            _worldDataService = worldDataService;
            _worldTimeService = worldTimeService;
        }

        public void Init()
        {
            Dictionary<string, MiningFarmData> miningFarmDatas = _worldDataService.WorldData.MiningFarmDatas;

            CreateMiningFarmsFromData(miningFarmDatas);

            if (!HasCreatedFarms())
                return;

            ConfigureMiningFarms(miningFarmDatas);

            HandleTimeDifference();
        }

        public MiningFarmItem CreateMiningFarm()
        {
            var item = _gameItemFactory.Create<MiningFarmItem>(_locationProvider.MiningFarmSpawnPoint,
                _locationProvider.MiningFarmSpawnPoint.position);

            MiningFarmSO data = _gameStaticDataService.GetSO<MiningFarmSO>();
            PrepareItem(item, 0, data.ProfitPerMinute, false,
                data.MinTemperature, data.MidTemperature, data.MaxTemperature);

            MiningFarmData targetItemData = item.ToData();
            _worldDataService.WorldData.MiningFarmDatas.TryAdd(targetItemData.Id, targetItemData);

            return item;
        }

        public void SetWorkingMinutes(string id, int minutes) =>
            _worldDataService.WorldData.MiningFarmDatas[id].WorkingMinutes = minutes;

        public void SetProfit(int amount) =>
            _walletService.Set(ItemTypeId.Money, amount);

        public void SetNeedClean(string id, bool needClean)
        {
            _worldDataService.WorldData.MiningFarmDatas[id].NeedClean = needClean;
            _createdFarms[id].SetNeedClean(needClean);
        }

        public void SetNeedCleanToData(string id, bool needClean)
        {
            _worldDataService.WorldData.MiningFarmDatas[id].NeedClean = needClean;
            
            if(!needClean)
                _worldTimeService.SaveMiningFarmLastCleanTime(id);
        }

        private void HandleTimeDifference()
        {
            foreach (MiningFarmItem miningFarmItem in _createdFarms.Values)
            {
                var timeDifference = _worldTimeService.GetMiningFarmLastCleanTime(miningFarmItem.Id);
                timeDifference = Mathf.Clamp(timeDifference, 0, TimeConstantValue.MinutesInTwoHour);
                HandleTimeDifference(timeDifference, miningFarmItem.Id);
            }
        }

        private bool HasCreatedFarms() =>
            _createdFarms.Count != 0;

        private void ConfigureMiningFarms(Dictionary<string, MiningFarmData> miningFarmDatas)
        {
            MiningFarmSO miningFarmSo = _gameStaticDataService.GetSO<MiningFarmSO>();

            foreach (MiningFarmItem miningFarmItem in _createdFarms.Values)
            {
                MiningFarmData targetData = miningFarmDatas[miningFarmItem.Id];
                PrepareItem(miningFarmItem, targetData.WorkingMinutes, targetData.ProfitPerMinute, targetData.NeedClean,
                    miningFarmSo.MinTemperature, miningFarmSo.MidTemperature, miningFarmSo.MaxTemperature);
            }
        }

        private void HandleTimeDifference(int timeDifference, string id)
        {
            if (TrySetNeedClean(timeDifference, id))
                return;

            SetWorkingMinutesToFarms(timeDifference, id);
        }

        private void SetWorkingMinutesToFarms(int timeDifference, string id)
        {
            MiningFarmItem miningFarmItem = _createdFarms[id];
            var workingMinutes = Mathf.Clamp(miningFarmItem.WorkingMinutes + timeDifference, 0, TimeConstantValue.MinutesInTwoHour);
            miningFarmItem.Init(workingMinutes, this);
        }

        private bool TrySetNeedClean(int timeDifference, string id)
        {
            if (timeDifference != TimeConstantValue.MinutesInTwoHour)
                return false;

            MiningFarmItem targetFarm = _createdFarms[id];
            SetNeedClean(id, true);
            SetMoneyToWalletService(timeDifference, targetFarm);
            return true;
        }

        private void SetMoneyToWalletService(int timeDifference, MiningFarmItem targetFarm) =>
            _walletService.Set(ItemTypeId.Money, targetFarm.ProfitPerMinute * timeDifference);

        private void PrepareItem(MiningFarmItem miningFarmItem,
            int workingMinutes,
            int profitPerMinute,
            bool needClean,
            int minTemp,
            int midTemp,
            int maxTemp)
        {
            miningFarmItem.WorkingMinutes = workingMinutes;
            miningFarmItem.ProfitPerMinute = profitPerMinute;
            miningFarmItem.SetTemperatures(minTemp, midTemp, maxTemp);
            miningFarmItem.SetNeedClean(needClean);
        }

        private void CreateMiningFarmsFromData(Dictionary<string, MiningFarmData> miningFarmDatas)
        {
            foreach (MiningFarmData miningFarmData in miningFarmDatas.Values)
            {
                var miningFarm = _gameItemFactory.Create<MiningFarmItem>(_locationProvider.MiningFarmSpawnPoint,
                    miningFarmData.Position.ToVector());
                Debug.Log(miningFarmData.LastCleanTime.ToDateTime());
                _createdFarms[miningFarm.Id] = miningFarm;
            }
        }
    }
}