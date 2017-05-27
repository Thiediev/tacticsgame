using UnityEngine;
using System.Collections;

public class Witch : Unit
{

    public static int cost = 3000;

    void Start()
    {
        attackRange = 2;
        movementPerActionPoint = 1;
        fleaActionPoints = 1;
        fleaMovementPoints = 1;

        HP = 10;
    }
}
