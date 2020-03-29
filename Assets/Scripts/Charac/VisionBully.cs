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
            if (triangle.GetComponent<Wall>())
            {
                return;
            }
            Round round = triangle.GetComponent<Round>();
            if (round != null)
            {
                father.listOfPeopleIDontLike.Add(round);
                return;
            }
            if (triangle.GetComponent<TrAIngle>())
            {
                if (triangle.GetComponent<TrAIngle>().listOfFriends.Count >= 3 * (father.listOfFriends.Count + 1))
                {
                    return;
                }
            }
            if (!father.listOfPotentialVictim.Contains(triangle.transform))
                father.listOfPotentialVictim.Add(triangle.transform);
        }

        Square square = other.GetComponent<Square>();
        if (square != null)
        {
            //if get enough far away in game : 
            //listOfFriends.Add(square);
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
            if (triangle.GetComponent<Wall>())
            {
                return;
            }

            if (father.listOfPotentialVictim.Contains(triangle.transform))
                father.listOfPotentialVictim.Remove(triangle.transform);
        }
    }
}
