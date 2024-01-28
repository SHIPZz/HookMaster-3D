using CodeBase.Animations;
using CodeBase.Services.Providers.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Money
{
    public class MoneyMovement : MonoBehaviour
    {
        [SerializeField] private float _upwardDuration = 0.3f;
        [SerializeField] private float _jumpPower;
        [SerializeField] private Ease _ease;
        [SerializeField] private TransformScaleAnim _transformScaleAnim;
        private PlayerProvider _playerProvider;
        private Vector3 _target;
        private bool _isMoved;
        private float _jumpTimer;
        private Vector3 _disableVector = new Vector3(0.1f, 0.1f, 0.1f);

        private void Awake()
        {
            _jumpTimer = 0f;
        }

        [Inject]
        private void Construct(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        private void Update()
        {
            _target = _playerProvider.Player.transform.position;

            if (!_isMoved)
                return;

            transform.position = Vector3.Lerp(transform.position, _target + Vector3.up,
                9 * Time.deltaTime);

            if (!(Vector3.Distance(transform.position, _target) <= 1.5f))
                return;
            
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 3 * Time.deltaTime);
                
            if (transform.localScale.sqrMagnitude < 0.01f) 
                gameObject.SetActive(false);
        }

        public void Move()
        {
            _isMoved = true;
        }
    }
}