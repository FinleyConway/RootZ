using UnityEngine;

public class FarmInfectionManager : MonoBehaviour 
{
    [SerializeField, Range(1, 4)] private float _difficultLevel;
    private int _currentCropsOnFarm;
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
        float infection = _currentCropsOnFarm / _difficultLevel;
        return Mathf.RoundToInt(infection);
    }

    // get all crops on the farm before the games starts
    public void GetPlantedCropsAmount(FarmPlotObject[,] grid)
    {
        for (int x = 0; x < _farm.Grid.GetRow(); x++)
        {
            for (int y = 0; y < _farm.Grid.GetColumns(); y++)
            {
                if (!grid[x, y].IsTileEmpty())
                {
                    _currentCropsOnFarm++;
                }
            }
        }
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