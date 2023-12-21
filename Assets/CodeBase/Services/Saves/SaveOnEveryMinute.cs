using System.Collections;
using CodeBase.Services.Coroutine;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Services.Saves
{
    public class SaveOnEveryMinute
    {
        private readonly WaitForSecondsRealtime _waitMinute = new(60f);
        private readonly IWorldDataService _worldDataService;
        private readonly ICoroutineRunner _coroutineRunner;

        public SaveOnEveryMinute(IWorldDataService worldDataService, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
        }

        public void Init() => 
            _coroutineRunner.StartCoroutine(StartSaveCoroutine());

        private IEnumerator StartSaveCoroutine()
        {
            while (true)
            {
                yield return _waitMinute;
                _worldDataService.Save();
            }
        }
    }
}