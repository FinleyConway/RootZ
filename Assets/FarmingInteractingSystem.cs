using System;
using System.Collections.Generic;
using UnityEngine;

public class FarmingInteractingSystem : MonoBehaviour
{
    [SerializeField] private GridObjetTypeSO _placedObject;

    [SerializeField] private float _interactDistance;
    [SerializeField] private LayerMask _groundMask;
    private Camera _cam;

    public static event Action<Vector3, GridObjetTypeSO> OnPlantCrop;
    public static event Action<Vector3> OnTryHarvest;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 hit = LookAtHit();
            PlantCrop(hit);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 hit = LookAtHit();
            ObtainCrop(hit);
        }
    }

    private Vector3 LookAtHit()
    {
        Transform lookPos = _cam.transform;
        if (Physics.Raycast(lookPos.position, lookPos.forward, out RaycastHit hit, _interactDistance, _groundMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void PlantCrop(Vector3 worldPosition)
    {
        OnPlantCrop?.Invoke(worldPosition, _placedObject);
    }

    private void ObtainCrop(Vector3 worldPosition)
    {
        OnTryHarvest?.Invoke(worldPosition);
    }
}
