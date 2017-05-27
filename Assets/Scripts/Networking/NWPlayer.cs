using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class NWPlayer : NetworkBehaviour
{
    public NWFlea nwFlea1;
    public NWUnit myUnit;

    public bool highlightsOn = false;

    [SyncVar]
    public Vector2 gridPosition = Vector2.zero;

    [SyncVar]
    public int id;




    public static NWPlayer instance;


    public Vector3 moveDestination;
    public float moveSpeed = 10.0f;
    public int baseAttackRange = 1;
    public int baseMovementPerActionPoint = 5;
    public int baseCost = 0;

    public bool moving = false;
    public bool attacking = false;
    public bool waiting = false;
    public bool inactive = false;

    
    

    // fleaMovemenPoints is to make sure the flea (when it's upgraded to promotion B) can only move twice.
    // fleaActionPoints is to make sure that it can still attack twice.
 


    public int damageBase = 55;


    public string playerName = "Celery";
    public int HP = 10;




    // movement animation
    public List<Vector3> positionQueue = new List<Vector3>();


    void Awake()
    {
        moveDestination = transform.position;
    }

    // Use this for initialization
    void Start()
    {

    }

    [Command]
    public void CmdSetId(int newId)
    {
        id = newId;
    }

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

        //feb 20
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

    [Command]
    public void CmdSpawnInfantry()
    {

        Debug.Log("ummmm this is happening");
        // create server-side instance
        GameObject fleaObject = (GameObject)Instantiate(NWGameManager.instance.FleaPrefab, transform.position, Quaternion.identity);
        // setup bullet component
        nwFlea1 = fleaObject.GetComponent<NWFlea>();
        nwFlea1.gridPosition = new Vector2(transform.position.x + Mathf.Floor(NWGameManager.instance.mapSize / 2), -transform.position.z + Mathf.Floor(NWGameManager.instance.mapSize / 2));

        // need to do stuff for p1 vs p2 here
        nwFlea1.isOwnedByPlayerOne = true;

        // spawn on the clients
        NetworkServer.Spawn(fleaObject);

        // adding 'bullet' to the list of units      
        AddToList(fleaObject);

        RpcAddToList(fleaObject);
    }


    public void moveCurrentUnit(NWTile t)
    {

        CmdMoveCurrentUnit(JsonUtility.ToJson(t.gridPosition));
        //GameManager.instance.moveCurrentPlayer(t);
    }
    [Command]
    public void CmdMoveCurrentUnit(string serializedGridPosition)
    {
        RpcMoveCurrentUnit(serializedGridPosition);
        // i just did this now in 9/29/16 slime year GameManager.instance.moveCurrentUnit(serializedGridPosition);
    }
    [ClientRpc]
    public void RpcMoveCurrentUnit(string serializedGridPosition)
    {


        //moved here recently might be terrible9/29/16 slime
        NWUnit activeUnit = null;


        // check if the player reticle is on top of a unit; make that unit active if so
        foreach (NWUnit u in NWUnitList.instance.unitList)
        {
            /*
            // theory: something about being on the client and therefore no unit component, only gameobject. uhhh kind of a stretch though
            NWUnit reversionUnit = u.GetComponent<NWUnit>();

            if (reversionUnit.gridPosition == gridPosition)
            {
                activeUnit = reversionUnit;
                break;
            }
            */

            NWUnit reversionUnit = u.GetComponent<NWUnit>();

            if (reversionUnit.isMyUnit)
            {
                activeUnit = reversionUnit;
                reversionUnit.isMyUnit = false;
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

        //NETWORKING BULLSHIT
        if (Input.GetKeyDown(KeyCode.J) && isLocalPlayer)
        {
            // spawn unit, this is a misnomer atm
            CmdSpawnInfantry();

            // is this just not a unit or something.  it's not working

            // this.infantry is null on the non-host client
            // which indicates that CmdSpawnInfantry isn't assigning infantry on the non-host for some reason
            // probably
            // (this down here is currently accomplishing nothing
            //UnitList.instance.AddUnit(this.infantry);
        }
        if (Input.GetKeyDown(KeyCode.D) && isLocalPlayer)
        {
            /*
            if (highlightsOn == true)
            {
                NWGameManager.instance.removeTileHighlights();
                highlightsOn = false;

                foreach (NWUnit u in NWUnitList.instance.unitList)
                    u.isMyUnit = false;

            }
            else
if (highlightsOn == false)
            { */
            foreach (NWUnit u in NWUnitList.instance.unitList)
            {
                if (this.gridPosition == u.gridPosition)
                {
                        if (isServer)
                        {
                            NWGameManager.instance.highlightTilesAt(gridPosition, Color.blue, nwFlea1.baseMovementPerActionPoint, false, false);
                        }
                        else
if (isClient)
                    {
                        NWGameManager.instance.highlightTilesAt(gridPosition, Color.blue, nwFlea1.baseMovementPerActionPoint, false, false);
                    }                  
                        highlightsOn = true;
                        // make this unit into MyUnit on both clients so movement can function
                        u.isMyUnit = true;

                    }

                }

            }

      //  }

        //if (isLocalPlayer)
        //{
        if (Input.GetKeyDown(KeyCode.M) && isLocalPlayer)
        //if (Tile.instance.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && Tile.instance.impassable != true 
        //&& GameManager.myUnit.moving
        // )
        {
            foreach (List<NWTile> l in NWGameManager.instance.map)
            {
                foreach (NWTile t in l)
                {
                    if (this.gridPosition == t.gridPosition)
                    {
                        NWTile.myTile = t;
                    }
                }
            }

            
            /*
            if (HP <= 0)
            {
                GameManager.KillUnit(this);


            }
            */
            if (isServer)
            {
                // GONE CUZ TESTING, WILL PUT BACK IF I FAIL ererrrer
                moveCurrentUnit(NWTile.myTile);

            }
            else
            {
                DoSomethingUseful(NWTile.myTile);
            }
        }
    }

    // 
    
    



    //

    public void DoSomethingUseful(NWTile t)
    {
        CmdDoSomethingUseful(JsonUtility.ToJson(t.gridPosition));
    }

    [Command]
    public void CmdDoSomethingUseful(string serializedGridPosition)
    {

        // Unit.instance should be replaced with activeUnit in the event of failure
        // also this is supposed to be activeUnit (or whatever. read above line).rpcmovecurrentPLAYER, but i changed it to unit so now it's using 
        // the code from the Player class that i just now am seeing. slime year
        RpcMoveCurrentUnit(serializedGridPosition);
    }


    public void SetGridPosition(Vector2 newPos)
    {
        if (isLocalPlayer)
        {
            CmdSetGridPosition(newPos);
        }
    }
    public void callNextTurn()
    {
        GoToNextTurn();
    }
    [Command]
    public void CmdSetGridPosition(Vector2 newPos)
    {
        gridPosition = newPos;
    }
    //transform.position = new Vector3(gridPosition.x - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -gridPosition.y + Mathf.Floor(GameManager.instance.mapSize / 2));

    [Command]
    public void CmdUpdateTransformPosition()
    {
        transform.position = new Vector3(gridPosition.x - Mathf.Floor(NWGameManager.instance.mapSize / 2), 1.5f, -gridPosition.y + Mathf.Floor(NWGameManager.instance.mapSize / 2));
    }


    public void AddToList(GameObject obj)
    {
        //if (isLocalPlayer && obj.GetComponent<NetworkIdentity>() != null)
        {
            CmdAddToList(obj);
        }
    }

    [Command]
    void CmdAddToList(GameObject obj)
    {
        // this code is only executed on the server
        RpcAddToList(obj); // invoke Rpc on all clients
    }

    [ClientRpc]
    void RpcAddToList(GameObject obj)
    {
        // this code is executed on all clients
        //GameManager.instance.unitObjects.Add(obj);
        NWUnitList.instance.AddUnit(obj.GetComponent<NWUnit>());
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


