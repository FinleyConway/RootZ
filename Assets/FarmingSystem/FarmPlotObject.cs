using System.Collections.Generic;
using UnityEngine;

public class FarmPlotObject
{
    private int _xPosition;
    private int _zPosition;
    private Grid3D<FarmPlotObject> _grid;
    private FarmPlotPlacedObject _placedObject;

    private int _growthAmount = 0;
    private int _currentGrowthDay = 0;

    public FarmPlotObject(Grid3D<FarmPlotObject> grid, int x, int z)
    {
        _grid = grid;
        _xPosition = x;
        _zPosition = z;
    }

    public FarmPlotPlacedObject GetPlacedObject()
    {
        return _placedObject;
    }

    public bool IsTileEmpty()
    {
        return _placedObject == null;
    }

    public void SetTileObject(FarmPlotPlacedObject transform)
    {
        _placedObject = transform;
        _growthAmount = _placedObject.GetGrowthStages().GetGrowths().Count;
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }

    public void RemoveTileObject()
    {
        _placedObject = null;
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }

    public void Grow()
    {
        if (_currentGrowthDay >= _growthAmount - 1) return;

        List<Transform> list = _placedObject.GetGrowthStages().GetGrowths();

        list[_currentGrowthDay].gameObject.SetActive(false);

        _currentGrowthDay++;

        list[_currentGrowthDay].gameObject.SetActive(true);
    }
}
