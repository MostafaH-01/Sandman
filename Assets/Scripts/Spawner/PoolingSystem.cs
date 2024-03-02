using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingSystem : MonoBehaviour
{
    #region Variables
    public ObjectPool<GameObject> pool;
    [SerializeField] private GameObject pooledObject;
    [SerializeField] private Transform[] spawnPoints;
    #endregion

    #region Awake
    void Awake()
    {
        pool = new ObjectPool<GameObject>(CreateObject, ActivateObject, DeactivateObject, DestroyObject, true, 10, 15);
    }
    #endregion

    #region Pool Methods
    private GameObject CreateObject()
    {
        GameObject spawnedObject = Instantiate(pooledObject, RandomPosition(), Quaternion.identity);
        return spawnedObject;
    }

    private void ActivateObject(GameObject pooledObject)
    {
        pooledObject.transform.position = RandomPosition();
        pooledObject.gameObject.SetActive(true);
    }

    private void DeactivateObject(GameObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void DestroyObject(GameObject pooledObject)
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
