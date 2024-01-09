using System.Collections;
using CodeBase.Enums;
using CodeBase.Gameplay.Effects;
using CodeBase.Services.GOPool;
using UnityEngine;
using Zenject;

public class ExtinguisherSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem _traceSmokeEffect;
    [SerializeField] private float _smokeMoveDistance = 1f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _moveTime = 1f;
    [SerializeField] private float _spawnCount = 10;
    [SerializeField] private float _spawnInterval = 0.3f;

    private readonly WaitForSeconds _second = new WaitForSeconds(1f);
    private EffectPool _effectPool;

    [Inject]
    private void Construct(EffectPool effectPool)
    {
        _effectPool = effectPool;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(SpawnSmokeEffects());
        }
    }

    private IEnumerator SpawnSmokeEffects()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            EffectView effectView = _effectPool.Pop(EffectTypeId.SmokeBlow);
            effectView.transform.position = _spawnPoint.position;
            _traceSmokeEffect.Play();
            effectView.Effect.Play(true);
            effectView.transform.rotation = Quaternion.LookRotation(transform.forward);
            StartCoroutine(MoveSmokeEffect(effectView));
            yield return new WaitForSeconds(_spawnInterval);
        }
        
        _traceSmokeEffect.Stop();
    }

    private IEnumerator MoveSmokeEffect(EffectView effectView)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < _moveTime)
        {
            effectView.transform.Translate(transform.forward * _smokeMoveDistance * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return _second;
        effectView.Effect.Stop();
        _effectPool.Push(effectView);
    }
}