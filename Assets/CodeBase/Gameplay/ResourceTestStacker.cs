using CodeBase.Gameplay.AnimMovement;
using CodeBase.Gameplay.ResourceItem;
using UnityEngine;

public class ResourceTestStacker : MonoBehaviour
{
    [SerializeField] private int _itemsPerRow = 2;
    [SerializeField] private float _horizontalSpacing = 1f;
    [SerializeField] private float _verticalSpacing = 1f;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _startAnimPosition;
    [SerializeField] private AfterResourceCreateMovementBehaviour _resourceCurveMovement;
    [SerializeField] private float _stackAnimSpeed = 0.65f;

    private int _itemsInRow;

    private Vector3 _lastLeftPosition;
    private Vector3 _lastRightPosition;

    public void StackItems(Resource prefab)
    {
        if (TrySetFirstPosition(prefab))
            return;

        if (_itemsInRow != 0 && _itemsInRow % _itemsPerRow == 0)
        {
            SetItemToLeftPosition(prefab);
        }
        else
        {
            SetItemToRightPosition(prefab);
        }

        _itemsInRow++;
    }

    public void Clear()
    {
        _itemsInRow = 0;
        _lastLeftPosition = Vector3.zero;
        _lastRightPosition = Vector3.zero;
    }

    private void SetItemToRightPosition(Resource prefab)
    {
        if (_lastRightPosition != Vector3.zero)
        {
            Vector3 lastRightPosition = _lastRightPosition;
            lastRightPosition.y += _verticalSpacing;

            _resourceCurveMovement.Move(prefab, _startAnimPosition.position, () => lastRightPosition, _stackAnimSpeed);
            _lastRightPosition = lastRightPosition;
        }
        else
        {
            var targetPosition = _startPosition.position + new Vector3(_horizontalSpacing, 0, 0);
            _resourceCurveMovement.Move(prefab, _startAnimPosition.position, () => targetPosition, _stackAnimSpeed);
            _lastRightPosition = targetPosition;
        }
    }

    private void SetItemToLeftPosition(Resource prefab)
    {
        Vector3 lastLeftPosition = _lastLeftPosition;
        lastLeftPosition.y += _verticalSpacing;

        _resourceCurveMovement.Move(prefab, _startAnimPosition.position, () => lastLeftPosition, _stackAnimSpeed);
        _lastLeftPosition = lastLeftPosition;
    }

    private bool TrySetFirstPosition(Resource prefab)
    {
        if (_itemsInRow == 0)
        {
            _lastLeftPosition = _startPosition.position;
            _resourceCurveMovement.Move(prefab, _startAnimPosition.position, () => _lastLeftPosition, _stackAnimSpeed);
            _itemsInRow++;
            return true;
        }

        return false;
    }
}