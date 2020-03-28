using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    public Dictionary<Transform, float> interestPointList = new Dictionary<Transform, float>();
    public Transform mainTarget;
    public float mainInterest = 10f;

    private float firstYPos;
    private float firstZPos = -8.5f;
    private Vector3 realPosition = Vector3.zero;


    public Vector3 velocity;
    public float smoothTime = 0.1f;
    public float rotationIntensity = 1f;
#if UNITY_EDITOR
    public List<Transform> transformFromPointList = new List<Transform>();
#endif

    [Header("Trauma")]
    [Range(0,1)]
    public float trauma;
    public float screenSpeed = 10f;
    public float screenShakeIntensity = 1f;
    public float screenShakeDecreaseTime = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        realPosition = this.transform.position;
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

        realPosition = Vector3.SmoothDamp(realPosition, finalTarget, ref velocity, smoothTime);
        if (velocity.x != 0)
        {
            this.transform.eulerAngles = new Vector3(45, velocity.x * rotationIntensity, 0);
        }

        Vector3 screenVec = Vector3.zero;
        //Screenshake part
        if (trauma>0)
        {
            screenVec.x = (Mathf.PerlinNoise(1, Time.timeSinceLevelLoad * screenSpeed) - 0.5f ) * trauma * trauma * screenShakeIntensity;
            screenVec.z = (Mathf.PerlinNoise(Time.timeSinceLevelLoad * screenSpeed, 1) - 0.5f ) * trauma * trauma * screenShakeIntensity;
            trauma -= Time.deltaTime * screenShakeDecreaseTime;
        }

        this.transform.position = realPosition + screenVec;

#if UNITY_EDITOR
        ForDebug();
#endif
    }

    [ContextMenu("Screenshake")]
    public void Screenshake()
    {
        Screenshake(1);
    }
    public void Screenshake(float intensity)
    {
        trauma = intensity;
    }

#if UNITY_EDITOR
    void ForDebug()
    {
        transformFromPointList.Clear();
        foreach (KeyValuePair<Transform, float> interestPoint in interestPointList)
        {
            transformFromPointList.Add( interestPoint.Key);
        }
    }
#endif
}
