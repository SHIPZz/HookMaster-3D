using System;
using System.Collections;
using CodeBase.Enums;
using CodeBase.Gameplay.Effects;
using CodeBase.Services.GOPool;
using CodeBase.Services.Providers.Player;
using UnityEngine;
using Zenject;

public class ExtinguisherSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem _traceSmokeEffect;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnCount = 10;
    [SerializeField] private float _spawnInterval = 0.3f;
    [SerializeField] private AudioSource _sound;

    private EffectPool _effectPool;
    private PlayerProvider _playerProvider;
    private readonly WaitForSeconds _second = new WaitForSeconds(1f);
    private Coroutine _coroutine;

    [Inject]
    private void Construct(EffectPool effectPool, PlayerProvider playerProvider)
    {
        _playerProvider = playerProvider;
        _effectPool = effectPool;
    }

    public void Activate(Action onFinishedCallback = null)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SpawnSmokeEffects(onFinishedCallback));
    }

    private IEnumerator SpawnSmokeEffects(Action onFinishedCallback = null)
    {
        _sound.Play();

        for (int i = 0; i < _spawnCount; i++)
        {
            var effectView = _effectPool.Pop<SmokeEffectView>(EffectTypeId.SmokeBlow);
            effectView.SetPool(_effectPool);
            effectView.transform.rotation = Quaternion.LookRotation(_playerProvider.PlayerMovement.transform.forward);
            effectView.transform.localPosition = _spawnPoint.position;
            _traceSmokeEffect.Play();
            effectView.Play();
            yield return new WaitForSeconds(_spawnInterval);
        }

        _traceSmokeEffect.Stop();
        _sound.Stop();

        yield return _second;
        onFinishedCallback?.Invoke();
    }
}