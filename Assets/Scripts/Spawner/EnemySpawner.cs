using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Variables
    [Header("Object to Spawn and Rate")]
    [SerializeField] PoolingSystem[] enemyPools;
    [SerializeField] float spawnRate;
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

        int randomPool = Random.Range(0, enemyPools.Length);
        EnemyScript enemy = enemyPools[randomPool].pool.Get();
        yield return new WaitForSeconds(spawnRate);
        canSpawn = true;
    }

    #endregion
}
