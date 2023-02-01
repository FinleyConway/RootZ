using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int _initBillAmount;
    [SerializeField] private int _billIncreaseAmount;
    private int _currentBillAmount;
    private int _currentAmount;

    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GetComponent<GameManager>();
        _currentBillAmount = _initBillAmount;

        SellCropsSystem.OnSellCrops += OnCropsSold;
    }

    private void OnDisable()
    {
        SellCropsSystem.OnSellCrops -= OnCropsSold;
    }

    public void IncreaseBillAmount()
    {
        _currentBillAmount += _billIncreaseAmount;
    }

    public void TryPayBills()
    {
        _currentAmount -= _currentBillAmount;

        if (_currentAmount <= 0)
        {
            // game lost
            _gameManager.UpdateGameState(GameState.Lost);
        }
    }

    public int GetMoney()
    {
        return _currentAmount;
    }

    private void OnCropsSold(int soldAmount)
    {
        _currentAmount += soldAmount;
    }
}