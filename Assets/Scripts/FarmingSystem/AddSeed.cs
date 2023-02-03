using UnityEngine;

public class AddSeed : MonoBehaviour, IInteractable
{
    [SerializeField] private PlantSO _seed;
    [SerializeField] private int _getAmount;
    [SerializeField] private FarmingInteractingSystem _playerPlanting;

    public void Interact(GameObject host)
    {
        if (MoneyManager.Instance.GetMoney() > _seed.BuyAmount)
        {
            _playerPlanting.AddSeed(_seed, _getAmount);
        }
    }
}