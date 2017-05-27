using UnityEngine;
using System.Collections;

public class WitchPlusAAI : AIPlayerFix {

    void Start()
    {
        attackRange = 3;
        movementPerActionPoint = 1;
    }

    public static void Promotion(AIPlayerFix u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.waiting = true;
        u.fleaMovementPoints = 0;
        u.fleaActionPoints = 0;
    }
}
