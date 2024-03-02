using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public PoolingSystem poolingSystem;
    public ParticleSystem particleSystemPlayerWon;
    public ParticleSystem particleSystemEnemyReached;
    public SkinnedMeshRenderer renderer;
    public float particleDuration;

    private Material _material;
    private float _startTime = 0f;

    bool _ghostDisappearing = false;

    private void Start()
    {
        _material = renderer.material;
    }
    private void OnPlayerSuccess()
    {
        particleSystemPlayerWon.Play();

        _startTime = Time.time;
        _ghostDisappearing = true;
    }
    private void Update()
    {
        if ( _ghostDisappearing )
        {
            Color color = _material.color;
            color.a = Mathf.Lerp(color.a, 0, (Time.time - _startTime) / (particleDuration+2f));
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
        particleSystemEnemyReached.Play();
    }

    public void ParticleSystemStopped()
    {
        _ghostDisappearing = false;
        _startTime = 0f;

        Color color = _material.color;
        color.a = 1;
        _material.color = color;
        poolingSystem.pool.Release(this);
    }
}
