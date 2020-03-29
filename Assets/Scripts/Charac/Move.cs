using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Triangle
{
    public HeroVal gmplVal;

    [Header("Hero Move")]
    public List<Round> foxesAtReach = new List<Round>();
    public List<TrAIngle> doggoAtReach = new List<TrAIngle>();

    public int friendNumbers = 0;

    // Start is called before the first frame update
    void Start()
    {
        rangeToGetAwayFrom = gmplVal.rangeToGetAwayFrom;
        bumpDurationReducer = gmplVal.bumpDurationReducer;

        foxesAtReach.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AskForHelp();
        }

        Movement();
    }

    public Animator askForHelpAnim;

    public void AskForHelp()
    {
        askForHelpAnim.SetTrigger("Ask");

        foreach (Round fox in foxesAtReach)
        {
            fox.CallForHelp(this);
        }

        foreach (TrAIngle doggo in doggoAtReach)
        {
            doggo.GetScared(doggo.transform.position - this.transform.position);
        }
    }


    public void Movement()
    {
        //if !allow
        //return;

        Vector2 movement = Vector2.zero;
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        Vector3 finalMove = Vector3.zero;

        if (bumpVector == Vector3.zero)
        {
            float joystickIntensity = Mathf.Min(1, movement.sqrMagnitude);
            Vector2 direction = movement.normalized;

            finalMove.x = direction.x * joystickIntensity * gmplVal.speed;
            finalMove.z = direction.y * joystickIntensity * gmplVal.speed;

        }
        else
        {
            float joystickIntensity = Mathf.Min(1, movement.sqrMagnitude);
            Vector2 direction = movement.normalized;



            finalMove.x = bumpVector.x * gmplVal.bumpSpeed + direction.x * joystickIntensity * Mathf.Min(timerBumper, gmplVal.speed);
            finalMove.z = bumpVector.z * gmplVal.bumpSpeed + direction.y * joystickIntensity * Mathf.Min(timerBumper, gmplVal.speed);
            timerBumper += Time.deltaTime * gmplVal.resistanceToBump;
            bumpVector -= bumpVector * Time.deltaTime * bumpDurationReducer;
        }


        this.transform.Translate(finalMove * Time.deltaTime);
        lastMove = finalMove;
    }

    public override void Bump()
    {
        //screenshake
        GameManager.Instance.cameraScreen.Screenshake(GameManager.Instance.feedbackValue.screenshakeForHeroBump);
    }

    public override TriangleType getType()
    {
        return TriangleType.hero;
    }

}
