using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMove : MonoBehaviour
{
    public Move father;

    public void Start()
    {
        if (father == null)
            father = GetComponentInParent<Move>();
    }


    private void OnTriggerEnter(Collider other)
    {
        Round round = other.GetComponent<Round>();
        if (round != null)
        {
            if (!father.foxesAtReach.Contains(round))
                father.foxesAtReach.Add(round);
        }

        TrAIngle doggo = other.GetComponent<TrAIngle>();
        if (doggo != null)
        {
            if (!father.doggoAtReach.Contains(doggo))
                father.doggoAtReach.Add(doggo);
        }
    }

    //Removal have to be on another one
    private void OnTriggerExit(Collider other)
    {
        Round round = other.GetComponent<Round>();
        if (round != null)
        {
            if (father.foxesAtReach.Contains(round))
                father.foxesAtReach.Remove(round);
        }

        TrAIngle doggo = other.GetComponent<TrAIngle>();
        if (doggo != null)
        {
            if (father.doggoAtReach.Contains(doggo))
                father.doggoAtReach.Remove(doggo);
        }
    }
}
