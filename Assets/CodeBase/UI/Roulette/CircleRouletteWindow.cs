using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using CodeBase.Services.TriggerObserve;
using CodeBase.UI;
using CodeBase.UI.Roulette;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CircleRouletteWindow : WindowBase
{
    [SerializeField] private float _minRotationZ = 180f;
    [SerializeField] private float _maxRotationZ = 720f;
    [SerializeField] private float _rotateDuration = 1.5f;
    [SerializeField] private Transform _target;
    [SerializeField] private CanvasAnimator _canvasAnimator;
    [SerializeField] private Button _rotateButton;
    [SerializeField] private float _rotateCount = 5;
    [SerializeField] private TMP_Text _rotateCountText;
    [SerializeField] private List<RouletteItem> _rouletteItems;
    [SerializeField] private TriggerObserver _arrowTrigger;

    private Vector3 _lastRotation;

    private Coroutine _rotateCoroutine;
    private RouletteItem _lastDroppedRouletteItem;

    private void OnEnable()
    {
        _rotateButton.onClick.AddListener(Rotate);
        _arrowTrigger.TriggerEntered += SetLastDroppedObject;
        _arrowTrigger.TriggerEntered += SetLastDroppedObject;
        _rouletteItems.ForEach(x=>x.Init());
    }

    private void OnDisable()
    {
        _rotateButton.onClick.RemoveListener(Rotate);
        _arrowTrigger.TriggerEntered -= SetLastDroppedObject;
    }

    private void SetLastDroppedObject(Collider obj)
    {
        _lastDroppedRouletteItem = obj.GetComponent<RouletteItem>();
    }

    public override void Open()
    {
        _canvasAnimator.FadeInCanvas();
    }

    private void Rotate()
    {
        _rotateCount = Mathf.Clamp(_rotateCount--, 0, _rotateCount);
        _rotateCountText.text = _rotateCount.ToString(CultureInfo.InvariantCulture);

        _rotateButton.interactable = false;
        
        if(_rotateCount == 0)
            _rotateButton.gameObject.SetActive(false);

        if (_rotateCoroutine != null)
            StopCoroutine(_rotateCoroutine);

        _rotateCoroutine = StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        _lastRotation = _target.localEulerAngles;

        float randomRotationZ = Random.Range(_minRotationZ, _maxRotationZ);
        Vector3 targetRotation = _lastRotation + new Vector3(0, 0, randomRotationZ);

        float elapsedTime = 0f;
        float startRotationZ = _lastRotation.z;

        while (elapsedTime < _rotateDuration)
        {
            float t = elapsedTime / _rotateDuration;

            float easedT = Mathf.SmoothStep(0, 1, t);

            float currentRotationZ = Mathf.Lerp(startRotationZ, targetRotation.z, easedT);

            _target.localEulerAngles = new Vector3(0, 0, currentRotationZ);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        
        _target.localEulerAngles = targetRotation;
        _rotateButton.interactable = true;
        print(_lastDroppedRouletteItem.RouletteItemTypeId);
    }
}