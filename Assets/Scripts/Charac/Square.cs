using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public WolfVal gmplValue;

    [Header("Move")]
    public float currentSpeed = 1f;
    private Vector3 currentTarget = Vector3.zero;
    private bool idleWait = false;

    [Header("Bully")]
    public List<Transform> listOfPotentialVictim = new List<Transform>();
    public List<Round> listOfPeopleIDontLike = new List<Round>();
    public List<Square> listOfFriends = new List<Square>();
    public float timerBump = 0f;

    public int resistance = 7;

    public enum State
    {
        idle,
        bully,
        runAway,
        laugh,
    }
    [Header("State")]
    public State currentState = State.idle;

    [Header("Visual")]
    public SpriteRenderer icon;
    public List<Sprite> spriteList = new List<Sprite>(); //0 normal / 1: bully

    [Header("Bark")]
    public SpriteRenderer barkRenderer;
    public Transform rotateBark;

    //Start
    public void Start()
    {
        currentSpeed = gmplValue.idleSpeed;
        StartCoroutine(idleTargetting());
    }

    IEnumerator idleTargetting()
    {
        while (true)
        {
            idleWait = false;
            newIdlePos();
            yield return new WaitUntil(() => currentState == State.idle && (this.transform.position - currentTarget).sqrMagnitude < 0.4f);
            idleWait = true;
            float randomWait = Random.Range(gmplValue.waitRange.x, gmplValue.waitRange.x);
            yield return new WaitForSeconds(randomWait);
        }

    }
    void newIdlePos()
    {
        currentTarget = this.transform.position;
        currentTarget.x += Random.Range(gmplValue.idleRangeX.x, gmplValue.idleRangeX.y);
        currentTarget.z += Random.Range(gmplValue.idleRangeY.x, gmplValue.idleRangeY.y);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.idle:
                if (listOfPotentialVictim.Count != 0)
                {
                    goesBully();
                }

                if (currentSpeed > gmplValue.idleSpeed)
                    currentSpeed -= Time.deltaTime * 1f;
                else
                    currentSpeed = gmplValue.idleSpeed;


                if (!idleWait)
                {
                    Vector3 idleMove = (currentTarget - this.transform.position).normalized * currentSpeed;

                    idleMove += addMoveAwayFromPeopleIDontLike();

                    this.transform.Translate(idleMove * Time.deltaTime);
                }
                break;
            case State.bully:
                if (listOfPotentialVictim.Count == 0)
                {
                    goesBackToIdle();
                }
                else
                {
                    //itere on all triangle and choose which one to bully until it's empty
                    Vector3 direction = (listOfPotentialVictim[0].position - this.transform.position).normalized;
                    Vector3 finalMove = direction * currentSpeed;

                    Vector3 moveAwayVec = addMoveAwayFromPeopleIDontLike();
                    finalMove += moveAwayVec;

                    if (listOfPeopleIDontLike.Count != 0 && moveAwayVec != Vector3.zero && listOfPeopleIDontLike.Count * 2 + listOfPotentialVictim.Count > (resistance-1) * (listOfFriends.Count + 1))
                    {
                        finalMove -= finalMove;
                        finalMove = moveAwayVec;
                    }

                    this.transform.Translate(finalMove * Time.deltaTime);

                }

                //when bump ! They stop a little to laugh 
                break;
            case State.runAway:
                break;
            case State.laugh:
                timerBump -= Time.deltaTime;
                if (timerBump <= 0)
                    goesBackToIdle();
                break;
            default:
                break;
        }




        //Visual, bad
        switch (currentState)
        {
            case State.idle:
                icon.sprite = spriteList[0];
                break;
            case State.bully:
                icon.sprite = spriteList[1];
                break;
            case State.runAway:

                break;
            default:
                break;
        }
    }

    public Vector3 addMoveAwayFromPeopleIDontLike()
    {
        float howManyToAvoid = 0;
        Vector3 directionToAvoidPeople = Vector3.zero;
        foreach (Round fox in listOfPeopleIDontLike)
        {
            if (fox.triangleToHelp == null)
                continue;
            Vector3 diff = (this.transform.position - fox.transform.position);
            diff.y = 0;
            float dist = diff.sqrMagnitude;
            /*if (dist < tr.rangeToGetAwayFrom * tr.rangeToGetAwayFrom)*/
            {
                diff /= dist;//Approx dist = 0 --> 60
                diff *= gmplValue.howMuchDontLikeFox;
                directionToAvoidPeople += diff;
                //Debug.DrawRay(this.transform.position, diff * minDist, Color.green);
            }
            howManyToAvoid++;
        }
        directionToAvoidPeople /= (howManyToAvoid==0?1:howManyToAvoid);

        return directionToAvoidPeople;
    }


    void goesBully()
    {
        currentSpeed = gmplValue.bullySpeed;
        currentState = State.bully;
    }

    void goesBackToIdle()
    {
        newIdlePos();
        currentState = State.idle;
    }

    void goesLaugh()
    {
        timerBump = gmplValue.howLongStayLaughing;
        currentState = State.laugh;
    }

    public void BarkInThatDirection(Vector3 triaPos)
    {
        float dot = Vector3.Dot(Vector3.forward, (triaPos - this.transform.position).normalized);
        float sign = Mathf.Sign(Vector3.Cross(Vector3.forward, (triaPos - this.transform.position)).y);
        rotateBark.localEulerAngles = Vector3.forward * (dot - 1) * 90 * sign;

        barkRenderer.color = Color.white;
        StartCoroutine(BarkFadeAway());
    }

    public IEnumerator BarkFadeAway()
    {
        float lerp = 0;
        while(lerp < 1)
        {
            barkRenderer.color = Color.Lerp(Color.white, Color.white - Color.black, lerp * lerp);
            lerp += Time.deltaTime * 2f;
            yield return new WaitForSeconds(0.01f);
        }
        barkRenderer.color = Color.white - Color.black;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Triangle tria = collision.gameObject.GetComponent<Triangle>();
        if (tria != null) 
        {
            if (tria.getType() == Triangle.TriangleType.fox)
            {
                //wait bro, no bump
                return;
            }
            //Shuld be a method in triangle
            tria.bumpVector = (tria.transform.position - this.transform.position).normalized * gmplValue.bumpIntensity;
            tria.timerBumper = 0;
            tria.Bump();

            goesLaugh();
            GameManager.Instance.sndManager.PlayWolfBark(tria.getType() == Triangle.TriangleType.hero);

            //bark in that direction
            BarkInThatDirection(tria.transform.position);
        }

//        Debug.Log("Collision name"+collision.gameObject.name);
    }
}
