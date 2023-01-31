using System.Collections;
using UnityEngine;

public class FarmPlotObject
{
    private int _xPosition;
    private int _zPosition;
    private Grid3D<FarmPlotObject> _grid;
    private FarmPlotPlacedObject _placedObject;

    private bool _isGrowing = false;
    private bool _isDoneGrowing = false;
    private bool _isInfected = false;

    public FarmPlotObject(Grid3D<FarmPlotObject> grid, int x, int z)
    {
        _grid = grid;
        _xPosition = x;
        _zPosition = z;
    }

    public bool IsGrowing => _isGrowing;

    public bool IsDoneGrowning => _isDoneGrowing;

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
        _isGrowing = false;
        _isInfected = false;
        _isDoneGrowing = false;
        _placedObject = null;
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }

    public IEnumerator Grow()
    {
        _isGrowing = true;
        
        float duration = _placedObject.GetPlacedObjectTypeSO().GrowthTime;
        yield return new WaitForSeconds(duration);

        _isDoneGrowing = true;
        
    }
}
