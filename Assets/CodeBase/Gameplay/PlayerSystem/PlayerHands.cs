using System.Collections;
using CodeBase.Gameplay.PaperS;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerHands : MonoBehaviour
    {
        [SerializeField] private PlayerPaperContainer _playerPaperContainer;

        private Coroutine _coroutine;
        private PlayerIKService _playerIKService;

        [Inject]
        private void Construct(PlayerIKService playerIKService)
        {
            _playerIKService = playerIKService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out TableHolder tableHolder))
                return;

            _coroutine = StartCoroutine(StartTransferCoroutine(tableHolder));
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out TableHolder tableHolder))
            {
                tableHolder.IsItemAdding = false;
                
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
            }

            if (!_playerPaperContainer.HasPapers)
                CleanUp();
        }

        private IEnumerator StartTransferCoroutine(TableHolder tableHolder)
        {
            while (_playerPaperContainer.HasPapers)
            {
                tableHolder.IsItemAdding = true;
                yield return tableHolder.PutAsync(_playerPaperContainer.Pop());
            }

            tableHolder.IsItemAdding = false;
            CleanUp();
        }

        private void CleanUp()
        {
            _playerPaperContainer.Clear();
            _playerIKService.ClearIKHandTargets().Forget();
        }
    }
}