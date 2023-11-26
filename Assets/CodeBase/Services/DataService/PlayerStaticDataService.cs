using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.SO.Player;
using UnityEngine;

namespace CodeBase.Services.Data
{
    public class PlayerStaticDataService
    {
        private readonly Dictionary<PlayerTypeId, PlayerSO> _playerDatas;

        public PlayerStaticDataService()
        {
            _playerDatas = Resources.LoadAll<PlayerSO>("Datas/Player")
                .ToDictionary(x => x.PlayerTypeId, x => x);
        }

        public PlayerSO Get(PlayerTypeId playerTypeId) =>
            _playerDatas[playerTypeId];
    }
}