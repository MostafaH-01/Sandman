using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera Camera;
    [Header("Tick for LookAt, Untick to Look Away")]
    public bool atOrAway = true;
    void Update()
    {
        if (Camera == null)
        {
            if (atOrAway)
                this.transform.LookAt(Camera.main.transform.position);
            else
                this.transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
        }
        else
        {
            if (atOrAway)
                this.transform.LookAt(Camera.transform.position);
            else
                this.transform.LookAt(transform.position - (Camera.transform.position - transform.position));
        }

    }
}
