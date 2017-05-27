using UnityEngine;
using System.Collections;

public class HQ : Unit
{
    bool endGame = true;
    void Start()
    {
        attackRange = 0;
        movementPerActionPoint = 0;
        HP = 100;
        
    }
}
