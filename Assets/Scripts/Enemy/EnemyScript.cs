using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public PoolingSystem _poolingSystem;
    public ParticleSystem _particleSystem;
    public SkinnedMeshRenderer _renderer;
    public float _particleDuration;
    private Material _material;

    bool _ghostDisappearing = false;

    private void Start()
    {
        _material = _renderer.material;
    }
    private void OnPlayerSuccess()
    {
        _particleSystem.Play();
        _ghostDisappearing = true;
    }
    private void Update()
    {
        if ( _ghostDisappearing )
        {
            Color color = _material.color;
            color.a = Mathf.Lerp(color.a, 0, _particleDuration * Time.deltaTime);
            _material.color = color;
        }
    }
    private void OnEnable()
    {
        PlayerActions.PlayerSucceeded += OnPlayerSuccess;
    }
    private void OnDisable()
    {
        PlayerActions.PlayerSucceeded -= OnPlayerSuccess;
    }

    public void ParticleSystemStopped()
    {
        _ghostDisappearing = false;

        Color color = _material.color;
        color.a = 255;
        _material.color = color;
        _poolingSystem.pool.Release(this);
    }
}
