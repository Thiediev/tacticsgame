using UnityEngine;
using System.Collections;

public class FleaPlusA : Unit {


    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 4;
        //fleaMovementPoints = 1;
        //fleaActionPoints = 1;
    }

    public static void Promotion(Unit u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.waiting = true;
        u.isFlyer = true;
    }

}
