using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [Header("Move")]
    public float currentSpeed = 1f;
    public float idleSpeed = 1f;
    private Vector3 currentTarget = Vector3.zero;
    private bool idleWait = false;
    public float bullySpeed = 5f;

    [Header("Bully")]
    public List<Transform> listOfPotentialVictim = new List<Transform>();
    public List<Round> listOfPeopleIDontLike = new List<Round>();
    public List<Square> listOfFriends = new List<Square>();
    public float bumpIntensity = 5f;

    public float timerBump = 0f;
    public float howLongStayLaughing = 5f;


    public enum State
    {
        idle,
        bully,
        runAway,
        laugh,
    }
    [Header("State")]
    public State currentState = State.idle;

    //Start
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
            yield return new WaitUntil(() => currentState == State.idle && (this.transform.position - currentTarget).sqrMagnitude < 0.4f);
            idleWait = true;
            float randomWait = Random.Range(0f, 1.2f);
            yield return new WaitForSeconds(randomWait);
        }

    }
    void newIdlePos()
    {
        currentTarget = this.transform.position;
        currentTarget.x += Random.Range(-4f, 4f);
        currentTarget.z += Random.Range(-8f, 8f);
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

                if (currentSpeed > idleSpeed)
                    currentSpeed -= Time.deltaTime * 1f;
                else
                    currentSpeed = idleSpeed;


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

                    if (listOfPeopleIDontLike.Count != 0 && moveAwayVec != Vector3.zero && listOfPeopleIDontLike.Count * 2 + listOfPotentialVictim.Count > 3 * (listOfFriends.Count + 1))
                    {
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
    [Header("Visual")]
    public SpriteRenderer icon;
    public List<Sprite> spriteList = new List<Sprite>(); //0 normal / 1: bully

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
                diff *= 24f;
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
        currentSpeed = bullySpeed;
        currentState = State.bully;
    }

    void goesBackToIdle()
    {
        newIdlePos();
        currentState = State.idle;
    }

    void goesLaugh()
    {
        timerBump = howLongStayLaughing;
        currentState = State.laugh;
    }

    [Header("Bark")]
    public SpriteRenderer barkRenderer;
    public Transform rotateBark;

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
            tria.bumpVector = (tria.transform.position - this.transform.position).normalized * bumpIntensity;
            tria.timerBumper = 0;
            tria.Bump();

            goesLaugh();

            //bark in that direction
            BarkInThatDirection(tria.transform.position);
        }

//        Debug.Log("Collision name"+collision.gameObject.name);
    }
}
