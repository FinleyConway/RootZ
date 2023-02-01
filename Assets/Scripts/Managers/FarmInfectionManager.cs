using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FarmInfectionManager : MonoBehaviour 
{
    [SerializeField, Range(1, 4)] private float _difficultLevel;
    [SerializeField] private float _spawnRate = 0.5f;
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
    }

    private IEnumerator InitWave()
    {
        List<FarmPlotObject> crops = _farm.GetActiveCrops();

        if (crops.Count > 0)
        {
            for (int i = 0; i < _currentMaxInfectionAmount; i++)
            {
                yield return new WaitForSeconds(_spawnRate);
                crops[Random.Range(0, crops.Count)].GetPlacedObject().GetRoot().TriggerGrow();
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
            crops[Random.Range(0, crops.Count)].GetPlacedObject().GetRoot().TriggerGrow();

        }
        else
        {
            print("No more crops to infect");
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