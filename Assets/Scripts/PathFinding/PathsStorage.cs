using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsStorage : MonoBehaviour
{
    #region Variables

    public Path[] paths;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        paths = GetComponentsInChildren<Path>();
    }
}
