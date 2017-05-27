using UnityEngine;
using System.Collections;

public class SpiderPlusB : Unit {

    void Start()
    {
        attackRange = 2;
        movementPerActionPoint = 2;
    }

    public static void Promotion(Unit u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.waiting = true;
    }

}
