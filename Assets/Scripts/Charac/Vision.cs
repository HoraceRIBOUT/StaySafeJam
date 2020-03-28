using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public TrAIngle father;
    public bool add = true;

    public void Start()
    {
        if (father == null)
            father = GetComponentInParent<TrAIngle>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!add)
            return;


        Triangle triangle = other.GetComponent<Triangle>();
        if (triangle != null)
        {
            if (!father.listOfViewedOther.Contains(triangle))
                father.listOfViewedOther.Add(triangle);
        }

        Square square = other.GetComponent<Square>();
        if (square != null)
        {
            if(!father.listOfViewSquare.Contains(square.transform))
                father.listOfViewSquare.Add(square.transform);
        }
    }

    //Removal have to be on another one
    private void OnTriggerExit(Collider other)
    {
        if (add)
            return;

        Triangle triangle = other.GetComponent<Triangle>();
        if (triangle != null)
        {
            if (father.listOfViewedOther.Contains(triangle))
                father.listOfViewedOther.Remove(triangle);
        }

        Square square = other.GetComponent<Square>();
        if (square != null)
        {
            if (father.listOfViewSquare.Contains(square.transform))
                father.listOfViewSquare.Remove(square.transform);
        }
    }
}
