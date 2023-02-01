using System;
using UnityEngine;

public class SellCropsSystem : MonoBehaviour, IInteractable
{
    [Header("Reference To Farm")]
    [SerializeField] private FarmManager _farm;

    private bool _canSell = false;

    public static event Action<int> OnSellCrops;

    private void OnEnable()
    {
        GameManager.OnGameStart += GameManager_OnGameStart;
        GameManager.OnDownTime += GameManager_OnDownTime;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= GameManager_OnGameStart;
        GameManager.OnDownTime -= GameManager_OnDownTime;
    }

    public void Interact(GameObject host)
    {
        if (_canSell)
        {
            SellCrops();
        }
    }

    private void SellCrops()
    {
        int amount = 0;
        foreach (FarmPlotObject plot in _farm.GetActiveCrops())
        {
            amount += plot.GetPlacedObject().GetObjectData().SellAmount;
        }
        OnSellCrops?.Invoke(amount);
    }

    public void SetBuyingState(bool state) => _canSell = state;

    private void GameManager_OnGameStart()
    {
        SetBuyingState(false);
    }

    private void GameManager_OnDownTime()
    {
        SetBuyingState(true);
    }
}