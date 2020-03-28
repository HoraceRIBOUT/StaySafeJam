using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBully : MonoBehaviour
{
    public Square father;
    public bool add = true;

    public void Start()
    {
        if (father == null)
            father = GetComponentInParent<Square>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!add)
            return;


        Triangle triangle = other.GetComponent<Triangle>();
        if (triangle != null)
        {
            if (!father.listOfPotentialVictim.Contains(triangle.transform))
                father.listOfPotentialVictim.Add(triangle.transform);
        }

        Square square = other.GetComponent<Square>();
        if (square != null)
        {
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
            if (father.listOfPotentialVictim.Contains(triangle.transform))
                father.listOfPotentialVictim.Remove(triangle.transform);
        }
    }
}
