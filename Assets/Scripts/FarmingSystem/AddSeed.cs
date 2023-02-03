using UnityEngine;

public class AddSeed : MonoBehaviour, IInteractable
{
    [SerializeField] private PlantSO _seed;
    [SerializeField] private int _getAmount;
    [SerializeField] private FarmingInteractingSystem _playerPlanting;

    public void Interact(GameObject host)
    {
        int money = MoneyManager.Instance.GetMoney();
        if (money > _seed.BuyAmount)
        {
            _playerPlanting.AddSeed(_seed, _getAmount);
            money -= _seed.BuyAmount;
            if (money > 0)
            {
                money = 0;
            }
            MoneyManager.Instance.SetMoney(money);
        }
    }
}