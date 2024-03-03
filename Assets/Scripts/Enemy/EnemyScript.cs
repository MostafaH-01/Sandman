using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip dream;
    public AudioClip nightmare;
    public PoolingSystem poolingSystem;
    public ParticleSystem particleSystemGoodGhost;
    public ParticleSystem particleSystemBadGhost;
    public GameObject badGhostRenderer;
    public GameObject goodGhostRenderer;
    public float particleGoodDuration = 5f;
    public float particleBadDuration = 1f;

    public static event Action<bool> GhostArrived;
    private bool _goodOrBad = false; // true is good, bad is false

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnPlayerSuccess() // Ghost converted
    {
        _goodOrBad = true;
        badGhostRenderer.SetActive(false);
        goodGhostRenderer.SetActive(true);

        audioSource.PlayOneShot(dream);
        GhostArrived?.Invoke(true);
    }
    //private void OnEnable()
    //{
    //    PlayerActions.PlayerSucceeded += OnPlayerSuccess;
    //}
    //private void OnDisable()
    //{
    //    PlayerActions.PlayerSucceeded -= OnPlayerSuccess;
    //}

    public void ReachedHouse()
    {
        if (_goodOrBad)
        {
            particleSystemGoodGhost.Play();
        }
        else
        {
            audioSource.PlayOneShot(nightmare);

            particleSystemBadGhost.Play();
            GhostArrived?.Invoke(false);
        }
    }

    public void ParticleSystemStopped()
    {
        _goodOrBad = false;
        goodGhostRenderer.SetActive(false);
        badGhostRenderer.SetActive(true);
        poolingSystem.pool.Release(this);
    }
}
