using UnityEngine;
using System.Collections;

public class FleaAI : AIPlayerFix {

    public static int cost = 2000;

    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 3;

        HP = 10;
    }
}
