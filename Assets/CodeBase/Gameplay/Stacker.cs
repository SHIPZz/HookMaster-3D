using System;
using CodeBase.Gameplay.AnimMovement;
using UnityEngine;

public class Stacker : MonoBehaviour
{
    [SerializeField] private int _xScale = 5; // xyz count of object in stack, 5x5x5 objects here 
    [SerializeField] private int _yScale = 5;
    [SerializeField] private int _zScale = 5;

    [SerializeField] private float _xCellScale = 1; //if stack entry is not a cube
    [SerializeField] private float _yCellScale = 1;
    [SerializeField] private float _zCellScale = 1;

    [SerializeField] private float _gap = 0.7f;

    private GameObject[] _objects;
    private int _currentIndex;

    private void Awake()
    {
        _objects = new GameObject[_xScale * _yScale * _zScale];
    }

    public void SetCurrentIndexZero() => _currentIndex = 0;

    public void AddToStack(GameObject prefab)
    {
        if (_currentIndex >= _objects.Length)
            return;

        Vector3 position = GetPosition();

        _currentIndex++;
    }

    private Vector3 GetPosition()
    {
        Vector3 index3d = Get3dIndex();

        index3d.x *= _xCellScale + _gap;
        index3d.y *= _yCellScale + _gap; //y * 1 + y *2
        index3d.z *= _zCellScale + _gap;
        return index3d;
    }

    private Vector3Int Get3dIndex()
    {
        // Input: k in N(ABC)
        // Output: (x, y, z) in N(A) x N(B) x N(C)

        // N(ABC) -> N(A) x N(BC) 15

        int y = _currentIndex / (_yScale * _xScale); // y in N(A)
        float w = _currentIndex % (_yScale * _xScale); // w in N(BC)

        // N(BC) -> N(B) x N(C)
        int x = (int)(w / _xScale); // y in N(B)
        int z = (int)(w % _xScale); // z in N(C)
        return new Vector3Int(x, y, z);
    }
}