using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathMovement : MonoBehaviour
{
    #region Variables
    private Node destinationNode;
    private Node travelledNode;

    public Node CurrentNode
    {
        get { return destinationNode;}
        set { destinationNode = value; }
    }

    private NavMeshAgent agent;
    #endregion

    #region Awake Start and Update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.destination = destinationNode.transform.position;
    }

    void Update()
    {
        Walk();
    }
    #endregion

    #region Moving and Assigning Nodes
    private void Walk()
    {
        if (destinationNode != null)
        {
            float distanceToNode = Vector3.Distance(destinationNode.transform.position, transform.position);

            if (distanceToNode <= 1)
            {
                if (destinationNode.gameObject.CompareTag("PathEnd"))
                {
                    PathEnd endScript = CurrentNode.gameObject.GetComponent<PathEnd>();
                    int possiblity = Random.Range(1, 101);

                    if (possiblity <= endScript.EndPossibility)
                    {
                        endScript.DecreasePossibility();
                        CurrentNode = null;
                        gameObject.GetComponent<EnemyScript>().ReachedHouse();
                    }
                    else
                    {
                        AssignNewNode();
                    }
                }
                else
                {
                    AssignNewNode();
                }

            }
        }
    }

    private void AssignNewNode()
    {
        List<Node> storedNodes = destinationNode.StoredNodes;
        int randomNumber = Random.Range(0, storedNodes.Count);

        if (storedNodes[randomNumber] == travelledNode)
        {
            AssignNewNode();
        }
        else
        {
            travelledNode = destinationNode;
            destinationNode = storedNodes[randomNumber];
            agent.destination = destinationNode.transform.position;
        }
    }
    #endregion
}
