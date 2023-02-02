using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionManager : MonoBehaviour
{
    [SerializeField, Range(2, 5)] private float _difficultLevel;
    [SerializeField] private float _highGrowthResistant;
    [SerializeField] private float _lowGrowthResistant;
    private int _maxInfection;
    private bool _canInfect;

    private List<FarmPlotObject> _currentlyInfectingPlots = new List<FarmPlotObject>();

    private FarmManager _farm;

    private void Awake()
    {
        _farm = GetComponent<FarmManager>();
    }

    private void OnEnable()
    {
        FarmPlotObject.OnRemovePlot += DeathHandler;
    }

    private void OnDisable()
    {
        FarmPlotObject.OnRemovePlot -= DeathHandler;
    }

    // start wave
    public void InitWave()
    {
        _canInfect = true;

        // if amount is above 0
        int cropAmount = _farm.GetActiveCrops().Count;
        if (cropAmount > 0)
        {
            // get max infection of that wave
            _maxInfection = CalculateActiveInfection(cropAmount);

            // iterate though and get random plot
            for (int i = 0; i < _maxInfection; i++)
            {
                FarmPlotObject randomPlot = _farm.GetActiveCrops()[Random.Range(0, _farm.GetActiveCrops().Count)];
                randomPlot.GetPlacedObject().GetRoot().TriggerGrow(ResistantDecider(randomPlot));

                // remove it from normal and add it to infection list
                _currentlyInfectingPlots.Add(randomPlot);
                _farm.RemoveActiveObjet(randomPlot);
            }
        }
    }

    public void StopWave()
    {
        _canInfect = false;

        // get all the infected crops and kill them off
        if (_currentlyInfectingPlots.Count > 0)
        {
            for (int i = 0; i < _currentlyInfectingPlots.Count; i++)
            {
                _currentlyInfectingPlots[i].GetPlacedObject().GetRoot().Kill();
            }
        }

        // clear list
        _currentlyInfectingPlots.Clear();
    }

    private void DeathHandler(FarmPlotObject plot, bool killedByPlayer)
    {
        // put it back in the loop
        if (killedByPlayer)
        {
            _currentlyInfectingPlots.Remove(plot);
            _farm.GetActiveCrops().Add(plot);

            if (_canInfect)
            {
                // try get the next active crop
                int cropAmount = _farm.GetActiveCrops().Count;
                if (cropAmount > 0)
                {
                    // get a random active plot
                    FarmPlotObject randomPlot = _farm.GetActiveCrops()[Random.Range(0, cropAmount)];
                    // infect it
                    randomPlot.GetPlacedObject().GetRoot().TriggerGrow(ResistantDecider(randomPlot));

                    // remove it from normal and add it to infection list
                    _currentlyInfectingPlots.Add(randomPlot);
                    _farm.RemoveActiveObjet(randomPlot);
                }
            }
        }
        else if (!killedByPlayer && _canInfect)
        {
            // remove it from the infection list
            _currentlyInfectingPlots.Remove(plot);

            // try get the next active crop
            int cropAmount = _farm.GetActiveCrops().Count;
            if (cropAmount > 0)
            {
                // get a random active plot
                FarmPlotObject randomPlot = _farm.GetActiveCrops()[Random.Range(0, cropAmount)];
                // infect it
                randomPlot.GetPlacedObject().GetRoot().TriggerGrow(ResistantDecider(randomPlot));

                // remove it from normal and add it to infection list
                _currentlyInfectingPlots.Add(randomPlot);
                _farm.RemoveActiveObjet(randomPlot);
            }
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
