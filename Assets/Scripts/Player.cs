using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

    public Vector2 gridPosition = Vector2.zero;

    public static Player instance;


    public Vector3 moveDestination;
    public float moveSpeed =  10.0f;
    public int baseAttackRange = 1;
    public int baseMovementPerActionPoint = 5;
    public int baseCost = 0;

    public bool moving = false;
    public bool attacking = false;
    public bool waiting = false;
    public bool inactive = false;


    //who owns the units
    public bool isOwnedByPlayerOne = false;
    public bool isOwnedByPlayerTwo = false;
    public bool isOwnedByAI = false;
    public bool isDestroyed = false;

    public bool isFleaUpB = false;
    public bool isFlyer = false;
    public bool isHealer = false;

    // fleaMovementPoints is to make sure the flea (when it's upgraded to promotion B) can only move twice.
    // fleaActionPoints is to make sure that it can still attack twice.
    public int fleaMovementPoints = 2;
    public int fleaActionPoints = 2;

    public int attackRange;

    public int movementPerActionPoint = 5;

    public int damageBase = 55;


    public string playerName = "Celery";
    public int HP = 10;

    


    // movement animation
    public List<Vector3> positionQueue = new List<Vector3>();


    void Awake ()
    {
        moveDestination = transform.position;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
        if (waiting)
        {
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (waiting == false && GameManager.myUnit == null)
        {
            transform.GetComponent<Renderer>().material.color = Color.white;
        }

        /*
        if (HP <= 0)
        {
            GameManager.KillUnit(this);

        
        }
        */
    }


    //TODO: i dont know what to do with this yet
    public virtual void TurnUpdate()
    {
        /*if (actionPoints <= 0)
        {
            actionPoints = 2;
            moving = false;
            attacking = false;
            GameManager.instance.NextTurn();
        }*/
    }

    public virtual void TurnOnGUI()
    {

    }

    public void OnGUI()
    {
     
    }
}


