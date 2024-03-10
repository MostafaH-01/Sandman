using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<Node> storedNodes;

    public List<Node> StoredNodes
    {
        get { return storedNodes; }
    }
    #endregion

    #region Visaulize Paths
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        if(storedNodes.Count > 0)
        {
            foreach (var node in storedNodes)
            {
                if (node == null)
                {
                    return;
                }

                Gizmos.DrawLine(transform.position, node.transform.position);
            }
        }
    }
    #endregion
}
