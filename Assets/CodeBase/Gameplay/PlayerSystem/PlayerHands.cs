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

        private TableHolder _tableHolder;

        [Inject]
        private void Construct(PlayerIKService playerIKService) =>
            _playerIKService = playerIKService;

        private void Start() =>
            StartCoroutine(StartTransferCoroutine());

        private void OnEnable() =>
            _playerPaperContainer.Cleared += CleanUp;

        private void OnDisable() =>
            _playerPaperContainer.Cleared -= CleanUp;

        private IEnumerator StartTransferCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);

                yield return new WaitUntil(() => _tableHolder != null);

                if (_playerPaperContainer.HasPapers)
                {
                    _tableHolder.IsItemAdding = true;
                    yield return _tableHolder.PutCoroutine(_playerPaperContainer.Pop());
                }
                else
                {
                    if (_tableHolder != null)
                        _tableHolder.IsItemAdding = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out TableHolder tableHolder))
                return;

            if (!tableHolder.CanPut)
                return;

            _tableHolder = tableHolder;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out TableHolder tableHolder))
                return;

            if (!tableHolder.CanPut)
                return;

            _tableHolder = null;

            tableHolder.IsItemAdding = false;
        }

        private void CleanUp() =>
            _playerIKService.ClearIKHandTargets().Forget();
    }
}