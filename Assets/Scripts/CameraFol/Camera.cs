using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Dictionary<Transform, float> interestPointList = new Dictionary<Transform, float>();
    public Transform mainTarget;
    public float mainInterest = 10f;

    private float firstYPos;
    private float firstZPos = -30;


    public Vector3 velocity;
    public float smoothTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        firstYPos = this.transform.position.y;
        //firstZPos = this.transform.position.z;
        interestPointList.Add(mainTarget, mainInterest);
    }

    // Update is called once per frame
    void Update()
    {
        float sumForInterest = 0;
        Vector3 finalTarget = Vector3.zero;
        foreach (KeyValuePair<Transform, float> interestPoint in interestPointList)
        {
            finalTarget += interestPoint.Key.position * interestPoint.Value;
            sumForInterest += interestPoint.Value;
        }
        finalTarget /= sumForInterest;

        //Surely : add a lerp / delay

        finalTarget.y = firstYPos;
        finalTarget.z += firstZPos;

        this.transform.position = Vector3.SmoothDamp(this.transform.position, finalTarget, ref velocity, smoothTime);
    }
}
