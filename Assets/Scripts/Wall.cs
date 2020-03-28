using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Triangle
{
    public void Update()
    {
        lastMove = Vector3.zero;
    }

    public override TriangleType getType()
    {
        return TriangleType.wall;
    }
}
