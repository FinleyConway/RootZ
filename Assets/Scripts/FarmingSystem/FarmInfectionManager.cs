using System.Collections.Generic;
using UnityEngine;

public class FarmInfectionManager : MonoBehaviour 
{
    [SerializeField, Range(1, 4)] private float _difficultLevel;
    private List<FarmPlotObject> _currentActiveCrops = new List<FarmPlotObject>();
    private int _currentInfectionAmount;
    private FarmManager _farm;

    private void Awake()
    {
        _farm = GetComponent<FarmManager>();
    }   

    // if difficult is 2
    // 10 crops / difficult would make 5 crops prone to infection
    public int CalculateActiveInfection()
    {
        float infection = _currentActiveCrops.Count / _difficultLevel;
        return Mathf.RoundToInt(infection);
    }

    // get all crops on the farm before the games starts
    public void GetPlantedCropsAmount(Grid3D<FarmPlotObject> grid)
    {
        for (int x = 0; x < grid.GetRow(); x++)
        {
            for (int y = 0; y < _farm.Grid.GetColumns(); y++)
            {
                if (!grid.GetGrid()[x, y].IsTileEmpty())
                {
                    _currentActiveCrops.Add(grid.GetGrid()[x, y]);
                }
            }
        }
    }

    // infects a random crop when called
    public void SetRandomInfected()
    {
        int random = Random.Range(0, _currentActiveCrops.Count);
        _currentActiveCrops[random].IsInfected = true;
    }

    public void DecreaseCurrentInfection()
    {
        _currentInfectionAmount--;
    }
    public void IncreaseCurrentInfection()
    {
        _currentInfectionAmount++;
    }
}