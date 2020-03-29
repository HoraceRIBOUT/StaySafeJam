using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PopulWall : MonoBehaviour
{
    public bool replaceWall = false;
    public float stepSize = 1f;

    // Update is called once per frame
    void Update()
    {
        if (replaceWall)
            ReplaceWall();
    }

    void ReplaceWall()
    {
        replaceWall = false;
        int index = 1;
        foreach(Transform trChild in GetComponentsInChildren<Transform>())
        {
            if(trChild.name == this.name)
            {
                continue;
            }
            trChild.localPosition = index * Vector3.left * stepSize;
            index++;
        }
    }
}
