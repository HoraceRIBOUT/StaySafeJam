﻿using System.Collections;
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
    public float bumpIntensity = 5f;


    public enum State
    {
        idle,
        bully,
        runAway,
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
                    this.transform.Translate(finalMove * Time.deltaTime);

                }

                //when bump ! They stop a little to laugh 
                break;
            case State.runAway:
                break;
            default:
                break;
        }
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


    private void OnCollisionEnter(Collision collision)
    {
        Triangle tria = collision.gameObject.GetComponent<Triangle>();
        if (tria != null) 
        {
            //Shuld be a method in triangle
            tria.bumpVector = (tria.transform.position - this.transform.position).normalized * bumpIntensity;
            tria.timerBumper = 0;
        }

        Debug.Log("Collision name"+collision.gameObject.name);
    }
}