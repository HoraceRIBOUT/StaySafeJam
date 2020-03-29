using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrAIngle : Triangle
{
    [Header("Debug")]
    public bool debug = false;

    [Header("Move")]
    public float currentSpeed = 1f;
    public float idleSpeed = 1f;
    private Vector3 currentTarget = Vector3.zero;
    private bool idleWait = false;
    public float runAwaySpeed = 1f;
    public float groupSpeed = 4f;

    public float maxRunAwaySpeed = 1f;
    public float maxIdleSpeed = 1f;
    public float maxAvoidSpeed = 1f;
    public float maxFollowSpeed = 3f;
    public float maxBackToLeaderSpeed = 3f;


    public float minDist = 12f;

    [Range(0,10)]
    public float centerOfTheAttention = 1f;
    [Range(0, 10)]
    public float introvertness = 1f;
    public SpriteRenderer friendshipSprite;

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
    public List<Triangle> listOfFriends = new List<Triangle>();

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
        //WOLF DODGE
        if (listOfViewSquare.Count != 0 /*&& groupSize > square.groupSize*/)
        {
            Vector3 sumOfDistance = Vector3.zero;
            foreach (Transform square in listOfViewSquare)
            {
                sumOfDistance += (this.transform.position - square.position);
            }
            Vector3 wolfMove = sumOfDistance.normalized * currentSpeed;
            wolfMove = Vector3.ClampMagnitude(wolfMove, maxRunAwaySpeed);
            finalMove += wolfMove;
        }
        //IDLE MOVE
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
            Debug.DrawRay(this.transform.position, idleMove, Color.red + Color.blue);

            idleMove = Vector3.ClampMagnitude(idleMove, maxIdleSpeed);
            finalMove += idleMove;
            if (debug)
                Debug.Log("SO : " + idleMove + " final Move = " + finalMove);
        }

        //STAY IN GROUP
        if (listOfFriends.Count !=0)
        {
            //move towards the average position.
            Vector3 posSum = Vector3.zero;
            Vector3 moveFromLeader = Vector3.zero;

            float numberOfCloseFriend = 0;
            //I WANT TO STAY WITH FRIENDS
            foreach (Triangle tr in listOfFriends)
            {
                posSum += tr.transform.position;

                //if (distanceToFriend < 1f )
                {
                    numberOfCloseFriend++;
                    //if (numberOfCloseFriend >= 2)
                    //    break;
                }


                if (tr.getType() == TriangleType.hero)
                {
                    moveFromLeader = tr.lastMove; //* distToLeader is clearly not the same as (dist)

                    float reduce = Vector3.Dot(moveFromLeader.normalized, (tr.transform.position - this.transform.position).normalized);

                    Debug.DrawRay(this.transform.position, moveFromLeader, Color.blue);
                    Debug.DrawRay(this.transform.position, (tr.transform.position - this.transform.position), Color.white * 0.25f);
                    Debug.DrawRay(this.transform.position, moveFromLeader * reduce, Color.cyan);

                    moveFromLeader *= reduce;

                    moveFromLeader = Vector3.ClampMagnitude(moveFromLeader, maxBackToLeaderSpeed);
                }


            }
            posSum /= listOfFriends.Count;
            posSum -= this.transform.position;
            posSum *= centerOfTheAttention;

            if(numberOfCloseFriend >= 2)
            {
                //don't need to get back to his friends
            }

            posSum = Vector3.ClampMagnitude(posSum, maxFollowSpeed);
            finalMove += posSum;
            finalMove += moveFromLeader;
        }

        //DODGE PEOPLE
        if (listOfViewedOther.Count != 0)
        {
            if (currentState == State.idle){newIdlePos();currentState = State.inGroup; }

            //
            //add dodge from other dog but not as powerfull as the run away
            float howManyToAvoid = 0;
            float howManyToMove = 0;
            Vector3 directionToAvoidPeople = Vector3.zero;
            Vector3 sumMove = Vector3.zero;
            foreach (Triangle tr in listOfViewedOther)
            {
                Vector3 diff = (this.transform.position - tr.transform.position);
                diff.y = 0;
                float dist = diff.sqrMagnitude;
                /*if (dist < tr.rangeToGetAwayFrom * tr.rangeToGetAwayFrom)*/
                {
                    diff /= dist;//Approx dist = 0 --> 60
                    diff *= tr.rangeToGetAwayFrom * minDist * nearEffect.Evaluate(dist);
                    directionToAvoidPeople += diff;
                    //Debug.DrawRay(this.transform.position, diff * minDist, Color.green);
                }
                howManyToAvoid++;

                if (tr.lastMove != Vector3.zero)
                {
                    sumMove += tr.lastMove;
                    howManyToMove++;
                }

                //only if it's the player
                if(tr.getType() == TriangleType.hero)
                {
                    UpdateFriendship(Time.deltaTime * 0.5f);

                    if (friendship >= 5 && listOfFriends.Count==0)
                    {
                        listOfFriends.Add(tr);
                    }
                }
                else
                {
                    if (friendship + tr.friendship > 12f && !listOfFriends.Contains(tr))
                    {
                        listOfFriends.Add(tr);
                    }
                }
            }
            directionToAvoidPeople /= howManyToAvoid;
            //Debug.DrawRay(this.transform.position, directionToAvoidPeople, Color.green + Color.blue);
            sumMove /= howManyToMove;
            //Debug.DrawRay(this.transform.position, sumMove, Color.red);

            directionToAvoidPeople = Vector3.ClampMagnitude(directionToAvoidPeople, maxAvoidSpeed);
            finalMove += directionToAvoidPeople * introvertness;

            //add move, is enough.
            if (howManyToMove != 0)
            {
                sumMove = Vector3.ClampMagnitude(sumMove, maxFollowSpeed);
                finalMove += sumMove * (friendship/10f);
            }

            if (debug)
                Debug.Log("THEN : " + directionToAvoidPeople + " final Move = " + finalMove);
        }
        else{ if (currentState == State.inGroup){ newIdlePos();currentState = State.idle;}}
        
        //FINAL
        finalMove = addBumpyness(finalMove);
        this.transform.Translate(finalMove * Time.deltaTime);
        lastMove = finalMove;
        if (debug)
            Debug.Log("FOR : " + finalMove);
    }

    public void UpdateFriendship(float value)
    {
        if(value > 0 && friendship < 10)
        {
            friendship += value;
        }
        else if(value > 0)
            friendship = 10;

        friendshipSprite.color = Color.Lerp(Color.white - Color.black, Color.white, (friendship / 10f));
    }


    Vector3 computeAdvoidingDirection()
    {
        //add dodge from other dog but not as powerfull as the run away
        float howManyToAvoid = 0;
        float howManyToMove = 0;
        Vector3 directionToAvoidPeople = Vector3.zero;
        Vector3 sumMove = Vector3.zero;
        foreach (Triangle tr in listOfViewedOther)
        {
            Vector3 diff = (this.transform.position - tr.transform.position);
            diff.y = 0;
            float dist = diff.sqrMagnitude;
            /*if (dist < tr.rangeToGetAwayFrom * tr.rangeToGetAwayFrom)*/
            {
                diff /= dist;//Approx dist = 0 --> 60
                diff *= tr.rangeToGetAwayFrom * minDist * nearEffect.Evaluate(dist);
                directionToAvoidPeople += diff;
                Debug.DrawRay(this.transform.position, diff * minDist, Color.green);
            }
            howManyToAvoid++;

            if (tr.lastMove != Vector3.zero)
            {
                sumMove += tr.lastMove;
                howManyToMove++;
            }
        }
        directionToAvoidPeople /= howManyToAvoid;
        Debug.DrawRay(this.transform.position, directionToAvoidPeople, Color.green + Color.blue);
        sumMove /= howManyToMove;

        Debug.DrawRay(this.transform.position, sumMove , Color.red);

        return directionToAvoidPeople;
    }

    public AnimationCurve nearEffect = new AnimationCurve();

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
