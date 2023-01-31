using UnityEngine;

[CreateAssetMenu(fileName = "newShopItem", menuName = "Data/ShopItem")]
public class ShopItemSO : ScriptableObject
{
    [field: Header("Selling")]
    [field: SerializeField] public string Name { get; private set; } = "Insert Name";
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int SellAmount { get; private set; } = 0;
    [field: SerializeField] public int CostAmount { get; private set; } = 0;
}
