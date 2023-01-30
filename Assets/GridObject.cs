using UnityEngine;

public class GridObject
{
    Grid3D<GridObject> _grid;
    private int _xPosition;
    private int _zPosition;
    private GridPlacedObject _placedObject;

    public GridObject(Grid3D<GridObject> grid, int x, int z)
    {
        _grid = grid;
        _xPosition = x;
        _zPosition = z;
    }

    public GridPlacedObject GetPlacedObject()
    {
        return _placedObject;
    }

    public bool IsTileEmpty()
    {
        return _placedObject == null;
    }

    public void SetTileObject(GridPlacedObject transform)
    {
        _placedObject = transform;
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }

    public void RemoveTileObject()
    {
        _placedObject = null;
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }
}
