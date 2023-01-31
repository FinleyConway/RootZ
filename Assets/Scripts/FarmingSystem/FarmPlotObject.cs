using System.Collections;
using UnityEngine;

public class FarmPlotObject
{
    private int _xPosition;
    private int _zPosition;
    private Grid3D<FarmPlotObject> _grid;
    private FarmPlotPlacedObject _placedObject;

    public bool IsGrowing { get; private set; } = false;
    public bool IsDoneGrowing { get; private set; }= false;
    public bool IsInfected { get; set; } = false;

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
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }

    public void RemoveTileObject()
    {
        IsGrowing = false;
        IsInfected = false;
        IsDoneGrowing = false;
        _placedObject = null;
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }

    public IEnumerator Grow()
    {
        IsGrowing = true;
        
        float duration = _placedObject.GetPlacedObjectTypeSO().GrowthTime;
        yield return new WaitForSeconds(duration);

        IsDoneGrowing = true;
    }
}
