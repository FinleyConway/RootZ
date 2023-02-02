using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FarmInfectionManager : MonoBehaviour 
{
    [SerializeField, Range(1, 4)] private float _difficultLevel;
    [SerializeField] private float _spawnRate = 0.5f;
    [SerializeField] private float _highGrowthResistant;
    [SerializeField] private float _lowGrowthResistant;
    private int _maxInfectionAmount = 0;
    private int _currentMaxInfectionAmount = 0;

    private bool _canInfect;

    private FarmManager _farm;

    private void Awake()
    {
        _farm = GetComponent<FarmManager>();
        _maxInfectionAmount = CalculateActiveInfection(_farm.Grid.GetColumns() * _farm.Grid.GetRow());
    }

    private void OnEnable()
    {
        FarmPlotObject.OnRemovePlot += OnRootDeath;
    }

    private void OnDisable()
    {
        FarmPlotObject.OnRemovePlot -= OnRootDeath;
    }

    public void InitInfection(int cropAmount)
    {
        _canInfect = true;
        _currentMaxInfectionAmount += CalculateActiveInfection(cropAmount);

        if (_currentMaxInfectionAmount > _maxInfectionAmount) _currentMaxInfectionAmount = _maxInfectionAmount;
        StartCoroutine(InitWave());
    }

    public void StopInfection()
    {
        _canInfect = false;
        List<FarmPlotObject> crops = _farm.GetActiveCrops();

        if (crops.Count > 0)
        {
            for (int i = 0; i < crops.Count; i++)
            {
                crops[crops.Count].GetPlacedObject().GetRoot().StopGrow();
            }
        }
    }

    private IEnumerator InitWave()
    {
        List<FarmPlotObject> crops = _farm.GetActiveCrops();

        if (crops.Count > 0)
        {
            for (int i = 0; i < _currentMaxInfectionAmount; i++)
            {
                yield return new WaitForSeconds(_spawnRate);

                FarmPlotObject randPlot = crops[Random.Range(0, crops.Count)];
                randPlot.GetPlacedObject().GetRoot().TriggerGrow(ResistantDecider(randPlot));
            }
        }
        else
        {
            print("No more to infect");
        }
    }

    private void OnRootDeath(FarmPlotObject plot)
    {
        if (!_canInfect) return;

        _farm.RemoveActiveObjet(plot);

        List<FarmPlotObject> crops = _farm.GetActiveCrops();
        if (crops.Count > 0)
        {
            FarmPlotObject randPlot = crops[Random.Range(0, crops.Count)];

            randPlot.GetPlacedObject().GetRoot().TriggerGrow(ResistantDecider(randPlot));
        }
        else
        {
            print("No more crops to infect");
        }
    }

    private float ResistantDecider(FarmPlotObject obj)
    {
        PlantSO.RootResistant type = obj.GetPlacedObject().GetObjectData().RootResistantType;

        if (type == PlantSO.RootResistant.Low)
        {
            return _lowGrowthResistant;
        }
        else
        {
            return _highGrowthResistant;
        }
    }

    // if difficult is 2
    // 10 crops / difficult would make 5 crops prone to infection
    private int CalculateActiveInfection(int cropAmount)
    {
        float infection = cropAmount / _difficultLevel;
        return Mathf.RoundToInt(infection);
    }
}