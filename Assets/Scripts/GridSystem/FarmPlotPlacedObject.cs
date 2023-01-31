using System.Collections.Generic;
using UnityEngine;

public class FarmPlotPlacedObject : MonoBehaviour
{
    public static FarmPlotPlacedObject Create(Vector3 worldPosition, Vector2Int origin, GridObjetTypeSO.Dir dir, PlantSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.Prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        FarmPlotPlacedObject placedObject = placedObjectTransform.GetComponent<FarmPlotPlacedObject>();
        placedObject._placedObjectTypeSO = placedObjectTypeSO;
        placedObject._origin = origin;
        placedObject._dir = dir;
        placedObject._growthStages = placedObjectTransform.GetComponentInChildren<GrowthStages>();

        return placedObject;
    }

    private PlantSO _placedObjectTypeSO;
    private GridObjetTypeSO.Dir _dir;
    private Vector2Int _origin;
    private GrowthStages _growthStages;

    public Vector2Int GetGridPosition()
    {
        return _origin;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return _placedObjectTypeSO.GetGridPositionList(_origin, _dir);
    }

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public PlantSO GetPlacedObjectTypeSO()
    {
        return _placedObjectTypeSO;
    }

    public GrowthStages GetGrowthStages()
    {
        return _growthStages;
    }
}