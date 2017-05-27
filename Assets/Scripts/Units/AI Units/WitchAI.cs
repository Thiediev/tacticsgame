using UnityEngine;
using System.Collections;

public class WitchAI : AIPlayerFix
{


    public static int cost = 3000;

    void Start()
    {
        attackRange = 2;
        movementPerActionPoint = 1;
        HP = 10;
    }
}
