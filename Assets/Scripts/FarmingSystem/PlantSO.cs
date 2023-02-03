using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "Data/Crops")]
public class PlantSO : GridObjetTypeSO
{
    [field: Header("Growing")]
    [field: SerializeField] public int BuyAmount { get; private set; }
    [field: SerializeField] public int SellAmount { get; private set; } = 0;
    [field: SerializeField] public RootResistant RootResistantType { get; private set; }
    [field: SerializeField] public Transform ZombiePrefab { get; private set; }

    public enum RootResistant
    {
        Low,
        High,
    }
}
