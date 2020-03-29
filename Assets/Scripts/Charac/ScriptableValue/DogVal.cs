using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DogValue", order = 1)]
public class DogVal : ScriptableObject
{
    [Header("Speeds")]
    public float idleSpeed = 1f;
    public float maxRunAwaySpeed = 1f;
    public float maxIdleSpeed = 1f;
    public float maxAvoidSpeed = 1f;
    public float maxFollowSpeed = 3f;
    public float maxBackToLeaderSpeed = 3f;

    [Header("Idle")]
    public Vector2 waitRange = new Vector2(0f, 4f);
    public Vector2 idleRangeX = new Vector2(-2f, 2f);
    public Vector2 idleRangeY = new Vector2(-3f, 3f);
    
    [Header("Friend")]
    public float secondToFriend = 3f;
    public float rangeToGetAwayFrom = 1f;

    [Header("Importance of each")]
    [Range(0, 10)]
    public float centerOfTheAttention = 1f;
    [Range(0, 10)]
    public float introvertness = 1f;
    [Range(0, 10)]
    public float followHero = 1f;
    
    [Header("Bump")]
    public float bumpIntensity = 3f;
    public float bumpDurationReducer = 5f;

    [Header("What ?")]
    public float minDist = 12f;///probably change the strongness of avoidance or attirance



}