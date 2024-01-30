using CodeBase.Services.Providers.Player;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Money
{
    public class MoneyMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _duration = 0.5f; 
        private PlayerProvider _playerProvider;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private bool _isMoved;
        private float _timeElapsed;

        [Inject]
        private void Construct(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            _endPosition = _playerProvider.Player.transform.position;
            
            if (!_isMoved)
                return;

            _timeElapsed += Time.deltaTime;

            float t = _timeElapsed / _duration;
            t = Mathf.Clamp01(t);

            Vector3 position = BezierCurve(_startPosition, _endPosition, t);

            transform.position = position;

            if (t >= 0.5f)
            {
                transform.localScale = Vector3.Slerp(transform.localScale, Vector3.zero, _speed * Time.deltaTime);
            }
        }

        public void Move() => 
            _isMoved = true;

        private Vector3 BezierCurve(Vector3 start, Vector3 end, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * start; // (1-t)^3 * P0
            p += 3 * uu * t * start; // 3 * (1-t)^2 * t * P1
            p += 3 * u * tt * end; // 3 * (1-t) * t^2 * P2
            p += ttt * end; // t^3 * P3

            return p;
        }
    }
}