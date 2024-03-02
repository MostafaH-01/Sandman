using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathMovement : MonoBehaviour
{
    #region Variables
    private Path path;
    private NavMeshAgent agent;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
