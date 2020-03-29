using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : Triangle
{
    public FoxVal gmplValue;

    [Header("Move")]
    private Vector3 currentTarget = Vector3.zero;
    private bool idleWait = false;
    public Triangle triangleToHelp;
     
    //Start
    public void Start()
    {
        rangeToGetAwayFrom = gmplValue.rangeToGetAwayFrom;

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
            float randomWait = Random.Range(gmplValue.waitRange.x, gmplValue.waitRange.x);
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
        currentTarget.x += Random.Range(gmplValue.idleRangeX.x, gmplValue.idleRangeX.y);
        currentTarget.z += Random.Range(gmplValue.idleRangeY.x, gmplValue.idleRangeY.y);
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
        yield return new WaitForSeconds(gmplValue.surprisePause);
        idleWait = false;
        spurpispsprite.color = Color.clear;
        //Surprise
        StartCoroutine(idleTargetting());
    }

    void newIdlePos(Vector3 position)
    {
        currentTarget = position;
        currentTarget.x += Random.Range(-1f, 1f);
        currentTarget.z += Random.Range(-1f, 1f);
    }

    private void Update()
    {
        if (!idleWait)
        {
            Vector3 idleMove = (currentTarget - this.transform.position).normalized * gmplValue.basicSpeed;
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
