using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEnd : MonoBehaviour
{
    [SerializeField] private int endPossibility = 35;
    [SerializeField] private int maxPossibility = 50;
    [SerializeField] private int minPossibility = 20;

    public int EndPossibility
    {
        get { return endPossibility; }
        set { endPossibility = value; }
    }

    public void DecreasePossibility()
    {
        endPossibility -= 5;

        if( endPossibility < minPossibility)
        {
            endPossibility = maxPossibility;
        }
    }
}
