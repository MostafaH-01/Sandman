using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public PoolingSystem poolingSystem;
    public ParticleSystem particleSystemGoodGhost;
    public ParticleSystem particleSystemBadGhost;
    public SkinnedMeshRenderer badGhostRenderer;
    public GameObject goodGhostRenderer;
    public float particleGoodDuration = 5f;
    public float particleBadDuration = 1f;

    public static event Action<bool> GhostArrived;

    private float _particleDuration;
    private bool _goodOrBad = false; // true is good, bad is false
    private Material _material;
    private float _startTime = 0f;

    bool _ghostDisappearing = false;

    private void Start()
    {
        _material = badGhostRenderer.material;
    }
    private void OnPlayerSuccess() // Ghost converted
    {
        _goodOrBad = true;
        badGhostRenderer.gameObject.SetActive(false);
        goodGhostRenderer.SetActive(true);
    }
    private void Update()
    {
        if ( _ghostDisappearing )
        {
            Color color = _material.color;
            color.a = Mathf.Lerp(color.a, 0, (Time.time - _startTime) / _particleDuration);
            Debug.Log("Alpha: " + color.a);
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

    public void ReachedHouse()
    {
        _startTime = Time.time;

        if (_goodOrBad)
        {
            _particleDuration = particleGoodDuration;

            particleSystemGoodGhost.Play();
            GhostArrived?.Invoke(true);
        }
        else
        {
            _particleDuration = particleBadDuration;

            particleSystemBadGhost.Play();
            GhostArrived?.Invoke(false);
        }
        _ghostDisappearing = true;
    }

    public void ParticleSystemStopped()
    {
        _ghostDisappearing = false;
        _goodOrBad = false;
        _startTime = 0f;

        Color color = _material.color;
        color.a = 1;
        _material.color = color;
        poolingSystem.pool.Release(this);
    }
}
