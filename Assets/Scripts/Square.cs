using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public float bumpIntensity = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Triangle tria = collision.gameObject.GetComponent<Triangle>();
        if (tria != null) 
        {
            //Shuld be a method in triangle
            tria.bumpVector = (tria.transform.position - this.transform.position).normalized * bumpIntensity;
            tria.timerBumper = 0;
        }

        Debug.Log("Collision name"+collision.gameObject.name);
    }
}
