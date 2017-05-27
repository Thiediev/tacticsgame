using UnityEngine;
using System.Collections;

public class NWFleaPlusB : NWUnit
{


    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 5;

    }

    public static void Promotion(NWUnit u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.fleaActionPoints = 1;
        u.fleaMovementPoints = 1;
    }

}
