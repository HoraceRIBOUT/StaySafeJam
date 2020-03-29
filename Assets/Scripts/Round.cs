using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : Triangle
{
    [Header("Move")]
    public float basicSpeed = 1f;
    private Vector3 currentTarget = Vector3.zero;
    private bool idleWait = false;
    public Triangle triangleToHelp;
     
    //Start
    public void Start()
    {
        StartCoroutine(idleTargetting());
    }

    IEnumerator idleTargetting()
    {
        while (true)
        {
            idleWait = false;
            newIdlePos();
            yield return new WaitUntil(() => (this.transform.position - currentTarget).sqrMagnitude < 0.4f);
            idleWait = true;
            float randomWait = Random.Range(0f, 1.2f);
            if (triangleToHelp != null)
            {
                randomWait = 2f;
            }
            yield return new WaitForSeconds(randomWait);
        }

    }
    void newIdlePos()
    {
        currentTarget = this.transform.position;
        currentTarget.x += Random.Range(-4f, 4f);
        currentTarget.z += Random.Range(-8f, 8f);
    }

    public void CallForHelp(Triangle triangle)
    {
        newIdlePos(triangleToHelp.transform.position);
        StartCoroutine(surprisedPause());
    }

    public IEnumerator surprisedPause()
    {
        idleWait = true;
        yield return new WaitForSeconds(0.33f);
        idleWait = false;
    }

    void newIdlePos(Vector3 position)
    {
        currentTarget = this.transform.position;
        currentTarget.x += Random.Range(-4f, 4f);
        currentTarget.z += Random.Range(-8f, 8f);
    }

    private void Update()
    {
        if (!idleWait)
        {
            Vector3 idleMove = (currentTarget - this.transform.position).normalized * basicSpeed;
            this.transform.Translate(idleMove * Time.deltaTime);
        }
    }



    public override TriangleType getType()
    {
        throw new System.NotImplementedException();
    }

    public override void Bump()
    {
        //NO BUMP.

    }

}
