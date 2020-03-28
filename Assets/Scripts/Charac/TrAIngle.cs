using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrAIngle : Triangle
{
    [Header("Move")]
    public float currentSpeed = 1f;
    public float idleSpeed = 1f;
    private Vector3 currentTarget = Vector3.zero;
    private bool idleWait = false;
    public float runAwaySpeed = 1f;
    public float groupSpeed = 4f;

    public float minDist = 12f;

    [Range(0,10)]
    public float centerOfTheAttention = 1f;
    [Range(0, 10)]
    public float introvertness = 1f;
    [Range(0, 10)]
    public float rebellness = 1f;

    public enum State
    {
        idle,
        runAway,
        inGroup,
    }
    [Header("State")]
    public State currentState = State.idle;

    public float viewRange;

    public List<Transform> listOfViewSquare = new List<Transform>();
    public List<Triangle> listOfViewedOther = new List<Triangle>();

    public void Start()
    {
        currentSpeed = idleSpeed;
        StartCoroutine(idleTargetting());
    }

    IEnumerator idleTargetting()
    {
        while (true)
        {
            idleWait = false;
            newIdlePos();
            yield return new WaitUntil(()=> (this.transform.position - currentTarget).sqrMagnitude < 0.4f);
            idleWait = true;
            float randomWait = Random.Range(0f, 3f);
            yield return new WaitForSeconds(randomWait);
        }

    }
    void newIdlePos()
    {
        currentTarget = this.transform.position;
        currentTarget.x += Random.Range(-2f, 2f);
        currentTarget.z += Random.Range(-3f, 3f);
    }

    public void Update()
    {
        Vector3 finalMove = Vector3.zero;
        if (listOfViewSquare.Count != 0 /*&& groupSize > square.groupSize*/)
        {
            Vector3 sumOfDistance = Vector3.zero;
            foreach (Transform square in listOfViewSquare)
            {
                sumOfDistance += (this.transform.position - square.position);
            }
            Vector3 wolfMove = sumOfDistance.normalized * runAwaySpeed;
            finalMove += wolfMove;
        }
        else
        {
            Vector3 idleMove = Vector3.zero;
            if (currentSpeed > idleSpeed)
                currentSpeed -= Time.deltaTime * 2f;
            else
                currentSpeed = idleSpeed;

            if (!idleWait)
            {
                idleMove = (currentTarget - this.transform.position).normalized * currentSpeed;
            }
            finalMove += idleMove;
        }

        if (listOfViewedOther.Count != 0)
        {
            finalMove += computeAdvoidingDirection() * introvertness;
        }
        
        //FINAL
        finalMove = addBumpyness(finalMove);
        this.transform.Translate(finalMove * Time.deltaTime);
        lastMove = finalMove;
    }

    Vector3 computeAdvoidingDirection()
    {
        //add dodge from other dog but not as powerfull as the run away
        float numberInGroup = 0;
        Vector3 directionToAvoidPeople = Vector3.zero;
        foreach (Triangle tr in listOfViewedOther)
        {
            Vector3 diff = (this.transform.position - tr.transform.position);
            diff.y = 0;
            float dist = diff.sqrMagnitude;
            /*if (dist < tr.rangeToGetAwayFrom * tr.rangeToGetAwayFrom)*/
            {
                diff /= dist;
                diff *= tr.rangeToGetAwayFrom;
                directionToAvoidPeople += diff * minDist;
                Debug.DrawRay(this.transform.position, diff * minDist, Color.green);
            }
            numberInGroup++;
        }
        directionToAvoidPeople /= numberInGroup;

        return directionToAvoidPeople;
    }

    Vector3 addBumpyness(Vector3 moveVector)
    {
        moveVector.x += bumpVector.x * runAwaySpeed;
        moveVector.z += bumpVector.z * runAwaySpeed;
        timerBumper += Time.deltaTime * 0.5f;
        bumpVector -= (bumpVector * bumpReducer) * Time.deltaTime;
        return moveVector;
    }


    public override TriangleType getType()
    {
        return TriangleType.dog;
    }

    [ContextMenu("DistanceToTarget")]
    float distranceToTarget()
    {
        float dist = (this.transform.position - currentTarget).sqrMagnitude;
        Debug.Log("dist = "+ dist);
        return dist;
    }


}
