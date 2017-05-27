using UnityEngine;
using System.Collections;

public class SpiderPlusA : Unit {


    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 2;
    }

    public static void Promotion(Unit u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.waiting = true;
    }
}
