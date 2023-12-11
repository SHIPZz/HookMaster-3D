using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.SO.Player;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class PlayerStaticDataService
    {
        private readonly Dictionary<CharacterTypeId, PlayerSO> _playerDatas;

        public PlayerStaticDataService()
        {
            _playerDatas = Resources.LoadAll<PlayerSO>("Datas/Player")
                .ToDictionary(x => x.characterTypeId, x => x);
        }

        public PlayerSO Get(CharacterTypeId characterTypeId) =>
            _playerDatas[characterTypeId];
    }
}