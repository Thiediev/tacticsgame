using UnityEngine;
using System.Collections;

public class Spider : Unit
{

    public static int cost = 4000;
    
    void Start()
    {
        attackRange = 1;
        movementPerActionPoint = 2;
        fleaActionPoints = 1;
        fleaMovementPoints = 1;

        HP = 10;
    }
   
}
