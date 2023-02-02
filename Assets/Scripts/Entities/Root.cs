using System;
using UnityEngine;

public class Root : Entity
{
    private float _currentGrowTime;
    private float _growTime;
    private float _currentGrowValue;
    private bool _canGrow;

    private const string _growName = "Grow_";

    private MeshRenderer _mesh;
    private Material _shaderMat;

    public event Action<bool> OnDestroy;

    protected override void Awake()
    {
        base.Awake();

        _mesh = GetComponent<MeshRenderer>();

        if (_mesh.material.HasProperty(_growName))
        {
            _mesh.material.SetFloat(_growName, 0);
            _shaderMat = _mesh.material;
        }
    }

    private void Update()
    {
        if (_canGrow) Grow();
    }

    public void TriggerGrow(float infectionRate)
    {
        _currentGrowTime = infectionRate;
        _currentGrowValue = _shaderMat.GetFloat(_growName);
        _canGrow = true;
    }

    private void StopGrow()
    {
        _canGrow = false;
        _currentGrowTime = 0;
        _currentGrowValue = 0;
        _growTime = 0;
        _shaderMat.SetFloat(_growName, 0);
    }

    private void Grow()
    {
        if (_growTime < _currentGrowTime)
        {
            _currentGrowValue = Mathf.Lerp(0, 1, _growTime / _currentGrowTime);
            _shaderMat.SetFloat(_growName, _currentGrowValue);

            _growTime += Time.deltaTime;
        }
        else
        {
            OnDestroy?.Invoke(false);
        }
    }

    public void Kill() => Death();

    protected override void Death()
    {
        StopGrow();
        OnDestroy?.Invoke(true);
    }
}
