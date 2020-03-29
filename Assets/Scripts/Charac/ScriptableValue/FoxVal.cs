using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Fox Value", order = 1)]
public class FoxVal : ScriptableObject
{
    public float rangeToGetAwayFrom = 1f;

    public float basicSpeed = 1f;
    public Vector2 waitRange = new Vector2(0.4f, 1.2f);
    public Vector2 idleRangeX = new Vector2(-4f, 4f);
    public Vector2 idleRangeY = new Vector2(-8f, 8f);

    public float surprisePause = 0.33f;
}
