using UnityEngine;
using System.Collections;

public class WitchPlusA : Unit {

    void Start()
    {
        attackRange = 3;
        movementPerActionPoint = 1;
    }

    public static void Promotion(Unit u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.waiting = true;
    }

}
