using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPath : MonoBehaviour
{
    #region Variables
    private SphereCollider pathCollider;
    private float colliderRadius = 2;
    private List<GameObject> pathsAvailable;
    private Queue travelled;
    private GameObject destination;
    private NavMeshAgent agent;
    private int maxQueueStorage = 2;
    #endregion

    #region Awake and Start
    private void Awake()
    {
        pathsAvailable = new List<GameObject>();
        travelled = new Queue();
        agent = GetComponent<NavMeshAgent>();
        pathCollider = gameObject.AddComponent<SphereCollider>();
        pathCollider.radius = colliderRadius;
        pathCollider.isTrigger = true;
    }
    #endregion

    #region Actions on Trigger Enter and Exit
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Path")) 
        {
            return;
        } 
        else
        {
            pathsAvailable.Add(other.gameObject);
            NextDestination();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Path"))
        {
            return;
        }
        else
        {
            GameObject pathToRemove = other.gameObject;
            for (int i = 0; i < pathsAvailable.Count; i++)
            {
                if (pathsAvailable[i] == pathToRemove)
                {
                    pathsAvailable.RemoveAt(i);
                }
            }
        }
    }
    #endregion

    #region Movement Methods
    private void NextDestination()
    {
        GameObject pathChosen = pathsAvailable[UnityEngine.Random.Range(0, pathsAvailable.Count)];

        if (travelled.Contains(pathChosen))
        {
            NextDestination();
        }
        else
        {
            Debug.Log("Path chosen is " + pathChosen.name);
            agent.destination = pathChosen.GetComponent<BoxCollider>().bounds.center;
            AddToTravelled(pathChosen);
            return;
        }
    }

    private void AddToTravelled(GameObject path)
    {
        if(travelled.Count >= maxQueueStorage)
        {
            travelled.Dequeue();
        }

        travelled.Enqueue(path);
    }

    #endregion

    #region OnDisable
    private void OnDisable()
    {
        pathsAvailable.Clear();
    }
    #endregion
}
