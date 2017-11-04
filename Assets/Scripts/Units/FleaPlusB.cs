using UnityEngine;
using System.Collections;

public class FleaPlusB : Unit {


    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 4;
    
    }

    public static void Promotion(Unit u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.fleaActionPoints = 1;
        u.fleaMovementPoints = 1;
        u.isFleaUpB = true;
    }

}
