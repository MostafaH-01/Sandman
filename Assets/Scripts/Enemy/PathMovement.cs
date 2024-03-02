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
    private bool reachedHouse;

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

    private void OnEnable()
    {
        Debug.Log(wayPointIndex);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        wayPointIndex = 0;
        reachedHouse = false;
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
                if(!reachedHouse)
                {
                    GetComponent<EnemyScript>().ReachedHouse();
                    reachedHouse = true;
                }
            }
            else
            {
                wayPointIndex++;
                agent.SetDestination(path.PathNodes[wayPointIndex].transform.position);
            }
        }
    }

    public void SetAgentDestination()
    {
        agent.SetDestination(path.PathNodes[wayPointIndex].transform.position);
    }

    private void OnDisable()
    {
        wayPointIndex = 0;
        reachedHouse= false;
    }
}
