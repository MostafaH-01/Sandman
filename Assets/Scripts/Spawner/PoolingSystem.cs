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
    [SerializeField] private GameObject[] spawnPoints;
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
        GameObject spawnPoint = RandomSpawnPoint();
        EnemyScript spawnedObject = Instantiate(pooledObject, spawnPoint.transform.position, Quaternion.identity);
        spawnedObject.poolingSystem = this;
        return spawnedObject;
    }

    private void ActivateObject(EnemyScript pooledObject)
    {
        GameObject spawnPoint = RandomSpawnPoint();
        pooledObject.transform.position = spawnPoint.transform.position;
        pooledObject.gameObject.GetComponent<PathMovement>().DestinationNode = AssignNode(spawnPoint);
        pooledObject.gameObject.SetActive(true);
        pooledObject.gameObject.GetComponent<PathMovement>().AssignNewNode();
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

    #region Randomization and Node Assignment
    private GameObject RandomSpawnPoint()
    {
        int randomNumber = UnityEngine.Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomNumber];
    }

    private Node AssignNode(GameObject spawnPoint)
    {
        Node node = spawnPoint.GetComponent<Node>();
        return node.StoredNodes[0];
    }
    #endregion
}
