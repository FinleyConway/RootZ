using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    [SerializeField] private int _cellSize;
    [SerializeField] private bool _drawDebug = true;

    private Grid3D<FarmPlotObject> _grid;

    private void Awake()
    {
        _grid = new Grid3D<FarmPlotObject>(_rows, _columns, _cellSize, transform.position, (Grid3D<FarmPlotObject> g, int x, int z) => new FarmPlotObject(g, x, z));
    }

    private void OnEnable()
    {
        FarmingInteractingSystem.OnPlantCrop += PlaceCrop;
        FarmingInteractingSystem.OnTryHarvest += RemoveCrop;
    }

    private void OnDestroy()
    {
        FarmingInteractingSystem.OnPlantCrop -= PlaceCrop;
        FarmingInteractingSystem.OnTryHarvest -= RemoveCrop;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateAllCrops();
        }
    }

    // place item on grid
    private void PlaceCrop(Vector3 worldPosition, PlantSO crop)
    {
        _grid.ConvertWorldToGrid(worldPosition, out int x, out int z);

        // check if the world position is within the grid
        if (_grid.IsInsideGrid(x, z))
        {
            // get all grid positions of the object
            List<Vector2Int> gridPositions = crop.GetGridPositionList(new Vector2Int(x, z), GridObjetTypeSO.Dir.Down);

            // iterate though each position to see if its a valid position
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

            // create item onto the grid
            if (canBuild)
            {
                print("Placed Crop");

                Vector2Int rotationOffset = crop.GetRotationOffset(GridObjetTypeSO.Dir.Down);
                Vector3 placedObjectWorldPosition = _grid.ConvertGridToWorld(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.GetCellSize();
                FarmPlotPlacedObject placedObject = FarmPlotPlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), GridObjetTypeSO.Dir.Down, crop);

                foreach (Vector2Int position in gridPositions)
                {
                    _grid.GetObject(position.x, position.y).SetTileObject(placedObject);
                }
            }
            // inform the player that they cant
            else
            {
                print("cant place here");
            }
        }
    }

    // remove item from grid
    private void RemoveCrop(Vector3 worldPosition)
    {
        // check if the world position is in the grid
        _grid.ConvertWorldToGrid(worldPosition, out int x, out int z);
        if (_grid.IsInsideGrid(x, z))
        {
            FarmPlotObject gridObject = _grid.GetObject(worldPosition);
            FarmPlotPlacedObject placedObject = gridObject.GetPlacedObject();
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

    public void UpdateAllCrops()
    {
        foreach (FarmPlotObject crop in _grid.GetGrid())
        {
            if (!crop.IsTileEmpty())
            {
                crop.Grow();
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
}
