using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrAIngle : Triangle
{
    public DogVal gmplValue;

    [Header("Debug")]
    public bool debug = false;

    [Header("Move")]
    public float currentSpeed = 1f;
   
    private Vector3 currentTarget = Vector3.zero;
    private bool idleWait = false;
    
    public Vector3 getScare = Vector3.zero;

    public float multiplicatorFriendship = 0.33f;
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


    [Header("Friendship Aimation")]
    public SpriteRenderer coinAnim01;
    public SpriteRenderer coinAnim02;
    public List<Sprite> allSpriteToShow;
    public AnimationCurve nearEffect = new AnimationCurve();
    [Header("Bump")]
    public SpriteRenderer faceSprite;
    public List<Sprite> listSpriteFace; //0 = normal / 1=bumped /2 = fear
   
    public void Start()
    {
        rangeToGetAwayFrom = gmplValue.rangeToGetAwayFrom;
        bumpDurationReducer = gmplValue.bumpDurationReducer;

           currentSpeed = gmplValue.idleSpeed;
        StartCoroutine(idleTargetting());

        multiplicatorFriendship = 10f / gmplValue.secondToFriend;
    }

    IEnumerator idleTargetting()
    {
        while (true)
        {
            idleWait = false;
            newIdlePos();
            yield return new WaitUntil(()=> (this.transform.position - currentTarget).sqrMagnitude < 0.4f);
            idleWait = true;
            float randomWait = Random.Range(gmplValue.waitRange.x, gmplValue.waitRange.y);
            yield return new WaitForSeconds(randomWait);
        }

    }
    void newIdlePos()
    {
        currentTarget = this.transform.position;
        currentTarget.x += Random.Range(gmplValue.idleRangeX.x, gmplValue.idleRangeX.y);
        currentTarget.z += Random.Range(gmplValue.idleRangeY.x, gmplValue.idleRangeY.y);
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
            wolfMove = Vector3.ClampMagnitude(wolfMove, gmplValue.maxRunAwaySpeed);
            finalMove += wolfMove;
        }
        //IDLE MOVE
        else
        {
            Vector3 idleMove = Vector3.zero;
            if (currentSpeed > gmplValue.idleSpeed)
                currentSpeed -= Time.deltaTime * 2f;
            else
                currentSpeed = gmplValue.idleSpeed;

            if (!idleWait)
            {
                idleMove = (currentTarget - this.transform.position).normalized * currentSpeed;
            }
            idleMove = Vector3.ClampMagnitude(idleMove, gmplValue.maxIdleSpeed);
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
                    moveFromLeader *= reduce;
                    moveFromLeader = Vector3.ClampMagnitude(moveFromLeader, gmplValue.maxBackToLeaderSpeed);
                }
            }
            posSum /= listOfFriends.Count;
            posSum -= this.transform.position;
            posSum *= gmplValue.centerOfTheAttention;

            if(numberOfCloseFriend >= 2)
            {
                //don't need to get back to his friends
            }

            posSum = Vector3.ClampMagnitude(posSum, gmplValue.maxFollowSpeed);
            finalMove += posSum;
            finalMove += moveFromLeader * gmplValue.followHero;
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
                    diff *= tr.rangeToGetAwayFrom * gmplValue.minDist * nearEffect.Evaluate(dist);
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
                    UpdateFriendship(Time.deltaTime * multiplicatorFriendship);

                    if (friendship >= 5 && !listOfFriends.Contains(tr))
                    { 
                        tr.GetComponent<Move>().friendNumbers++;
                        GameManager.Instance.sndManager.UpdateMusic((tr.GetComponent<Move>().friendNumbers / 10f));
                        GameManager.Instance.sndManager.PlayDogFriendly();
                        listOfFriends.Add(tr);
                    }
                }
                else
                {
                    if (friendship + tr.friendship > 14f && !listOfFriends.Contains(tr))
                    {
                        listOfFriends.Add(tr);
                    }
                }
            }
            directionToAvoidPeople /= howManyToAvoid;
            //Debug.DrawRay(this.transform.position, directionToAvoidPeople, Color.green + Color.blue);
            sumMove /= howManyToMove;
            //Debug.DrawRay(this.transform.position, sumMove, Color.red);

            directionToAvoidPeople = Vector3.ClampMagnitude(directionToAvoidPeople, gmplValue.maxAvoidSpeed);
            finalMove += directionToAvoidPeople * gmplValue.introvertness;

            //add move, is enough.
            if (howManyToMove != 0)
            {
                sumMove = Vector3.ClampMagnitude(sumMove, gmplValue.maxFollowSpeed);
                finalMove += sumMove * Mathf.Clamp01(friendship/10f);
            }

            if (debug)
                Debug.Log("THEN : " + directionToAvoidPeople + " final Move = " + finalMove);
        }
        else{ if (currentState == State.inGroup){ newIdlePos();currentState = State.idle;}}

        //If get scare
        finalMove += getScare;

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

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        float value01 = (friendship / 10f);
        float index = value01 * allSpriteToShow.Count;
        //
        int indexSprite = (int)index;
        if(indexSprite >= allSpriteToShow.Count - 1)
        {
            coinAnim01.sprite = allSpriteToShow[allSpriteToShow.Count - 1];
            coinAnim01.color = Color.white;
            return;
        }

        coinAnim01.sprite = allSpriteToShow[indexSprite];
        coinAnim02.sprite = allSpriteToShow[Mathf.Min(indexSprite + 1, allSpriteToShow.Count-2)];

        float lerp = index - indexSprite;
        coinAnim02.color = Color.Lerp(Color.white - Color.black, Color.white, lerp);
    }

    Vector3 addBumpyness(Vector3 moveVector)
    {
        if (bumpVector == Vector3.zero)
        {
            return moveVector;
        }

        moveVector.x += bumpVector.x * gmplValue.bumpIntensity;
        moveVector.z += bumpVector.z * gmplValue.bumpIntensity;
        timerBumper += Time.deltaTime * 0.5f;
        bumpVector -= bumpVector * Time.deltaTime * bumpDurationReducer;

        if (bumpVector == Vector3.zero)
        {
            EndBump();
        }
        return moveVector;
    }

    public void GetScared(Vector3 direction)
    {
        float friendshipValue = Mathf.Clamp01((6 - friendship) / 6f);
        getScare = direction.normalized * gmplValue.maxRunAwaySpeed * friendshipValue;
    }


    public override void Bump()
    {
        faceSprite.sprite = listSpriteFace[1];
        GameManager.Instance.sndManager.PlayDogFlee();

        if (friendship > 8)
        {
            float value = (friendship - 8f) / 2f;
            GameManager.Instance.cameraScreen.Screenshake(GameManager.Instance.feedbackValue.screenshakeForDog * value);
        }
    }

    public void EndBump()
    {
        faceSprite.sprite = listSpriteFace[0];
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
