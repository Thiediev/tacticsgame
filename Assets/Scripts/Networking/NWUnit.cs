using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NWUnit : NetworkBehaviour
{
    [SyncVar]
    public bool isMyUnit = false;

    //public List<Unit> unitList = new List<Unit>();

    public void Awake()
    {
        //GameManager.instance.RegisterAsUnitList(this);

        moveDestination = transform.position;

    }

    /*public void AddUnit(Unit unitToAdd)
    {
        this.unitList.Add(unitToAdd);
    }*/



    public int baseMatchupDamage = 0;



    [SyncVar]
    public Vector2 gridPosition = Vector2.zero;

    [SyncVar]
    public int id;

    public static NWUnit instance;

    public Vector3 moveDestination;
    public float moveSpeed = 10.0f;
    public int baseAttackRange = 1;
    public int baseMovementPerActionPoint = 5;
    public int baseCost = 0;
    public bool canCapture = false;
    public bool zoneUnit = false;
    public bool zoneBoost = false;
    public bool meterGain = false;

    public bool moving = false;
    public bool attacking = false;
    public bool waiting = false;
    public bool capturing = false;

    //who owns the units
    public bool isOwnedByPlayerOne = false;
    public bool isOwnedByPlayerTwo = false;
    public bool isDestroyed = false;

    public bool isFleaUpB = false;
    public bool isFlyer = false;
    public bool isHealer = false;

    public int fleaMovementPoints = 2;
    public int fleaActionPoints = 2;

    // currently, actionPoints are only used for counting the number of times a unit can MOVE
    public int actionPoints = 1;

    //who owns the units
 
    public int attackRange;

    public int movementPerActionPoint = 5;



    public string playerName = "Celery";
    public int HP = 100;
    public int damageBase= 55;
    public int baseLuck = 9;
    public float baseDamageReduction = 0.0f;


    //feb 6, no idea what this does
    [Command]
    public void CmdSetId(int newId)
    {
        id = newId;
    }

    //  no idea what this is 

    [ClientRpc]
    public void RpcSyncGameManagerPlayerIds(string serializedList)
    {
        var lst = JsonUtility.FromJson<int[]>(serializedList);

        var newOrderedList = new List<NWPlayer>();
        foreach (var id in lst)
        {
            //var added = GameManager.instance.players.Where(x => x.id == id).FirstOrDefault();
            //if (added == null)
            //{
            var added = GameObject.FindObjectsOfType<NWPlayer>().Where(x => x.id == id).First();
            //}
            newOrderedList.Add(added);
        }

        NWGameManager.instance.newPlayers = newOrderedList;
    }
    [Command]
    public void CmdSyncGameManagerPlayerIds(string serializedList)
    {
        var lst = JsonUtility.FromJson<int[]>(serializedList);

        var newOrderedList = new List<NWPlayer>();
        foreach (var id in lst)
        {
            //var added = GameManager.instance.players.Where(x => x.id == id).FirstOrDefault();
            //if (added == null)
            //{
            var added = GameObject.FindObjectsOfType<NWPlayer>().Where(x => x.id == id).First();
            //}
            newOrderedList.Add(added);
        }

        NWGameManager.instance.newPlayers = newOrderedList;
    }


    // 

    public void moveCurrentUnit(NWTile t)
    {
        // perhaps the issue is that when client is trying to move a unit, it's calling moveCurrentUnit, which is trying to call CmdMoveCurrentPlayer,
        // but the client cannot call this, so we get blocked.  
        // originally, we only had the line from CmdMoveCurrentPlayer here, not any of the other lines.
        // seems commands can only be sent from the local player object. so i think i just need to move this over to there?
        // but actually i cannot do that because it's a unit that's supposed to be calling it ('activeUnit').
        // hmm

        CmdMoveCurrentPlayer(JsonUtility.ToJson(t.gridPosition));


    }

    [Command]
    public void CmdMoveCurrentPlayer(string serializedGridPosition)
    {
        RpcMoveCurrentPlayer(serializedGridPosition);


        // i feel like this shouldn't be here but whatever we'll test without it later.
        NWGameManager.instance.moveCurrentUnit(serializedGridPosition);
    }
    [ClientRpc]
    public void RpcMoveCurrentPlayer(string serializedGridPosition)
    {


        //moved here recently might be terrible9/29/16 slime
        NWUnit activeUnit = null;


        // check if the player reticle is on top of a unit; make that unit active if so
        foreach (NWUnit u in NWUnitList.instance.unitList)
        {
            // theory: something about being on the client and therefore no unit component, only gameobject. uhhh kind of a stretch though
            NWUnit reversionUnit = u.GetComponent<NWUnit>();

            if (reversionUnit.gridPosition == gridPosition)
            {
                activeUnit = reversionUnit;
                break;
            }
        }


        // this sometimes is not being called
        // seems to be as such: when client presses m, server does not do this. server needs to do this because server is
        // the one actually moving stuff.  if server does this then it should have all the tools to move the unit?

        if (activeUnit != null)
        {
            NWGameManager.myUnit = activeUnit;
        }


        if (activeUnit == null)
        {
            print("haha bullet is null for some reason");
        }
        // count is not increasing for client, which it should be.
        // print(GameManager.instance.unitObjects.Count);
        print("yay, there is " + activeUnit.gridPosition);
        // hopefully move unit


        // cmdmovecurrentunit called here?  then server does crap uh hm

        //TEST BULLSHITCmdDoSomethingUseful(Tile.myTile);
        NWGameManager.instance.moveCurrentUnit(serializedGridPosition);
    }


    // movement animation
    public List<Vector3> positionQueue = new List<Vector3>();



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (waiting)
        {
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (waiting == false && NWGameManager.myUnit == null)
        {
            transform.GetComponent<Renderer>().material.color = Color.white;
        }
        if (HP <= 0)
        {
            NWGameManager.KillUnit(this);

        }
    }


    // maybe need to rework this for networking if playerOneTurn/playerTwoTurn aren't being synced by syncvar 

    public void GoToNextTurn()
    {
        //if (isServer)
        //{
        if (NWGameManager.instance.playerOneTurn)
        {
            NWGameManager.instance.playerOneTurn = false;
            NWGameManager.instance.playerTwoTurn = true;
            int playerTwoIncome = (NWGameManager.instance.numberOfPropsArmyTwo * 1000);
            NWGameManager.instance.fundsArmyTwo += playerTwoIncome;
        }
        else
                   if (NWGameManager.instance.playerTwoTurn)
        {
            NWGameManager.instance.playerOneTurn = true;
            NWGameManager.instance.playerTwoTurn = false;
            int playerOneIncome = (NWGameManager.instance.numberOfPropsArmyOne * 1000);
            NWGameManager.instance.fundsArmyOne += playerOneIncome;
        }
        //GameManager.instance.players.ForEach(x => x.actionPoints = 2);
        //players.Where(x => x.isLocalPlayer).First().SetCurrentPlayerIndex(currentPlayerIndex);

        //}
    }

    //TODO: i dont know what to do with this yet
    public virtual void TurnUpdate()
    {
        if (positionQueue.Count > 0)
        {
            transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
            {
                transform.position = positionQueue[0];
                positionQueue.RemoveAt(0);
                if (positionQueue.Count == 0)
                {
                    //feb 20 actionPoints--;
                }
                // GameManager.instance.NextTurn();  
                // (probably best to have something like this for a chesslike but for an AWlike the current code makes more sense)
            }
        }

    }


    public void SetGridPosition(Vector2 newPos)
    {
        CmdSetGridPosition(newPos);
    }
    public void callNextTurn()
    {
        GoToNextTurn();
    }
    [Command]
    public void CmdSetGridPosition(Vector2 newPos)
    {
        gridPosition = newPos;

        //transform.position = new Vector3(gridPosition.x - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -gridPosition.y + Mathf.Floor(GameManager.instance.mapSize / 2));
    }
    [Command]
    public void CmdUpdateTransformPosition()
    {
        transform.position = new Vector3(gridPosition.x - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -gridPosition.y + Mathf.Floor(GameManager.instance.mapSize / 2));
    }

    public virtual void TurnOnGUI()
    {

    }

    public void OnGUI()
    {
        

    }
}




