using System;
using System.Collections.Generic;
using UnityEngine;

public class Seeds
{
    public PlantSO Seed;
    public int Amount;

    public Seeds(PlantSO seed, int amount)
    {
        Seed = seed;
        Amount = amount;
    }
}

public class FarmingInteractingSystem : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private SimpleAudioEvent _plantSound;
    [SerializeField] private AudioSource _source;

    [Header("Seeds")]
    private PlantSO _currentSeed;
    private int _currentIndex = 0;
    private bool _canPlant = true;

    [SerializeField] private float _interactDistance;
    [SerializeField] private LayerMask _groundMask;
    private Camera _cam;

    public static event Action<Vector3, PlantSO> OnPlantCrop;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        GameManager.OnGameStart += GameManager_OnGameStart;
        GameManager.OnDownTime += GameManager_OnDownTime;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= GameManager_OnGameStart;
        GameManager.OnDownTime += GameManager_OnDownTime;
    }

    private void Update()
    {
        if (_currentSeed != null)
            if (Input.GetMouseButtonDown(1) && _canPlant)
            {
                Vector3 hit = LookAtHit();
                    PlantCrop(hit);
                    if (_plantSound != null) _plantSound.Play(_source);
            }
    }

    public void AddSeed(PlantSO plant, int amount)
    {
        Seeds seed = new Seeds(plant, amount);
        _currentSeed = plant;
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

    private void GameManager_OnDownTime()
    {
        _canPlant = true;
    }

    private void GameManager_OnGameStart()
    {
        _canPlant = false;
    }

    private void OnDrawGizmos()
    {
        Transform lookPos = Camera.main.transform;
        Gizmos.DrawRay(lookPos.position, lookPos.forward * _interactDistance);
    }
}