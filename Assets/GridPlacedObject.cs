using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacedObject : MonoBehaviour
{
    public static GridPlacedObject Create(Vector3 worldPosition, Vector2Int origin, GridObjetTypeSO.Dir dir, GridObjetTypeSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.Prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        GridPlacedObject placedObject = placedObjectTransform.GetComponent<GridPlacedObject>();
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        placedObject.Setup();

        return placedObject;
    }

    private GridObjetTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private GridObjetTypeSO.Dir dir;

    protected virtual void Setup()
    {
        //Debug.Log("PlacedObject.Setup() " + transform);
    }

    public virtual void GridSetupDone()
    {
        //Debug.Log("PlacedObject.GridSetupDone() " + transform);
    }

    /*
    protected virtual void TriggerGridObjectChanged()
    {
        foreach (Vector2Int gridPosition in GetGridPositionList())
        {
            GridBuildingSystem3D.Instance.GetGridObject(gridPosition).TriggerGridObjectChanged();
        }
    }
    */


    public Vector2Int GetGridPosition()
    {
        return origin;
    }

    public void SetOrigin(Vector2Int origin)
    {
        this.origin = origin;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public GridObjetTypeSO.Dir GetDir()
    {
        return dir;
    }

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return placedObjectTypeSO.Name;
    }

    public GridObjetTypeSO GetPlacedObjectTypeSO()
    {
        return placedObjectTypeSO;
    }
}