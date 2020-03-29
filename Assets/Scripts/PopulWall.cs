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
        for(int indexA = 0; indexA < transform.childCount; indexA++)
        {
            if(transform.GetChild(indexA).name == this.name)
            {
                continue;
            }
            transform.GetChild(indexA).localPosition = index * Vector3.left * stepSize;
            transform.GetChild(indexA).localEulerAngles = -this.transform.localEulerAngles;
            index++;
        }
    }
}
