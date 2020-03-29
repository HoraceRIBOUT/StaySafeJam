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
        newIdlePos();
        StartCoroutine(idleTargetting());
    }

    IEnumerator idleTargetting()
    {
        while (true)
        {
            idleWait = false;
            yield return new WaitUntil(() => (this.transform.position - currentTarget).sqrMagnitude < 0.4f);
            idleWait = true;
            float randomWait = Random.Range(0f, 1.2f);
            if (triangleToHelp != null)
            {
                randomWait = 2f;
            }
            yield return new WaitForSeconds(randomWait);
            newIdlePos();
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
        triangleToHelp = triangle;
        newIdlePos(triangleToHelp.transform.position);

        StopAllCoroutines();
        StartCoroutine(surprisedPause());
    }

    public SpriteRenderer spurpispsprite;

    public IEnumerator surprisedPause()
    {
        idleWait = true;
        //Surprise
        spurpispsprite.color = Color.white;
        yield return new WaitForSeconds(0.33f);
        idleWait = false;
        spurpispsprite.color = Color.clear;
        //Surprise
        StartCoroutine(idleTargetting());
    }

    void newIdlePos(Vector3 position)
    {
        currentTarget = position;
        currentTarget.x += Random.Range(-0.5f, 0.5f);
        currentTarget.z += Random.Range(-0.5f, 0.5f);
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
        return TriangleType.fox;
    }

    public override void Bump()
    {
        //NO BUMP.

    }

}
