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
        placedObject._root = placedObjectTransform.GetComponentInChildren<Root>();

        return placedObject;
    }

    private PlantSO _placedObjectTypeSO;
    private GridObjetTypeSO.Dir _dir;
    private Vector2Int _origin;
    private Root _root;

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

    public PlantSO GetObjectData()
    {
        return _placedObjectTypeSO;
    }

    public Root GetRoot()
    {
        return _root;
    }
}