using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Triangle
{
    public void Start()
    {
        friendship = -20;
    }
    public void Update()
    {
        lastMove = Vector3.zero;
    }

    public override TriangleType getType()
    {
        return TriangleType.wall;
    }

    public override void Bump()
    {
        foreach (Animator anim in animList)
        {
            anim.SetTrigger("Ask");
        }
        return;
    }

    public List<Animator> animList;
}
