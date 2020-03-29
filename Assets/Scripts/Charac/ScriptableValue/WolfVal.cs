using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Wolf Value", order = 1)]
public class WolfVal : ScriptableObject
{
    [Header("Idle")]
    public float idleSpeed = 1f;

    public Vector2 waitRange = new Vector2(0.4f, 1.2f);
    public Vector2 idleRangeX = new Vector2(-4f, 4f);
    public Vector2 idleRangeY = new Vector2(-8f, 8f);

    [Header("Bully")]
    public float bullySpeed = 5f;

    [Header("Bump")]
    public float bumpIntensity = 5f;
    public float howLongStayLaughing = 5f;

    [Header("Preference")]
    public float howMuchDontLikeFox = 40f;


}
