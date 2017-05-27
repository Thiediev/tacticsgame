using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;


public class NWUserPlayer : NWPlayer
{

    public void Start()
    {

        if (isLocalPlayer)
        {
            NWGameManager.instance.gameObject.SetActive(true);
            NWGameManager.instance.generateMap();
            //NWGameManager.instance.generatePlayers();

            //change funds when units are spawned online.  will have to look at previous backup for non-online shit because there's too much stuff changing now
            NWGameManager.instance.fundsArmyOne = (NWGameManager.instance.numberOfPropsArmyOne * 1000) / 2;
            NWGameManager.instance.fundsArmyTwo = 0;

            // i think this is just 'where are the players being spawned'
            // therefore i think it should be newPlayers as opposed to units in the start function
            // later i probably dont want these things on the grid anyway.
            do
            {
                gridPosition = new Vector2(Random.Range(0, NWGameManager.instance.mapSize), Random.Range(0, NWGameManager.instance.mapSize));
            } while (NWGameManager.instance.newPlayers.Where(x => x.gridPosition == gridPosition).Any() || NWGameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y].impassable);
            CmdSetGridPosition(gridPosition);
            CmdUpdateTransformPosition();

            transform.position = new Vector3(gridPosition.x - Mathf.Floor(NWGameManager.instance.mapSize / 2), 1.5f, -gridPosition.y + Mathf.Floor(NWGameManager.instance.mapSize / 2));
            transform.rotation = Quaternion.Euler(Vector3.zero);
            string playerName = "Player - " + NWGameManager.instance.newPlayers.Count.ToString();
            id = (int)GetComponent<NetworkIdentity>().netId.Value;
            CmdSetId((int)GetComponent<NetworkIdentity>().netId.Value);
            NWGameManager.instance.newPlayers.Add(this);
            //CmdSyncGameManagerPlayerIds(JsonUtility.ToJson(GameObject.FindObjectsOfType<Player>()));
            //RpcSyncGameManagerPlayerIds(JsonUtility.ToJson(GameObject.FindObjectsOfType<Player>()));
        }
        else
        {
            transform.position = new Vector3(gridPosition.x - Mathf.Floor(NWGameManager.instance.mapSize / 2), 1.5f, -gridPosition.y + Mathf.Floor(NWGameManager.instance.mapSize / 2));
            NWGameManager.instance.newPlayers.Add(this);
            //CmdSyncGameManagerPlayerIds(JsonUtility.ToJson(GameObject.FindObjectsOfType<Player>().ToArray()));
            //RpcSyncGameManagerPlayerIds(JsonUtility.ToJson(GameObject.FindObjectsOfType<Player>().ToArray()));
        }
    }


    IEnumerator StartRoutine()
    {
        while (NWGameManager.instance == null || !NWGameManager.instance.gameObject.activeSelf)
        {
            yield return null;
        }
    }


    // Update is called once per frame
    public override void Update()
    {
        // add a bunch of cmd and rpc bullshit for this i guessf
        if (isLocalPlayer)
        {

            //up arrow
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                CursorMoveUp();

            }
            //down arrow
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {

                CursorMoveDown();

                //transform.position = new Vector3(gridPosition.x - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -gridPosition.y + Mathf.Floor(GameManager.instance.mapSize / 2)); 
            }
            //left arrow
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {


                CursorMoveLeft();

                //transform.position = new Vector3(gridPosition.x  - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -(gridPosition.y) + Mathf.Floor(GameManager.instance.mapSize / 2));
            }
            //right arrow
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {

                CursorMoveRight();

                //transform.position = new Vector3(gridPosition.x  - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -(gridPosition.y) + Mathf.Floor(GameManager.instance.mapSize / 2));
            }
            transform.position = new Vector3(gridPosition.x - Mathf.Floor(NWGameManager.instance.mapSize / 2), 1.5f, -gridPosition.y + Mathf.Floor(NWGameManager.instance.mapSize / 2));


            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (NWUnit u in NWGameManager.instance.units)
                {
                    if (u.gridPosition == gridPosition)
                    {
                        NWGameManager.myUnit = u;
                    }
                }
                Debug.Log("myUnit is " + NWGameManager.myUnit.gridPosition);


                // if the unit is owned by the player whose turn it is, can move unit
                //if ((GameManager.instance.playerOneTurn && GameManager.instance.players[GameManager.instance.currentPlayerIndex].isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.instance.players[GameManager.instance.currentPlayerIndex].isOwnedByPlayerTwo))
                if ((NWGameManager.instance.playerOneTurn && NWGameManager.myUnit.isOwnedByPlayerOne) || (NWGameManager.instance.playerTwoTurn && NWGameManager.myUnit.isOwnedByPlayerTwo))
                {
                    if (!NWGameManager.myUnit.moving && !NWGameManager.myUnit.waiting)
                    {
                        NWGameManager.instance.removeTileHighlights();
                        NWGameManager.myUnit.moving = true;
                        NWGameManager.myUnit.attacking = false;
                        NWGameManager.myUnit.waiting = false;
                        NWGameManager.instance.highlightTilesAt(gridPosition, Color.blue, NWGameManager.myUnit.baseMovementPerActionPoint, false, false);
                    }
                    else
                    {

                        NWGameManager.myUnit.moving = false;
                        NWGameManager.myUnit.attacking = false;
                        NWGameManager.instance.removeTileHighlights();
                    }
                }
            }


            if (NWGameManager.myUnit == this)
            {
                if ((NWGameManager.instance.playerOneTurn && NWGameManager.myUnit.isOwnedByPlayerOne) || (NWGameManager.instance.playerTwoTurn && NWGameManager.myUnit.isOwnedByPlayerTwo))
                {
                    transform.GetComponent<Renderer>().material.color = Color.black;
                }
            }
            else
            {
                transform.GetComponent<Renderer>().material.color = Color.white;
            }

            base.Update();
        }
    }

    public void CursorMoveUp()
    {
        CmdCursorMoveUp();
    }
    [Command]
    public void CmdCursorMoveUp()
    {
        gridPosition = new Vector2(gridPosition.x, gridPosition.y - 1);
    }
    [ClientRpc]
    public void RpcCursorMoveUp()
    {
    }

    // down
    public void CursorMoveDown()
    {
        CmdCursorMoveDown();
    }
    [Command]
    public void CmdCursorMoveDown()
    {
        gridPosition = new Vector2(gridPosition.x, gridPosition.y + 1);
    }
    [ClientRpc]
    public void RpcCursorMoveDown()
    {
    }

    // left
    public void CursorMoveLeft()
    {
        CmdCursorMoveLeft();
    }
    [Command]
    public void CmdCursorMoveLeft()
    {
        gridPosition = new Vector2(gridPosition.x - 1, gridPosition.y);
    }
    [ClientRpc]
    public void RpcCursorMoveLeft()
    {
    }

    // right
    public void CursorMoveRight()
    {
        CmdCursorMoveRight();
    }
    [Command]
    public void CmdCursorMoveRight()
    {
        gridPosition = new Vector2(gridPosition.x + 1, gridPosition.y);
    }
    [ClientRpc]
    public void RpcCursorMoveRight()
    {
    }


    // this could be the issue here
    public override void TurnUpdate()
    {
        // highlight
        // 
        // 

        if (positionQueue.Count > 0)
        {
            transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
            {
                transform.position = positionQueue[0];
                positionQueue.RemoveAt(0);
                if (positionQueue.Count == 0)
                {
                    // probably have to handle this elsewhere
                    //myUnit.instance.fleaMovementPoints--;
                    /// if (fleaMovementPoints == 0)
                    // {
                    //   fleaActionPoints--;
                    // }

                }
                // GameManager.instance.NextTurn();  
                // (probably best to have something like this for a chesslike but for an AWlike the current code makes more sense)
            }
        }

        base.TurnUpdate();
    }

    public override void TurnOnGUI()
    {

        base.TurnOnGUI();
    }


    // choose a unit. or not
    void OnMouseDown()
    {
        /* PER NETWORKING we are getting rid of this but uh we will probably need at least some of it later
        //active unit becomes the one selected i hope
        NWGameManager.myUnit = this;

        // if the unit is owned by the player whose turn it is, can move unit
        //if ((GameManager.instance.playerOneTurn && GameManager.instance.players[GameManager.instance.currentPlayerIndex].isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.instance.players[GameManager.instance.currentPlayerIndex].isOwnedByPlayerTwo))
        if ((GameManager.instance.playerOneTurn && GameManager.myUnit.isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.myUnit.isOwnedByPlayerTwo))
        {
            // if flea promotion B, you get an extra action
            // if (isFleaUpB)
            // {
            if (fleaMovementPoints > 0)
            {
                if (!moving && !waiting)
                {
                    NWGameManager.instance.removeTileHighlights();
                    moving = true;
                    attacking = false;
                    waiting = false;
                    NWGameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false, false);
                }
                else
                {

                    moving = false;
                    attacking = false;
                    NWGameManager.instance.removeTileHighlights();
                }
            }
            //   }
            //else

            // if flea promotion A, you can fly over enemies and walls
            /* if (isFlyer)
             {
                 if (!moving && !waiting)
                 {
                     GameManager.instance.removeTileHighlights();
                     moving = true;
                     attacking = false;
                     //  waiting = false;
                     //cyan for flyers, magenta for healing?
                     GameManager.instance.highlightTilesAt(gridPosition, Color.cyan, movementPerActionPoint);
                 }
                 else

                     {

                     moving = false;
                     attacking = false;
                     GameManager.instance.removeTileHighlights();
                 }
                     }
             else
             {

                 // all other units move normally
                 if (!moving && !waiting)
                 {
                     GameManager.instance.removeTileHighlights();
                     moving = true;
                     attacking = false;
                     //  waiting = false;
                     GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false);
                 }
                 else
                 {

                     moving = false;
                     attacking = false;
                     GameManager.instance.removeTileHighlights();
                 }
                 */
        }
    }




