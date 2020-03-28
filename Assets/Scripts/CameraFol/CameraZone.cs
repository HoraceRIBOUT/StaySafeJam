using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public Transform targetToFollow;
    public CameraManagement cameraToFill;
    //probably a list of thing bidule chouette

    private void OnTriggerEnter(Collider other)
    {
        //interest
        if (other.GetComponent<interestPoint>())
        {
            cameraToFill.interestPointList.Add(other.transform, other.GetComponent<interestPoint>().interest);
        }
        //cameraToFill.
    }

    private void OnTriggerExit(Collider other)
    {
        //interest
        if (other.GetComponent<interestPoint>())
        {
            cameraToFill.interestPointList.Remove(other.transform);
        }
        //cameraToFill.
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = targetToFollow.transform.position;
    }
}
