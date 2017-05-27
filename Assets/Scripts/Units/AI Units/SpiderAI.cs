using UnityEngine;
using System.Collections;

public class SpiderAI : AIPlayerFix {


    public static int cost = 4000;

    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 2;
        HP = 10;
    }
}
