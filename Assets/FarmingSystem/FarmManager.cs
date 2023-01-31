using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    [SerializeField] private int _cellSize;
    [SerializeField] private bool _drawDebug = true;

    public Grid3D<FarmPlotObject> Grid { get; private set; }

    private void Awake()
    {
        Grid = new Grid3D<FarmPlotObject>(_rows, _columns, _cellSize, transform.position, (Grid3D<FarmPlotObject> g, int x, int z) => new FarmPlotObject(g, x, z));
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
        UpdateAllCrops();
    }

    // place item on grid
    private void PlaceCrop(Vector3 worldPosition, PlantSO crop)
    {
        Grid.ConvertWorldToGrid(worldPosition, out int x, out int z);

        // check if the world position is within the grid
        if (Grid.IsInsideGrid(x, z))
        {
            // get all grid positions of the object
            List<Vector2Int> gridPositions = crop.GetGridPositionList(new Vector2Int(x, z), GridObjetTypeSO.Dir.Down);

            // iterate though each position to see if its a valid position
            bool canBuild = true;
            foreach (Vector2Int position in gridPositions)
            {
                if (!Grid.IsInsideGrid(position.x, position.y))
                {
                    canBuild = false;
                    break;
                }

                if (!Grid.GetObject(position.x, position.y).IsTileEmpty())
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
                Vector3 placedObjectWorldPosition = Grid.ConvertGridToWorld(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * Grid.GetCellSize();
                FarmPlotPlacedObject placedObject = FarmPlotPlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), GridObjetTypeSO.Dir.Down, crop);

                foreach (Vector2Int position in gridPositions)
                {
                    Grid.GetObject(position.x, position.y).SetTileObject(placedObject);
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
        Grid.ConvertWorldToGrid(worldPosition, out int x, out int z);
        if (Grid.IsInsideGrid(x, z) && Grid.GetObject(worldPosition).IsDoneGrowning)
        {
            FarmPlotObject gridObject = Grid.GetObject(worldPosition);
            FarmPlotPlacedObject placedObject = gridObject.GetPlacedObject();
            if (placedObject != null)
            {
                placedObject.DestroySelf();

                List<Vector2Int> gridPositions = placedObject.GetGridPositionList();
                foreach (Vector2Int position in gridPositions)
                {
                    Grid.GetObject(position.x, position.y).RemoveTileObject();
                }
            }
        }
    }

    public void UpdateAllCrops()
    {
        FarmPlotObject[,] grid = Grid.GetGrid();
        for (int x = 0; x < Grid.GetRow(); x++)
        {
            for (int y = 0; y < Grid.GetColumns(); y++)
            {
                if (!grid[x, y].IsTileEmpty() && !grid[x, y].IsGrowing)
                {
                    StartCoroutine(grid[x, y].Grow());
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
}
