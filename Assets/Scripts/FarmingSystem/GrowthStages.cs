using System.Collections.Generic;
using UnityEngine;

public class GrowthStages : MonoBehaviour
{
    [SerializeField] private List<Transform> _growthStages;

    private void Awake()
    {
        for (int i = 1; i < _growthStages.Count; i++)
        {
            _growthStages[i].gameObject.SetActive(false);
        }
    }

    public List<Transform> GetGrowths() => _growthStages;
}
