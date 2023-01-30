using System;
using UnityEngine;

public class Grid3D<T>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    private int _rows;
    private int _columns;
    private int _cellSize;
    private Vector3 _originPosition;

    private T[,] _grid;

    public Grid3D(int rows, int columns, int cellSize, Vector3 originPosition, Func<Grid3D<T>, int, int, T> CreateGridObject)
    {
        _rows = rows;
        _columns = columns;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _grid = new T[rows, columns];
        for (int x = 0; x < _rows; x++)
        {
            for (int z = 0; z < _columns; z++)
            {
                _grid[x, z] = CreateGridObject(this, x, z);
            }
        }
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public int GetRow() => _rows;

    public int GetColumns() => _columns;

    public int GetCellSize() => _cellSize;

    public void ConvertWorldToGrid(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
    }

    public Vector3 ConvertGridToWorld(int x, int z) => new Vector3(x, 0, z) * _cellSize + _originPosition;

    public void SetValue(int x, int z, T value)
    {
        if (x >= 0 && z >= 0 && x < _rows && z < _columns)
        {
            _grid[x, z] = value;
            TriggerGridObjectChanged(x, z);
        }
    }

    public void SetValue(Vector3 worldPosition, T value)
    {
        ConvertWorldToGrid(worldPosition, out int x, out int z);
        SetValue(x, z, value);
    }

    public T GetObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < _rows && z < _columns) return _grid[x, z];
        else return default;
    }

    public T GetObject(Vector3 worldPosition)
    {
        ConvertWorldToGrid(worldPosition, out int x, out int z);
        return GetObject(x, z);
    }

    public bool IsInsideGrid(int x, int z)
    {
        return x >= 0 && x < _rows && z >= 0 && z < _columns;
    }
}
