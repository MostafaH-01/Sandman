using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingSystem : MonoBehaviour
{
    #region Variables
    public ObjectPool<EnemyScript> pool;
    [SerializeField] private EnemyScript pooledObject;
    [SerializeField] private Transform[] spawnPoints;
    #endregion

    #region Awake
    void Awake()
    {
        pool = new ObjectPool<EnemyScript>(CreateObject, ActivateObject, DeactivateObject, DestroyObject, true, 10, 15);
    }
    #endregion

    #region Pool Methods
    private EnemyScript CreateObject()
    {
        EnemyScript spawnedObject = Instantiate(pooledObject, RandomPosition(), Quaternion.identity);
        spawnedObject._poolingSystem = this;
        return spawnedObject;
    }

    private void ActivateObject(EnemyScript pooledObject)
    {
        pooledObject.transform.position = RandomPosition();
        pooledObject.gameObject.SetActive(true);
    }

    private void DeactivateObject(EnemyScript pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void DestroyObject(EnemyScript pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
    #endregion

    #region Randomization Method
    private Vector3 RandomPosition()
    {
        int randomNumber = UnityEngine.Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomNumber].position;
    }
    #endregion
}
