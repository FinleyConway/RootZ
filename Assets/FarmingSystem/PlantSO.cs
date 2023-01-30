using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "Data/Crops")]
public class PlantSO : GridObjetTypeSO
{
    [field: Header("Growing")]
    [field: SerializeField] public int DaysToGrow { get; private set; } = 0;
    [field: SerializeField] public List<Transform> GrowthStages { get; private set; }
}
