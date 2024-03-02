using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Variables
    [Header("Object to Spawn and Rate")]
    [SerializeField] PoolingSystem enemyPool;
    [SerializeField] float spawnRate;

    [Header("Paths Creation for Enemies")]
    [SerializeField] Path[] paths;
    private bool canSpawn = true;

    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            StartCoroutine(Spawn());
        }
    }
    #endregion

    #region Coroutine

    private IEnumerator Spawn()
    {
        canSpawn = false;
        enemyPool.pool.Get();
        yield return new WaitForSeconds(spawnRate);
        canSpawn = true;
    }

    #endregion
}
