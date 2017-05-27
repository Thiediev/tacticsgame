using UnityEngine;
using System.Collections;

public class NWFleaPlusA : NWUnit
{


    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 5;
        //fleaMovementPoints = 1;
        //fleaActionPoints = 1;
    }

    public static void Promotion(NWUnit u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.waiting = true;
        u.isFlyer = true;
    }

}