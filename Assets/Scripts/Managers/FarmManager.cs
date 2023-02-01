using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [Header("Farm Graphics")]
    [SerializeField] private Transform _farmSoil;

    [Header("Grid")]
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    [SerializeField] private int _cellSize;
    [SerializeField] private bool _drawDebug = true;
    private List<FarmPlotObject> _currentActiveCrops = new List<FarmPlotObject>();

    public Grid3D<FarmPlotObject> Grid { get; private set; }

    private void Awake()
    {
        Grid = new Grid3D<FarmPlotObject>(_rows, _columns, _cellSize, transform.position, (Grid3D<FarmPlotObject> g, int x, int z) => new FarmPlotObject(g, x, z));

        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _columns; y++)
            {
                Transform soil = Instantiate(_farmSoil);
                soil.SetParent(transform);

                soil.position = new Vector3(x + transform.position.x, 0, y + transform.position.z) * _cellSize;
            }
        }
    }

    private void OnEnable()
    {
        FarmingInteractingSystem.OnPlantCrop += PlaceCrop;
    }

    private void OnDestroy()
    {
        FarmingInteractingSystem.OnPlantCrop -= PlaceCrop;
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

    // get all crops on the farm before the games starts
    public void GetPlantedCropsAmount()
    {
        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _columns; y++)
            {
                if (!Grid.GetGrid()[x, y].IsTileEmpty())
                {
                    _currentActiveCrops.Add(Grid.GetGrid()[x, y]);
                }
            }
        }
    }

    public List<FarmPlotObject> GetActiveCrops()
    {
        return _currentActiveCrops;
    }

    public void RemoveActiveObjet(FarmPlotObject plot)
    {
        _currentActiveCrops.Remove(plot);
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
