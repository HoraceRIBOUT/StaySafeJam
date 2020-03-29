using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Feedback Value", order = 1)]
public class FeedbackVal : ScriptableObject
{
    public float screenshakeForHeroBump = 0.7f;
    public float screenshakeForDog = 0.2f;


    public float screenShakeSpeed = 10f;
    public float screenShakeIntensity = 1f;
    public float screenShakeDecreaseTime = 0.5f;
}
