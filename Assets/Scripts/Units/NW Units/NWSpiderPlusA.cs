﻿using UnityEngine;
using System.Collections;

public class NWSpiderPlusA : NWUnit
{


    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 3;
    }

    public static void Promotion(NWUnit u, int h)
    {
        // upon promotion, HP matches HP of original form, and unit is waiting.
        u.HP = h;
        u.waiting = true;
    }
}
