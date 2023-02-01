using System;
using System.Collections.Generic;
using UnityEngine;

public class FarmingInteractingSystem : MonoBehaviour
{
    [SerializeField] private List<PlantSO> _seeds;
    private PlantSO _currentSeed;
    private int _currentIndex = 0;

    [SerializeField] private float _interactDistance;
    [SerializeField] private LayerMask _groundMask;
    private Camera _cam;

    public static event Action<Vector3, PlantSO> OnPlantCrop;

    private void Awake()
    {
        _cam = Camera.main;
        if (_seeds.Count > 0) _currentSeed = _seeds[0];
    }

    private void Update()
    {
        SeedSelector();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 hit = LookAtHit();
            PlantCrop(hit);
        }
    }

    public void AddSeed(PlantSO plant)
    {
        _seeds.Add(plant);
    }

    // increments through crops and selected the current index
    private void SeedSelector()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta == 0) return;

        if (delta > 0) _currentIndex++;
        if (delta < 0) _currentIndex--;

        if (_currentIndex < 0) _currentIndex = _seeds.Count - 1;
        else if (_currentIndex > _seeds.Count - 1) _currentIndex = 0;

        _currentSeed = _seeds[_currentIndex];
    }

    // get the position of where the raycast hits
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
        OnPlantCrop?.Invoke(worldPosition, _currentSeed);
    }

    private void OnDrawGizmos()
    {
        Transform lookPos = Camera.main.transform;
        Gizmos.DrawRay(lookPos.position, lookPos.forward * _interactDistance);
    }
}