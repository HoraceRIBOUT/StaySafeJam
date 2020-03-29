using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Hero value", order = 1)]
public class HeroVal : ScriptableObject
{
    public float speed = 5f;
    public float bumpSpeed = 5f;
    public float bumpDurationReducer = 5f;
    public float rangeToGetAwayFrom = 1f;

    public float resistanceToBump = 0.5f;
}
