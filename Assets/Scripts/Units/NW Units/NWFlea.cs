using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class NWFlea : NWUnit
{

    public static int cost = 2000;

    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 5;
        fleaMovementPoints = 1;
        fleaActionPoints = 1;

        HP = 10;

    }
}
