using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Triangle
{
    [Header("Hero Move")]
    public float basicSpeed = 5f;

    public float moveIncapacity = 0.5f;

    public List<Round> foxesAtReach = new List<Round>();
    public List<TrAIngle> doggoAtReach = new List<TrAIngle>();

    public int friendNumbers = 0;

    // Start is called before the first frame update
    void Start()
    {
        foxesAtReach.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

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

            finalMove.x = direction.x * joystickIntensity * basicSpeed;
            finalMove.z = direction.y * joystickIntensity * basicSpeed;

        }
        else
        {
            float joystickIntensity = Mathf.Min(1, movement.sqrMagnitude);
            Vector2 direction = movement.normalized;



            finalMove.x = bumpVector.x * basicSpeed + direction.x * joystickIntensity * Mathf.Min(timerBumper, basicSpeed);
            finalMove.z = bumpVector.z * basicSpeed + direction.y * joystickIntensity * Mathf.Min(timerBumper, basicSpeed);
            timerBumper += Time.deltaTime * moveIncapacity;
            bumpVector -= (bumpVector * bumpReducer) * Time.deltaTime;
        }


        this.transform.Translate(finalMove * Time.deltaTime);
        lastMove = finalMove;
    }

    public override void Bump()
    {
        //screenshake
        GameManager.Instance.cameraScreen.Screenshake(0.7f);
    }

    public override TriangleType getType()
    {
        return TriangleType.hero;
    }

}
