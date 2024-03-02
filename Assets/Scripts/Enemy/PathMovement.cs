using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathMovement : MonoBehaviour
{
    #region Variables
    private Path path;
    private NavMeshAgent agent;
    private int wayPointIndex;

    public Path Path
    {
        get
        {
            return path;
        }
        set
        {
            path = value;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        wayPointIndex = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(path.PathNodes[wayPointIndex].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        float distanceToNode = Vector3.Distance(path.PathNodes[wayPointIndex].transform.position, transform.position);

        if(distanceToNode <= 1)
        {
            if (wayPointIndex == path.PathNodes.Length - 1) 
            {
                // release enemy
            }
            else
            {
                wayPointIndex++;
                Debug.Log(wayPointIndex);
                agent.SetDestination(path.PathNodes[wayPointIndex].transform.position);
            }
        }
    }
}
