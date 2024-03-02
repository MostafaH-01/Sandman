using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    #region Variables

    [SerializeField] Node[] pathNodes;

    public Node[] PathNodes
    {
        get { return pathNodes; }
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if (!pathNodes[pathNodes.Length - 1].gameObject.CompareTag("PathEnd")) 
        {
            Debug.LogError("Path " + transform.gameObject.name +  " leads nowhere!");
        }
    }
}
