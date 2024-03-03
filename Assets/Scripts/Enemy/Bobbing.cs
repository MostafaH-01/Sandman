using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    Vector2 floatY;
    float originalY;

    [SerializeField] private float floatStrength;

    //void Start()
    //{
    //    this.originalY = this.transform.position.y;
    //}

    void Update()
    {
        floatY = transform.position;
        floatY.y = (Mathf.Sin(Time.time) * floatStrength);
        transform.position = floatY;
    }
}
