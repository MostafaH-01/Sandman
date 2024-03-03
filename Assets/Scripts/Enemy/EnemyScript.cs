using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public PoolingSystem poolingSystem;
    public ParticleSystem particleSystemGoodGhost;
    public ParticleSystem particleSystemBadGhost;
    public GameObject badGhostRenderer;
    public GameObject goodGhostRenderer;
    public float particleGoodDuration = 5f;
    public float particleBadDuration = 1f;

    public static event Action<bool> GhostArrived;

    private float _particleDuration;
    private bool _goodOrBad = false; // true is good, bad is false

    bool _ghostDisappearing = false;
    private void OnPlayerSuccess() // Ghost converted
    {
        _goodOrBad = true;
        badGhostRenderer.SetActive(false);
        goodGhostRenderer.SetActive(true);
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
        _goodOrBad = false;
        poolingSystem.pool.Release(this);
    }
}
