using System;

public class FarmPlotObject
{
    private int _xPosition;
    private int _zPosition;
    private Grid3D<FarmPlotObject> _grid;
    private FarmPlotPlacedObject _placedObject;

    public bool IsInfecting { get; set; } = false;

    public static event Action<FarmPlotObject, bool> OnRemovePlot;

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
        transform.GetRoot().OnDestroy += KillPlant;

        _placedObject = transform;
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }

    private void KillPlant(bool killedByPlayer)
    {
        if (!killedByPlayer)
        {
            OnRemovePlot?.Invoke(this, killedByPlayer);
            _placedObject.DestroySelf();
            RemoveTileObject();
        }
        else
        {
            OnRemovePlot?.Invoke(this, killedByPlayer);
        }
    }

    public void RemoveTileObject()
    {
        _placedObject.GetRoot().OnDestroy -= KillPlant;

        _placedObject = null;
        _grid.TriggerGridObjectChanged(_xPosition, _zPosition);
    }
}
