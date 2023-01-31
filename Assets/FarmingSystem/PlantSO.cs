using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "Data/Crops")]
public class PlantSO : GridObjetTypeSO
{
    [field: Header("Growing")]
    [field: SerializeField] public float GrowthTime { get; private set; } = 0;
    [field: SerializeField] public Transform GrownAsset { get; private set; }
}
