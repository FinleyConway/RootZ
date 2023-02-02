using System;
using UnityEngine;

public class SellCropsSystem : MonoBehaviour, IInteractable
{
    [Header("Sound")]
    [SerializeField] private SimpleAudioEvent _openSound;
    [SerializeField] private AudioSource _source;

    [Header("Reference To Farm")]
    [SerializeField] private FarmManager _farm;
    private Animator _anim;

    private bool _canSell = false;

    public static event Action<int> OnSellCrops;

    private void OnEnable()
    {
        _anim = GetComponentInChildren<Animator>();

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
            if (_openSound != null) _openSound.Play(_source);
        }
    }

    private void SellCrops()
    {
        int amount = 0;
        foreach (FarmPlotObject plot in _farm.GetActiveCrops())
        {
            amount += plot.GetPlacedObject().GetObjectData().SellAmount;
            plot.GetPlacedObject().DestroySelf();
        }
        _farm.GetActiveCrops().Clear();
        OnSellCrops?.Invoke(amount);
        _anim.SetTrigger("Sell");
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