using CodeBase.Gameplay.AnimMovement;
using CodeBase.Gameplay.ResourceItem;
using UnityEngine;

public class ResourceTestStacker : MonoBehaviour
{
    [SerializeField] private int _itemsPerRow = 2;
    [SerializeField] private float _horizontalSpacing = 1f;
    [SerializeField] private float _verticalSpacing = 1f;
    [SerializeField] private Transform _startPosition;

    private int _itemsInRow;

    private Vector3 _lastLeftPosition;
    private Vector3 _lastRightPosition;

    public void CalculateTargetPosition(Resource prefab, out Vector3 position)
    {
        if (TrySetFirstPosition(prefab, out Vector3 targetPosition))
        {
            position = targetPosition;
            return;
        }

        if (_itemsInRow != 0 && _itemsInRow % _itemsPerRow == 0)
        {
            position = SetItemToLeftPosition(prefab);
        }
        else
        {
            position = SetItemToRightPosition(prefab);
        }

        _itemsInRow++;
    }

    public void Clear()
    {
        _itemsInRow = 0;
        _lastLeftPosition = Vector3.zero;
        _lastRightPosition = Vector3.zero;
    }

    private Vector3 SetItemToRightPosition(Resource prefab)
    {
        if (_lastRightPosition != Vector3.zero)
        {
            Vector3 lastRightPosition = _lastRightPosition;
            lastRightPosition.y += _verticalSpacing;

            _lastRightPosition = lastRightPosition;
        }
        else
        {
            var targetPosition = _startPosition.position + new Vector3(_horizontalSpacing, 0, 0);
            _lastRightPosition = targetPosition;
        }

        return _lastRightPosition;
    }

    private Vector3 SetItemToLeftPosition(Resource prefab)
    {
        Vector3 lastLeftPosition = _lastLeftPosition;
        lastLeftPosition.y += _verticalSpacing;

        _lastLeftPosition = lastLeftPosition;
        return _lastLeftPosition;
    }

    private bool TrySetFirstPosition(Resource prefab, out Vector3 targetPosition)
    {
        targetPosition = Vector3.zero;

        if (_itemsInRow == 0)
        {
            _lastLeftPosition = _startPosition.position;
            targetPosition = _lastLeftPosition;
            _itemsInRow++;
            return true;
        }

        return false;
    }
}