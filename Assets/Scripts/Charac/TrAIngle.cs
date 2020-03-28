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
            yield return new WaitUntil(()=> currentState == State.idle && (this.transform.position - currentTarget).sqrMagnitude < 0.4f);
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
        switch (currentState)
        {
            case State.idle:
                if (currentSpeed > idleSpeed)
                    currentSpeed -= Time.deltaTime * 2f;
                else
                    currentSpeed = idleSpeed;

                if (!idleWait)
                {
                    Vector3 idleMove = (currentTarget - this.transform.position).normalized * currentSpeed;


                    idleMove = addBumpyness(idleMove);

                    this.transform.Translate(idleMove * Time.deltaTime);
                    lastMove = idleMove;
                }

                if (listOfViewSquare.Count != 0)
                {
                    goesToRunAway();
                }
                else if (listOfViewedOther.Count != 0)
                {
                    goesInGroup();
                }
                break;
            case State.runAway:
                if (listOfViewSquare.Count != 0 /*&& groupSize > square.groupSize*/)
                {
                    Vector3 sumOfDistance = Vector3.zero;
                    foreach (Transform square in listOfViewSquare)
                    {
                        sumOfDistance += (this.transform.position - square.position);
                    }
                    Vector3 finalMove = sumOfDistance.normalized * runAwaySpeed;
                    
                    finalMove = addBumpyness(finalMove);

                    this.transform.Translate(finalMove * Time.deltaTime);
                    lastMove = finalMove;
                }
                else
                {
                    goesBackToIdle();
                }
                break;
            case State.inGroup:
                if (listOfViewSquare.Count != 0)
                {
                    goesToRunAway();
                }
                else if (listOfViewedOther.Count != 0) 
                {
                    //stay in distance to everyone : same run away than the square. (average distance)
                    Vector3 distSum = Vector3.zero;
                    //move also depending of "leader" movement
                    Vector3 moveOfLeader = Vector3.zero;
                    //move towards the average position.
                    Vector3 posSum = Vector3.zero;

                    float numberInGroup = 0;
                    foreach (Triangle tr in listOfViewedOther)
                    {
                        Vector3 diff = (this.transform.position - tr.transform.position);
                        diff /= diff.sqrMagnitude;
                        diff *= tr.rangeToGetAwayFrom;
                        distSum += diff * minDist;

                        moveOfLeader += tr.lastMove;

                        posSum += tr.transform.position;
                        numberInGroup++;
                    }
                    moveOfLeader /= numberInGroup;
                    distSum /= numberInGroup;
                    posSum /= numberInGroup;
                    posSum -= this.transform.position;

                    posSum *= centerOfTheAttention;
                    distSum *= introvertness;
                    moveOfLeader *= rebellness;

                    Vector3 finalMove = (distSum + posSum /*+ moveOfLeader*/).normalized * groupSpeed;
                    //finalMove = addBumpyness(finalMove);
                    this.transform.Translate(finalMove * Time.deltaTime);
                    lastMove = finalMove;
                }
                else
                {
                    goesBackToIdle();
                }
                break;
            default:
                break;
        }
    }

    void goesBackToIdle()
    {
        newIdlePos();
        currentState = State.idle;
    }

    void goesToRunAway()
    {
        currentSpeed = runAwaySpeed;
        currentState = State.runAway;
    }

    void goesInGroup()
    {
        currentState = State.inGroup;
    }


    Vector3 addBumpyness(Vector3 moveVector)
    {
        moveVector.x += bumpVector.x * runAwaySpeed;
        moveVector.z += bumpVector.z * runAwaySpeed;
        timerBumper += Time.deltaTime * 0.5f;
        bumpVector -= (bumpVector * bumpReducer) * Time.deltaTime;
        return moveVector;
    }


    [ContextMenu("DistanceToTarget")]
    float distranceToTarget()
    {
        float dist = (this.transform.position - currentTarget).sqrMagnitude;
        Debug.Log("dist = "+ dist);
        return dist;
    }


}
