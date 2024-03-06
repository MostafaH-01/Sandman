using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public AudioClip dream;
    public AudioClip nightmare;
    public PoolingSystem poolingSystem;
    public ParticleSystem particleSystemGoodGhost;
    public ParticleSystem particleSystemBadGhost;
    public GameObject badGhostRenderer;
    public GameObject goodGhostRenderer;
    public float particleGoodDuration = 5f;
    public float particleBadDuration = 1f;

    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;

    public static event Action<bool> GhostArrived;
    private bool _goodOrBad = false; // true is good, bad is false

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = ManagingGame.Instance.GetEnemySettings()[5]; // Setting enemy speed as it is in game manager
    }
    public void OnPlayerSuccess() // Ghost converted
    {
        _goodOrBad = true;
        badGhostRenderer.SetActive(false);
        goodGhostRenderer.SetActive(true);

        audioSource.PlayOneShot(dream);
        GhostArrived?.Invoke(true);
    }

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
