using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    [SerializeField] private int _cellSize;
    [SerializeField] private bool _drawDebug = true;

    private Grid3D<GridObject> _grid;
    private GridObjetTypeSO.Dir _currentDir = GridObjetTypeSO.Dir.Down;

    private void Awake()
    {
        _grid = new Grid3D<GridObject>(_rows, _columns, _cellSize, transform.position, (Grid3D<GridObject> g, int x, int z) => new GridObject(g, x, z));
    }

    private void OnEnable()
    {
        FarmingInteractingSystem.OnPlantCrop += PlaceGridObject;
        FarmingInteractingSystem.OnTryHarvest += RemoveGridObject;
    }

    private void OnDestroy()
    {
        FarmingInteractingSystem.OnPlantCrop -= PlaceGridObject;
        FarmingInteractingSystem.OnTryHarvest -= RemoveGridObject;
    }

    private void PlaceGridObject(Vector3 worldPosition, GridObjetTypeSO plant)
    {
        _grid.ConvertWorldToGrid(worldPosition, out int x, out int z);

        List<Vector2Int> gridPositions = plant.GetGridPositionList(new Vector2Int(x, z), _currentDir);

        bool canBuild = true;
        foreach (Vector2Int position in gridPositions)
        {
            if (!_grid.IsInsideGrid(position.x, position.y))
            {
                canBuild = false;
                break;
            }

            if (!_grid.GetObject(position.x, position.y).IsTileEmpty())
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            Vector2Int rotationOffset = plant.GetRotationOffset(_currentDir);
            Vector3 placedObjectWorldPosition = _grid.ConvertGridToWorld(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.GetCellSize();
            GridPlacedObject placedObject = GridPlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), _currentDir, plant);

            foreach (Vector2Int position in gridPositions)
            {
                _grid.GetObject(position.x, position.y).SetTileObject(placedObject);
            }
        }
        else
        {
            print("cant place here");
        }
    }

    private void RemoveGridObject(Vector3 worldPosition)
    {
        _grid.ConvertWorldToGrid(worldPosition, out int x, out int z);
        if (_grid.IsInsideGrid(x, z))
        {
            GridObject gridObject = _grid.GetObject(worldPosition);
            GridPlacedObject placedObject = gridObject.GetPlacedObject();
            if (placedObject != null)
            {
                placedObject.DestroySelf();

                List<Vector2Int> gridPositions = placedObject.GetGridPositionList();
                foreach (Vector2Int position in gridPositions)
                {
                    _grid.GetObject(position.x, position.y).RemoveTileObject();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_drawDebug) return;

        Gizmos.color = Color.green;
        for (float x = 0; x < _rows; x++)
        {
            for (float z = 0; z < _columns; z++)
            {
                Vector3 point1 = transform.TransformPoint(new Vector3(x, 0, z) * _cellSize);
                Vector3 point2 = transform.TransformPoint(new Vector3(x, 0, z + 1) * _cellSize);
                Gizmos.DrawLine(point1, point2);

                point2 = transform.TransformPoint(new Vector3(x + 1, 0, z) * _cellSize);
                Gizmos.DrawLine(point1, point2);
            }
        }

    }

    private void OnGUI()
    {
        if (!_drawDebug) return;

        GUI.Label(new Rect(5, 5, 50, 50), _currentDir.ToString());
    }
}
