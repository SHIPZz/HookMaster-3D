using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.DataService;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using CodeBase.SO.GameItem.MiningFarm;
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

        private List<MiningFarmItem> _createdFarms = new();

        public event Action Stopped;

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

            foreach (MiningFarmData miningFarmData in miningFarmDatas.Values)
            {
                var miningFarm = _gameItemFactory.Create<MiningFarmItem>(_locationProvider.MiningFarmSpawnPoint,
                    miningFarmData.Position.ToVector());

                _createdFarms.Add(miningFarm);
            }

            if (_createdFarms.Count == 0)
                return;

            ConfigureMiningFarms(_createdFarms, miningFarmDatas);

            var timeDifference = _worldTimeService.GetLastVisitedTimeByMinutes();
            timeDifference = Mathf.Clamp(timeDifference, 0, TimeConstantValue.MinutesInTwoHour);

            if (HandleTimeDifference(timeDifference, _createdFarms))
                return;

            Stopped?.Invoke();
        }

        public MiningFarmItem CreateMiningFarm()
        {
            var item = _gameItemFactory.Create<MiningFarmItem>(_locationProvider.MiningFarmSpawnPoint,
                _locationProvider.MiningFarmSpawnPoint.position);
            MiningFarmSO data = _gameStaticDataService.GetSO<MiningFarmSO>();
            PrepareItem(item, 0, data.ProfitPerMinute, false,
                data.MinTemperature, data.MidTemperature, data.MaxTemperature);

            MiningFarmData targetItemData = item.ToData();
            _worldDataService.WorldData.MiningFarmDatas.Add(targetItemData.Id, targetItemData);

            return item;
        }

        public void SetWorkingMinutes(string id, int minutes)
        {
            _worldDataService.WorldData.MiningFarmDatas[id].WorkingMinutes = minutes;
        }

        public void SetProfit(int amount) =>
            _walletService.Set(ItemTypeId.Money, amount);

        public void SetNeedClean(string id, bool needClean)
        {
            _worldDataService.WorldData.MiningFarmDatas[id].NeedClean = needClean;
        }

        public void ResetLastMiningTime()
        {
            _worldTimeService.ResetLastMiningTime();
        }

        private void ConfigureMiningFarms(List<MiningFarmItem> createdMiningFarms,
            Dictionary<string, MiningFarmData> miningFarmDatas)
        {
            MiningFarmSO data = _gameStaticDataService.GetSO<MiningFarmSO>();

            foreach (MiningFarmItem miningFarmItem in createdMiningFarms)
            {
                MiningFarmData targetData = miningFarmDatas[miningFarmItem.Id];
                PrepareItem(miningFarmItem, targetData.WorkingMinutes, targetData.ProfitPerMinute, targetData.NeedClean,
                    data.MinTemperature, data.MidTemperature, data.MaxTemperature);
            }
        }

        private bool HandleTimeDifference(int timeDifference, List<MiningFarmItem> createdMiningFarms)
        {
            if (timeDifference == TimeConstantValue.MinutesInTwoHour)
            {
                createdMiningFarms.ForEach(x =>
                {
                    SetNeedClean(x.Id, true);
                    _walletService.Set(ItemTypeId.Money, x.ProfitPerMinute * timeDifference);
                });

                Stopped?.Invoke();
                return true;
            }

            foreach (MiningFarmItem miningFarmItem in createdMiningFarms)
            {
                if (miningFarmItem.WorkingMinutes == TimeConstantValue.MinutesInTwoHour)
                    continue;

                var workingMinutes = Mathf.Clamp(miningFarmItem.WorkingMinutes + timeDifference, 0,
                    TimeConstantValue.MinutesInTwoHour);
                miningFarmItem.Init(workingMinutes, this);
            }

            return true;
        }

        public MiningFarmItem Get(string id) =>
            _createdFarms.FirstOrDefault(x => x.Id == id);

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
            miningFarmItem.NeedClean = needClean;
            miningFarmItem.SetTemperatures(minTemp, midTemp, maxTemp);
        }
    }
}