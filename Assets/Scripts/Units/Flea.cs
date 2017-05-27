using UnityEngine;
using System.Collections;

public class Flea : Unit {

    public static int cost = 2000;

    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 3;
        fleaMovementPoints = 1;
        fleaActionPoints = 1;

        HP = 10;

    }
}
