using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{

    public Vector3 lastMove = Vector3.zero;
    public float rangeToGetAwayFrom = 1f;
    //can be bump by Square

    public Vector3 bumpVector = Vector3.zero;
    public float bumpReducer = 0.33f;

    public float timerBumper = 0;
}
