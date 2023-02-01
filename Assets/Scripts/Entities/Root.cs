using System;
using System.Collections;
using UnityEngine;

public class Root : Entity
{
    [SerializeField] private float _growTime = 5;
    private const float _growRate = 0.05f;
    private Coroutine _grow;
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

    public void TriggerGrow()
    {
        _grow = StartCoroutine(Grow());
    }

    public void StopGrow()
    {
        StopCoroutine(_grow);
        _shaderMat.SetFloat(_growName, 0);
    }

    private IEnumerator Grow()
    {
        float growValue = _shaderMat.GetFloat(_growName);

        while (growValue < 1)
        {
            growValue += 1 / (_growTime / _growRate);
            _shaderMat.SetFloat(_growName, growValue);

            yield return new WaitForSeconds(_growRate);
        }
        OnDestroy?.Invoke(false);
    }

    protected override void Death()
    {
        StopGrow();
        OnDestroy?.Invoke(true);
    }
}
