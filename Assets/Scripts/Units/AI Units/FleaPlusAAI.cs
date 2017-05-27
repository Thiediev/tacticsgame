﻿using UnityEngine;
using System.Collections;

public class FleaPlusAAI : AIPlayerFix{


    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 4;
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
