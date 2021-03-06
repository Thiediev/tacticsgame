﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;



public class NWGameManager : NetworkBehaviour
{

    public int currentCampaignMap = 1;

    //
    public static NWUnit myUnit;

    //
    public int turnCount = 1;

    public static NWGameManager instance;

    public GameObject TilePrefab;
    public GameObject AIPlayerPrefab;
    public GameObject UserPlayerPrefab;

    public GameObject highlights;
    public List<GameObject> highlightTiles;

    public int numberOfPropsArmyOne
    {
        get
        {
            int armyOneProps = 0;
            // for each tile on the map, if addsIncomePlayerOne is true, then armyOneProps++.
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i][j].addsIncomePlayerOne)
                    {
                        armyOneProps++;
                    }
                }
            }
            return armyOneProps;

        }
        set { }
    }

    public int numberOfPropsArmyTwo
    {
        get
        {
            int armyTwoProps = 0;
            // for each tile on the map, if addsIncomePlayerOne is true, then armyOneProps++.
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i][j].addsIncomePlayerTwo)
                    {
                        armyTwoProps++;
                    }
                }
            }
            return armyTwoProps;

        }
        set { }
    }


    //which army owns this unit.  may not need these
    //public bool isOwnedByPlayerOne;
    //public bool isOwnedByPlayerTwo;

    //specific unit prefabs
    //player 1


    public GameObject HQPrefab;

    public GameObject FleaPrefab;
    public GameObject FleaUpAPrefab;
    public GameObject FleaUpBPrefab;

    public GameObject SpiderPrefab;
    public GameObject SpiderUpAPrefab;
    public GameObject SpiderUpBPrefab;

    public GameObject WitchPrefab;
    public GameObject WitchUpAPrefab;
    public GameObject WitchUpBPrefab;


    //player 2

    public GameObject P2HQPrefab;

    public GameObject P2FleaPrefab;
    public GameObject P2FleaUpAPrefab;
    public GameObject P2FleaUpBPrefab;

    public GameObject P2SpiderPrefab;
    public GameObject P2SpiderUpAPrefab;
    public GameObject P2SpiderUpBPrefab;

    public GameObject P2WitchPrefab;
    public GameObject P2WitchUpAPrefab;
    public GameObject P2WitchUpBPrefab;


    // ai units

    public GameObject AIFleaPrefab;
    public GameObject AIFleaUpAPrefab;
    public GameObject AIFleaUpBPrefab;

    public GameObject AISpiderPrefab;
    public GameObject AISpiderUpAPrefab;
    public GameObject AISpiderUpBPrefab;

    public GameObject AIWitchPrefab;
    public GameObject AIWitchUpAPrefab;
    public GameObject AIWitchUpBPrefab;



    public GameObject WinScreenPrefab;
    public GameObject LoseScreenPrefab;


    Transform mapTransform;

    public int mapSize = 11;



    // Control AI unit delay
    public bool resumeIn3Seconds = false;
    public bool startedPausing = false;


    public List<List<NWTile>> map = new List<List<NWTile>>();
    //public List<NWPlayer> players = new List<NWPlayer>();

    //NETWORKSHIT


    // perhaps some sort of gameobject id system would be better for syncing or whatever, i dont know

    public List<NWUnit> units = new List<NWUnit>();
    public List<Vector2> unitPositions = new List<Vector2>();

    public List<NWPlayer> newPlayers = new List<NWPlayer>();


    //

    // list of ai players
    public List<Player> aiPlayers = new List<Player>();
    public int currentAIUnitIndex = 0;
    public int numberOfActiveAIUnits = 0;

    [SyncVar]
    public int currentPlayerIndex = 0;
    public int currentArmyIndex = 0;

    // total number of each faction's units
    public static int playerOneCount = 0;
    public static int playerTwoCount = 0;

    public static int playerAICount = 0;

    // keep track of whose turn it is
    [SyncVar]
    public bool playerOneTurn = true;
    [SyncVar]
    public bool playerTwoTurn = false;

    // ehhhhhhhhhhhh
    public bool firstTurn = true;
    public bool diedOnCounter = false;
    public bool unitIsDead = false;



    public bool aiPlayerTurn = false;

    public int fundsArmyOne;
    public int fundsArmyTwo;


    //NETWORKING
    [ClientRpc]
    public void RpcSpawn()
    {
        Debug.Log("alphafalpha");
        var go = ((GameObject)Instantiate(FleaPrefab, new Vector3(NWTile.myTile.transform.position.x, 0.5f, NWTile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0))));
        //go.gridPosition = new Vector2(Tile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -Tile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
        //go.playerName = "Juicenjam";
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);


        //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
        //newInfantry.gridPosition = new Vector2(0, 0);
    }


    //







    void Awake()
    {
        instance = this;
        mapTransform = transform.FindChild("Map");

    }

    // Use this for initialization
    void Start()
    {
        generateMap();
        generatePlayers();


        // first player gets half the funds on the first day to counteract first turn advantage.  perhaps this is too much but whatever
        fundsArmyOne = (numberOfPropsArmyOne * 1000) / 2;
        fundsArmyTwo = 0;
    }



    // Update is called once per frame
    void Update()
    {

        // if a unit dies, this is where we cycle to the next unit.
        // this probably makes no sense and what we should actually be probably doing is having GUI stuff all be based on MyUnit and stuff but whatever for now.
        /*
              if (aiPlayers[currentAIUnitIndex] == null)
              {
                  currentAIUnitIndex++;
                  currentPlayerIndex++;
              }

              // the UI only exists if the current unit in the index is selected.  this is a terrible system but for now i'm keeping it that way.
              // so this is to make sure the UI never disappars.
              /*
          if (players[currentPlayerIndex] == null)
          {
              currentPlayerIndex++;
              if (currentPlayerIndex + 1 > playerOneCount)
              {
                  currentPlayerIndex = 0;
              }
          }
          */

        //this is older schema, will get rid of once turn stuff is more updated
        /* if (players[currentPlayerIndex].HP > 0)
          {
              players[currentPlayerIndex].TurnUpdate();
          }
          else
          {
              NextTurn();
          }*/
        if (myUnit == null)
        {
            // print("no unit chosen yet");
        }
        else
        if (myUnit.HP > 0)
        {
            myUnit.TurnUpdate();
        }
        /*
        else
        {
            NextTurn();
        }*/


        //TODO: make this work for specific armies
        if (playerOneCount <= 0)
        {
            //print("PLAYER TWO WINS MY DUDE");


        }
        if (playerTwoCount <= 0)
        {
            print("next level please");


            // IF THIS IS SCENE
            /*
            currentCampaignMap++;
            if (currentCampaignMap <= 7)
            {
                generateMap();
            }
            if (currentCampaignMap > 7)
            {
                currentCampaignMap = 7;
                //SceneManager.LoadScene("Main Menu");
            }
            // print("PLAYER ONNNE WINS YEAH");
            */
        }
        if (playerAICount <= 0)
        {






        }
    }

    private NWUnitList unitList;

    public void RegisterAsUnitList(NWUnitList unitList)
    {
        this.unitList = unitList;
    }

    // AI Unit Delay
    public void ResumeIn3Seconds()
    {

        startedPausing = false;

        StartCoroutine(ResumeAfterSeconds(0.5f));


    }


    private IEnumerator ResumeAfterSeconds(float resumetime) // 3
    {
        Time.timeScale = 0.0001f;
        float pauseEndTime = Time.realtimeSinceStartup + resumetime; // 10 + 4 = 13

        float number3 = Time.realtimeSinceStartup + 1; // 10 + 1 = 11
        float number2 = Time.realtimeSinceStartup + 2; // 10 + 2 = 12
        float number1 = Time.realtimeSinceStartup + 3; // 10 + 3 = 13

        while (Time.realtimeSinceStartup < pauseEndTime) // 10 < 13
        {

            yield return null;
        }
        resumeIn3Seconds = false;
        startedPausing = false;
        Time.timeScale = 1;
    }




    public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance)
    {
        highlightTilesAt(originLocation, highlightColor, distance, true, false);
    }

    public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance, bool ignorePlayers, bool invisibleHighlights)
    {
        List<NWTile> highlightedTiles = new List<NWTile>();
        if (ignorePlayers)
        {
            highlightedTiles = NWTileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance, highlightColor == Color.red || highlightColor == Color.cyan || highlightColor == Color.magenta);
        }
        else if (playerOneTurn)
        {
            highlightedTiles = NWTileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance, units.Where(x => x.gridPosition != originLocation && x.isOwnedByPlayerOne == false && x.isDestroyed == false).Select(x => x.gridPosition).ToArray(), highlightColor == Color.red);
        }
        else if (playerTwoTurn)
        {
            highlightedTiles = NWTileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance, units.Where(x => x.gridPosition != originLocation && x.isOwnedByPlayerTwo == false && x.isDestroyed == false).Select(x => x.gridPosition).ToArray(), highlightColor == Color.red);

        }


        if (invisibleHighlights == false)
        {
            foreach (NWTile t in highlightedTiles)
            {
                t.visual.transform.GetComponent<Renderer>().materials[0].color = highlightColor;
                highlights = (GameObject)Instantiate(PrefabHolder.instance.HIGHLIGHT_TILE, new Vector3((int)t.gridPosition.x - Mathf.Floor(mapSize / 2), 0.55f, -(int)t.gridPosition.y + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3(0, 0, 180)));
                highlightTiles.Add(highlights);
                NetworkServer.Spawn(highlights);

            }
        }
        else
        {
            foreach (NWTile t in highlightedTiles)
            {
                t.visual.transform.GetComponent<Renderer>().materials[0].color = highlightColor;
                highlights = (GameObject)Instantiate(PrefabHolder.instance.AI_HIGHLIGHT, new Vector3((int)t.gridPosition.x - Mathf.Floor(mapSize / 2), 0.55f, -(int)t.gridPosition.y + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3(0, 0, 180)));
                highlightTiles.Add(highlights);
            }
        }
    }


    
     

    public void removeTileHighlights()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {

                map[i][j].visual.transform.GetComponent<Renderer>().materials[0].color = Color.white;
            }
        }
        foreach (GameObject h in highlightTiles)
        {
            Destroy(h);
        }

    }

    void OnGUI()
    {

        if (units[currentPlayerIndex].HP > 0)
        {
            units[currentPlayerIndex].TurnOnGUI();
        }

        /*
        Vector3 incomeLocationOne = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 120 + Vector3.left * 250;
        GUI.TextArea(new Rect(incomeLocationOne.x, Screen.height - incomeLocationOne.y, 100, 20), "P1: $" + fundsArmyOne.ToString());

        Vector3 incomeLocationTwo = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 120 + Vector3.right * 150;
        GUI.TextArea(new Rect(incomeLocationTwo.x, Screen.height - incomeLocationTwo.y, 100, 20), "P2: $" + fundsArmyTwo.ToString());
        */
    }




    public void NextTurn()
    {
        // TODO: 'turn index' that keeps count of what turn you're on in number of days etc.

        // ends turn for whichever player is active when they press the end turn button
        if (playerOneTurn)
        {
            playerOneTurn = false;
            playerTwoTurn = true;
            int playerTwoIncome = (numberOfPropsArmyTwo * 1000);
            fundsArmyTwo += playerTwoIncome;


            foreach (NWUnit p in NWGameManager.instance.units)
            {
                if (p != null)
                {
                    if (p.isFleaUpB)
                    {
                        p.fleaActionPoints = 2;
                        p.fleaMovementPoints = 2;
                    }
                    else
                    {
                        p.fleaActionPoints = 1;
                        p.fleaMovementPoints = 1;
                    }

                    p.waiting = false;
                    p.moving = false;
                    p.attacking = false;
                }
            }
        }
        else
            if (playerTwoTurn)
        {
            playerOneTurn = true;
            playerTwoTurn = false;
            int playerOneIncome = (numberOfPropsArmyOne * 1000);
            fundsArmyOne += playerOneIncome;
            turnCount++;


            foreach (NWUnit p in NWGameManager.instance.units)
            {
                if (p != null)
                {
                    if (p.isFleaUpB)
                    {
                        p.fleaActionPoints = 2;
                        p.fleaMovementPoints = 2;
                    }
                    else
                    {
                        p.fleaActionPoints = 1;
                        p.fleaMovementPoints = 1;
                    }

                    p.waiting = false;
                    p.moving = false;
                    p.attacking = false;
                }
            }
        }
        MapUI.instance.UpdateTurnCounter();


        /* old scheme, not outdated yet though
        if (currentPlayerIndex + 1 < players.Count)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }*/
    }



    public void moveCurrentUnit(string serializedTile)
    {
        Vector2 deserializedTile = JsonUtility.FromJson<Vector2>(serializedTile);

        NWTile destTile = NWGameManager.instance.map[(int)deserializedTile.x][(int)deserializedTile.y];



        //if (/*!destTile.impassible && players[currentPlayerIndex].positionQueue.Count == 0*/) 
        {
            removeTileHighlights();
            myUnit.moving = false;
            foreach (NWTile t in NWTilePathFinder.FindPath(NWGameManager.instance.map[(int)myUnit.gridPosition.x][(int)myUnit.gridPosition.y], destTile, NWGameManager.instance.units.Where(x => x.gridPosition != destTile.gridPosition && x.gridPosition != myUnit.gridPosition).Select(x => x.gridPosition).ToArray()))
            {
                myUnit.positionQueue.Add(NWGameManager.instance.map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 1.5f * Vector3.up);
                Debug.Log("(" + myUnit.positionQueue[myUnit.positionQueue.Count - 1].x + "," + myUnit.positionQueue[myUnit.positionQueue.Count - 1].y + ")");
            }
            myUnit.gridPosition = destTile.gridPosition;
            myUnit.SetGridPosition(myUnit.gridPosition);
        }
    }

        // if the unit is owned by the player whose turn it is, player can move it
        /*
            if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassable && players[currentPlayerIndex].positionQueue.Count == 0)
            {
                removeTileHighlights();
                players[currentPlayerIndex].moving = false;

                foreach (Tile t in TilePathFinder.FindPath(map[(int)players[currentPlayerIndex].gridPosition.x][(int)players[currentPlayerIndex].gridPosition.y], destTile, players.Where(x => x.gridPosition != players[currentPlayerIndex].gridPosition).Select(x => x.gridPosition).ToArray()))
                {
                    players[currentPlayerIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 1.5f * Vector3.up);
                }
                players[currentPlayerIndex].gridPosition = destTile.gridPosition;

            }
            else
            {
                Debug.Log("destination invalid");
            }
        
    }*/

        /* removed for NETWORKING but may need

        // if it's an AI player, you do it differently
        if (aiPlayerTurn)
        {
            if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassable && aiPlayers[currentAIUnitIndex].positionQueue.Count == 0)
            {
                removeTileHighlights();
                aiPlayers[currentAIUnitIndex].moving = false;


                // if units are from the same faction, ignore them in pathfinding (can move through them)
                if (playerOneTurn)
                {
                    foreach (Tile t in TilePathFinder.FindPath(map[(int)aiPlayers[currentAIUnitIndex].gridPosition.x][(int)aiPlayers[currentAIUnitIndex].gridPosition.y], destTile, players.Where(x => x.gridPosition != aiPlayers[currentAIUnitIndex].gridPosition && x.isOwnedByPlayerOne != true && x.isDestroyed != true).Select(x => x.gridPosition).ToArray()))
                    {
                        aiPlayers[currentAIUnitIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 0.55f * Vector3.up);
                    }
                }
                else if (playerTwoTurn)
                {
                    foreach (Tile t in TilePathFinder.FindPath(map[(int)aiPlayers[currentAIUnitIndex].gridPosition.x][(int)aiPlayers[currentAIUnitIndex].gridPosition.y], destTile, players.Where(x => x.gridPosition != aiPlayers[currentAIUnitIndex].gridPosition && x.isOwnedByPlayerTwo != true && x.isDestroyed != true).Select(x => x.gridPosition).ToArray()))
                    {
                        aiPlayers[currentAIUnitIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 0.55f * Vector3.up);
                    }
                }
                aiPlayers[currentAIUnitIndex].gridPosition = destTile.gridPosition;
                //aiPlayers[currentAIUnitIndex].actionPoints--;

            }
        }
        else
            if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassable && myUnit.positionQueue.Count == 0)
        {
            removeTileHighlights();
            myUnit.moving = false;


            // if units are from the same faction, ignore them in pathfinding (can move through them)
            if (playerOneTurn)
            {
                foreach (Tile t in TilePathFinder.FindPath(map[(int)myUnit.gridPosition.x][(int)myUnit.gridPosition.y], destTile, players.Where(x => x.gridPosition != myUnit.gridPosition && x.isOwnedByPlayerOne != true && x.isDestroyed != true).Select(x => x.gridPosition).ToArray()))
                {
                    myUnit.positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 0.55f * Vector3.up);
                }
            }
            else if (playerTwoTurn)
            {
                foreach (Tile t in TilePathFinder.FindPath(map[(int)myUnit.gridPosition.x][(int)myUnit.gridPosition.y], destTile, players.Where(x => x.gridPosition != myUnit.gridPosition && x.isOwnedByPlayerTwo != true && x.isDestroyed != true).Select(x => x.gridPosition).ToArray()))
                {
                    myUnit.positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 0.55f * Vector3.up);
                }
            }
            myUnit.gridPosition = destTile.gridPosition;
            // myUnit.waiting = true;
            /* if (myUnit.isFleaUpBatta
             {
                 myUnit.fleaActionPoints--;
             }

        }
        else
        {
            Debug.Log("destination invalid");
        }
        */
    




    //
    public void confirmTarget(NWTile destTile)
    {

    }
    //

    public void attackWithCurrentPlayer(NWTile destTile)
    {

        NWUnit attacker;
       


            attacker = myUnit;
       


        int unitBaseDamage = attacker.damageBase;

        int counterDamage = 0;

        if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassable)
        {


            NWUnit target = null;
            foreach (NWUnit p in units)
            {
                if (p.gridPosition == destTile.gridPosition)
                {
                    target = p;
                }
            }

            float distanceX = Mathf.Abs(attacker.gridPosition.x - target.gridPosition.x);
            float distanceY = Mathf.Abs(attacker.gridPosition.y - target.gridPosition.y);
            float totalDistance = distanceX + distanceY;

            // set base damage based on what the unit matchup is

            if (attacker is NWFlea || attacker is NWFleaPlusA ||  attacker is NWFleaPlusB)
            {
                if (target is NWFlea || target is NWFleaPlusA  || target is NWFleaPlusB)
                {
                    unitBaseDamage = 5;
                    counterDamage = 4;
                }
                else if (target is NWSpider )
                {
                    unitBaseDamage = 2;
                    counterDamage = 7;
                }
                else if (target is NWWitch || target is NWWitchPlusA || target is NWWitchPlusB )

                {
                    unitBaseDamage = 3;
                    counterDamage = 5;

                }
                else if (target is NWSpiderPlusA )
                {
                    unitBaseDamage = 2;
                    counterDamage = 9;
                }
                else if (target is NWSpiderPlusB )
                {
                    unitBaseDamage = 0;
                    counterDamage = 7;
                }
                else if (target is NWHQ)
                {
                    unitBaseDamage = 20;
                    counterDamage = 0;
                }
            }
            else
        if ((attacker is NWWitch || attacker is NWWitchPlusA || attacker is NWWitchPlusB )
                && ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerTwo)  || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerOne) 
                 ))
            {
                if (target is NWFlea || target is NWFleaPlusA ||  target is NWFleaPlusB)
                {
                    unitBaseDamage = 6;
                    // if not adjacent, -1 damage. 

                    counterDamage = 2;
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                        counterDamage = 0;
                    }

                }
                else if (target is NWSpider )
                {
                    unitBaseDamage = 5;
                    counterDamage = 5;
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                        counterDamage = 0;
                    }
                }
                else if (target is NWSpiderPlusA )
                {
                    unitBaseDamage = 5;
                    counterDamage = 7;
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                        counterDamage = 0;
                    }
                }
                else if (target is NWSpiderPlusB )
                {
                    unitBaseDamage = 3;
                    counterDamage = 5;
                    if (totalDistance > 2)
                    {
                        unitBaseDamage--;
                        counterDamage = 0;
                    }
                }
                else if (target is NWWitch || target is NWWitchPlusB )

                {
                    unitBaseDamage = 5;

                    counterDamage = 4;
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                        counterDamage--;
                    }
                    if (totalDistance > 2)
                    {
                        counterDamage = 0;
                    }

                }

                else if (target is NWWitchPlusA )
                {
                    unitBaseDamage = 5;
                    counterDamage = 4;
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                        counterDamage--;
                    }

                }

                else if (target is NWHQ)
                {
                    unitBaseDamage = 10;
                    // if not adjacent, -1 damage
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                    }
                    counterDamage = 0;
                }
            }
            else
            if
                // witch promote B is healing an ally
                ((attacker is NWWitchPlusB) && ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo) ))
            {
                unitBaseDamage = -2;
                counterDamage = -1;
            }
            else

                if
                (attacker is NWSpider )
            {
                if (target is NWFlea || target is NWFleaPlusA ||  target is NWFleaPlusB )

                {
                    unitBaseDamage = 8;
                    counterDamage = 1;
                }
                else if (target is NWSpider )
                {
                    unitBaseDamage = 5;
                    counterDamage = 4;
                }
                else if (target is NWSpiderPlusA )
                {
                    unitBaseDamage = 5;
                    counterDamage = 6;
                }
                else if (target is NWSpiderPlusB )
                {
                    unitBaseDamage = 3;
                    counterDamage = 4;
                }
                else if (target is NWWitch || target is NWWitchPlusA ||  target is NWWitchPlusB )

                {
                    unitBaseDamage = 6;
                    counterDamage = 4;

                }
                else if (target is NWHQ)
                {
                    unitBaseDamage = 15;
                    counterDamage = 0;
                }
            }
            else if (attacker is NWSpiderPlusA )
            {
                {
                    if (target is NWFlea || target is NWFleaPlusA || target is NWFleaPlusB )
                    {
                        unitBaseDamage = 10;
                        counterDamage = 1;
                    }
                    else if (target is NWSpider )
                    {
                        unitBaseDamage = 7;
                        counterDamage = 4;
                    }
                    else if (target is NWSpiderPlusA)
                    {
                        unitBaseDamage = 7;
                        counterDamage = 6;
                    }
                    else if (target is NWSpiderPlusB )
                    {
                        unitBaseDamage = 5;
                        counterDamage = 4;
                    }
                    else if (target is NWWitch || target is NWWitchPlusA  || target is NWWitchPlusB )

                    {
                        unitBaseDamage = 8;
                        counterDamage = 4;

                    }
                    else if (target is NWHQ)
                    {
                        unitBaseDamage = 17;
                        counterDamage = 0;
                    }
                }
            }
            else if (attacker is NWSpiderPlusB )
            {
                {
                    if (target is NWFlea || target is NWFleaPlusA ||  target is NWFleaPlusB )
                    {
                        unitBaseDamage = 8;
                        counterDamage = 0;
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage = 0;
                        }
                    }
                    else if (target is NWSpider )
                    {
                        unitBaseDamage = 5;
                        counterDamage = 2;
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage = 0;
                        }
                    }
                    else if (target is NWSpiderPlusA )
                    {
                        unitBaseDamage = 5;
                        counterDamage = 4;
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage = 0;
                        }
                    }
                    else if (target is NWSpiderPlusB )
                    {
                        unitBaseDamage = 3;
                        counterDamage = 2;

                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage--;
                        }
                    }
                    else if (target is NWWitch || target is NWWitchPlusA || target is NWWitchPlusB )

                    {
                        unitBaseDamage = 6;
                        counterDamage = 2;
                        // if not adjacent, -1 damage
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;

                            counterDamage--;
                        }
                    }
                    else if (target is NWHQ)
                    {
                        unitBaseDamage = 15;
                        counterDamage = 0;
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage--;
                        }
                    }
                }
            }



            if (target != null)
            {
                // below checks if you're adjacent; USE THIS FOR WITCH ATTACKS
                //if (attacker.gridPosition.x >= target.gridPosition.x - 1 && attacker.gridPosition.x <= target.gridPosition.x + 1
                //    && attacker.gridPosition.y >= target.gridPosition.y - 1 && attacker.gridPosition.y <= target.gridPosition.y + 1)


                // TODO: trying to get base damage stuff happening here
                /* public int getBaseDamageAgainst(Player attacker, Player defender)
                {

                    attacker = players[currentPlayerIndex];
                    defender = p;

                    return baseDamageAgainst;
                }
                //*/

                //  {
                //print("Attacker: "  + players[currentPlayerIndex].)


                attacker.waiting = true;
                if (attacker.isFleaUpB)
                {
                    attacker.fleaActionPoints--;
                    if (attacker.fleaActionPoints <= 1 && attacker.fleaMovementPoints == 2)
                    {
                        attacker.fleaMovementPoints--;
                    }
                    if (attacker.fleaActionPoints > 0)
                    {
                        attacker.waiting = false;
                    }
                }
                removeTileHighlights();
                attacker.attacking = false;



                int amountOfDamage = unitBaseDamage;

                // at certain hp levels, damage output decreases. not if you're targeting an ally though (that means you're healing them)

                if ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerTwo)  || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerOne)
                )
                {
                    if (attacker.HP < 6)
                    {
                        amountOfDamage = amountOfDamage - 1;
                        if (attacker.HP < 3)
                        {
                            amountOfDamage = amountOfDamage - 1;

                        }
                    }
                }
                // unless a healer is healing, negative damage cannot happen
                // there's an issue with this conditional that you need to fix
                /*if ((attacker.isHealer && ((attacker.isOwnedByAI && target.isOwnedByAI) 
                    || (attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo))
                    != true))*/

                if (amountOfDamage < 0)
                {
                    if (attacker.isHealer)
                    {
                        if ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) != true && (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo) != true)
                        {
                            amountOfDamage = 0;
                        }
                        else
                        {
                            amountOfDamage = -2;
                            counterDamage = -1;
                        }

                    }
                    else
                    {
                        amountOfDamage = 0;
                    }
                }


                target.HP -= amountOfDamage;
                if (target.HP <= 0)
                {
                    KillUnit(target);
                }
                /*
               else
                if (target.HP > 0 && target.HP < 10)
                {
                    HPDisplay.instance.UpdateHPIndicator(target.HP);
                }*/

                Debug.Log(attacker.playerName + " hit " + target.playerName + " but it feels good. " + amountOfDamage + " damage.");

                //promote unit if it killed the enemy
                if (target.HP <= 0 && (playerAICount > 0 && playerOneCount > 0))
                {

                    // currently this only works for player one
                    // if attacker has a certain amount of HP, promotes to A branch. if not, B branch
                    //Flea
                    if (attacker is NWFlea && attacker.HP > 5 && attacker.isOwnedByPlayerOne)
                    {
                        NWFleaPlusA playerFleaUpA;

                        playerFleaUpA = ((GameObject)Instantiate(FleaUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWFleaPlusA>();

                        playerFleaUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerFleaUpA.playerName = "Juicenjam";

                        playerFleaUpA.isOwnedByPlayerOne = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerFleaUpA);
                        playerOneCount++;

                        NWFleaPlusA.Promotion(playerFleaUpA, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                    else if (attacker is NWFlea && attacker.HP <= 5 && attacker.isOwnedByPlayerOne)
                    {
                        NWFleaPlusB playerFleaUpB;

                        playerFleaUpB = ((GameObject)Instantiate(FleaUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWFleaPlusB>();

                        playerFleaUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerFleaUpB.playerName = "Juicenjam";

                        playerFleaUpB.isOwnedByPlayerOne = true;
                        playerFleaUpB.isFleaUpB = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerFleaUpB);
                        playerOneCount++;
                        playerFleaUpB.waiting = false;


                        NWFleaPlusB.Promotion(playerFleaUpB, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                    else
                     if (attacker is NWFlea && attacker.HP > 5 && attacker.isOwnedByPlayerTwo)
                    {
                        NWFleaPlusA playerFleaUpA;

                        playerFleaUpA = ((GameObject)Instantiate(P2FleaUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWFleaPlusA>();

                        playerFleaUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerFleaUpA.playerName = "Juicenjam";

                        playerFleaUpA.isOwnedByPlayerTwo = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerFleaUpA);
                        playerTwoCount++;

                        NWFleaPlusA.Promotion(playerFleaUpA, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                    else if (attacker is NWFlea && attacker.HP <= 5 && attacker.isOwnedByPlayerTwo)
                    {
                        NWFleaPlusB playerFleaUpB;

                        playerFleaUpB = ((GameObject)Instantiate(P2FleaUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWFleaPlusB>();

                        playerFleaUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerFleaUpB.playerName = "Juicenjam";

                        playerFleaUpB.isOwnedByPlayerTwo = true;
                        playerFleaUpB.isFleaUpB = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerFleaUpB);
                        playerTwoCount++;
                        playerFleaUpB.waiting = false;


                        NWFleaPlusB.Promotion(playerFleaUpB, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }

                    //Spider

                    if (attacker is NWSpider && attacker.HP > 5 && attacker.isOwnedByPlayerOne)
                    {
                        NWSpiderPlusA playerSpiderUpA;

                        playerSpiderUpA = ((GameObject)Instantiate(SpiderUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWSpiderPlusA>();

                        playerSpiderUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerSpiderUpA.playerName = "Juicenjam";

                        playerSpiderUpA.isOwnedByPlayerOne = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerSpiderUpA);
                        playerOneCount++;

                        NWSpiderPlusA.Promotion(playerSpiderUpA, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                    else if (attacker is NWSpider && attacker.HP <= 5 && attacker.isOwnedByPlayerOne)
                    {
                        NWSpiderPlusB playerSpiderUpB;

                        playerSpiderUpB = ((GameObject)Instantiate(SpiderUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWSpiderPlusB>();

                        playerSpiderUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerSpiderUpB.playerName = "Juicenjam";

                        playerSpiderUpB.isOwnedByPlayerOne = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerSpiderUpB);
                        playerOneCount++;

                        NWSpiderPlusA.Promotion(playerSpiderUpB, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }

                    else
                     if (attacker is NWSpider && attacker.HP > 5 && attacker.isOwnedByPlayerTwo)
                    {
                        NWSpiderPlusA playerSpiderUpA;

                        playerSpiderUpA = ((GameObject)Instantiate(P2SpiderUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWSpiderPlusA>();

                        playerSpiderUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerSpiderUpA.playerName = "Juicenjam";

                        playerSpiderUpA.isOwnedByPlayerTwo = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerSpiderUpA);
                        playerTwoCount++;

                        NWSpiderPlusA.Promotion(playerSpiderUpA, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                    else if (attacker is NWSpider && attacker.HP <= 5 && attacker.isOwnedByPlayerTwo)
                    {
                        NWSpiderPlusB playerSpiderUpB;

                        playerSpiderUpB = ((GameObject)Instantiate(P2SpiderUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWSpiderPlusB>();

                        playerSpiderUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerSpiderUpB.playerName = "Juicenjam";

                        playerSpiderUpB.isOwnedByPlayerTwo = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerSpiderUpB);
                        playerTwoCount++;

                        NWSpiderPlusA.Promotion(playerSpiderUpB, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                  
                    //Witch
                    if (attacker is NWWitch && attacker.HP > 5 && attacker.isOwnedByPlayerOne)
                    {

                        NWWitchPlusA playerWitchUpA;

                        playerWitchUpA = ((GameObject)Instantiate(WitchUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWWitchPlusA>();

                        playerWitchUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerWitchUpA.playerName = "Juicenjam";

                        playerWitchUpA.isOwnedByPlayerOne = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerWitchUpA);
                        playerOneCount++;

                        NWWitchPlusA.Promotion(playerWitchUpA, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                    else if (attacker is NWWitch && attacker.HP <= 5 && attacker.isOwnedByPlayerOne)
                    {
                        NWWitchPlusB playerWitchUpB;

                        playerWitchUpB = ((GameObject)Instantiate(WitchUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWWitchPlusB>();

                        playerWitchUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerWitchUpB.playerName = "Juicenjam";

                        playerWitchUpB.isOwnedByPlayerOne = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerWitchUpB);
                        playerOneCount++;

                        NWWitchPlusB.Promotion(playerWitchUpB, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                    else
                  // AI-controlled witch
                  if (attacker is NWWitch && attacker.HP > 5 && attacker.isOwnedByPlayerTwo)
                    {

                        NWWitchPlusA playerWitchUpA;

                        playerWitchUpA = ((GameObject)Instantiate(P2WitchUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWWitchPlusA>();

                        playerWitchUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerWitchUpA.playerName = "Juicenjam";

                        playerWitchUpA.isOwnedByPlayerTwo = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerWitchUpA);
                        playerTwoCount++;

                        NWWitchPlusA.Promotion(playerWitchUpA, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                    else if (attacker is NWWitch && attacker.HP <= 5 && attacker.isOwnedByPlayerTwo)
                    {
                        NWWitchPlusB playerWitchUpB;

                        playerWitchUpB = ((GameObject)Instantiate(P2WitchUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWWitchPlusB>();

                        playerWitchUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                        playerWitchUpB.playerName = "Juicenjam";

                        playerWitchUpB.isOwnedByPlayerTwo = true;

                        //playerWitchUpA.HP = attacker.HP;


                        units.Add(playerWitchUpB);
                        playerTwoCount++;

                        NWWitchPlusB.Promotion(playerWitchUpB, attacker.HP);

                        KillUnit(attacker);
                        // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                        // is there a way to just have the unit transform 
                    }
                
                }

                //counterattack
                if (target.HP > 0)
                {

                    if (target.HP < 6)
                    {
                        counterDamage = counterDamage - 1;
                        if (target.HP < 3)
                        {
                            counterDamage = counterDamage - 1;
                        }
                    }



                    // unless a healer is healing, negative damage cannot happen
                    if ((counterDamage < 0) && (attacker is NWWitchPlusB) && ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo)) != true)

                    {
                        if (counterDamage < 0)
                        {
                            counterDamage = 0;
                        }
                    }
                    if ((counterDamage < 0) && (attacker is NWWitchPlusB) && ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo)) == true)
                    {
                        counterDamage = -1;
                    }


                    attacker.HP -= counterDamage;



                    Debug.Log(target.playerName + " countered for " + counterDamage);
                    /*
                                        if (attacker.HP > 0 && attacker.HP < 10)
                                        {
                                            HPDisplay.instance.UpdateHPForDisplay(attacker.HP);
                                        }else*/
                    // counterattacking units can also promote if they get a kill,
                    if (attacker.HP <= 0)
                    {

                        KillUnit(attacker);



                        {

                            // currently this only works for player one
                            // if attacker has a certain amount of HP, promotes to A branch. if not, B branch
                            //Flea
                            if (target is NWFlea && target.HP > 5)
                            {
                                NWFleaPlusA playerFleaUpA;

                                playerFleaUpA = ((GameObject)Instantiate(FleaUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWFleaPlusA>();

                                playerFleaUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerFleaUpA.playerName = "Juicenjam";

                                playerFleaUpA.isOwnedByPlayerOne = true;

                                units.Add(playerFleaUpA);
                                playerOneCount++;

                                NWFleaPlusA.Promotion(playerFleaUpA, target.HP);

                                KillUnit(target);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (target is NWFlea && target.HP <= 5)
                            {
                                NWFleaPlusB playerFleaUpB;

                                playerFleaUpB = ((GameObject)Instantiate(FleaUpBPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWFleaPlusB>();

                                playerFleaUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerFleaUpB.playerName = "Juicenjam";

                                playerFleaUpB.isOwnedByPlayerOne = true;

                                units.Add(playerFleaUpB);
                                playerOneCount++;

                                NWFleaPlusB.Promotion(playerFleaUpB, target.HP);

                                KillUnit(target);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }


                            //Spider

                            if (target is NWSpider && target.HP > 5)
                            {
                                NWSpiderPlusA playerSpiderUpA;

                                playerSpiderUpA = ((GameObject)Instantiate(SpiderUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWSpiderPlusA>();

                                playerSpiderUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerSpiderUpA.playerName = "Juicenjam";

                                playerSpiderUpA.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                units.Add(playerSpiderUpA);
                                playerOneCount++;

                                NWSpiderPlusA.Promotion(playerSpiderUpA, target.HP);

                                KillUnit(target);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (target is NWSpider && target.HP <= 5)
                            {
                                NWSpiderPlusB playerSpiderUpB;

                                playerSpiderUpB = ((GameObject)Instantiate(SpiderUpBPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWSpiderPlusB>();

                                playerSpiderUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerSpiderUpB.playerName = "Juicenjam";

                                playerSpiderUpB.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                units.Add(playerSpiderUpB);
                                playerOneCount++;

                                NWSpiderPlusA.Promotion(playerSpiderUpB, target.HP);

                                KillUnit(target);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                           

                            //Witch
                            if (target is NWWitch && target.HP > 5)
                            {

                                NWWitchPlusA playerWitchUpA;

                                playerWitchUpA = ((GameObject)Instantiate(WitchUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWWitchPlusA>();

                                playerWitchUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerWitchUpA.playerName = "Juicenjam";

                                playerWitchUpA.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                units.Add(playerWitchUpA);
                                playerOneCount++;

                                NWWitchPlusA.Promotion(playerWitchUpA, target.HP);

                                KillUnit(target);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (target is NWWitch && target.HP <= 5)
                            {
                                NWWitchPlusB playerWitchUpB;

                                playerWitchUpB = ((GameObject)Instantiate(WitchUpBPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWWitchPlusB>();

                                playerWitchUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerWitchUpB.playerName = "Juicenjam";

                                playerWitchUpB.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                units.Add(playerWitchUpB);
                                playerOneCount++;

                                NWWitchPlusB.Promotion(playerWitchUpB, target.HP);

                                KillUnit(target);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            // AI-controlled witch
                         
                        }
                    }
                    //

                }
                // }

            }
        }

        else
        {
            Debug.Log("target is invalid");
        }

        // can click units again
        foreach (NWUnit p in units)
        {
            p.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }




    //destroy units
    public static void KillUnit(NWUnit unit)
    {


        unit.isDestroyed = true;



        Destroy(unit.gameObject);

        // revise the list of units

        for (int i = 0; i < instance.units.Count; i++)
        {
            if (instance.units[i] == unit)
            {
                instance.units.RemoveAt(i);
            }


        }

        // revise list of AI units too, so that queue doesn't screw up
        for (int j = 0; j < instance.aiPlayers.Count; j++)
        {
            if (instance.aiPlayers[j] == unit)
            {
                instance.aiPlayers.RemoveAt(j);

                // i need to work this in somewhere
                if (instance.playerTwoTurn)
                {
                    instance.diedOnCounter = true;
                    instance.currentAIUnitIndex--;
                }

            }
        }

        // get rid of gridposition of the destroyed unit too please. maybe not necessary now thoughor only?
        //player.gridPosition = null;


        // there may be issues here with units being promoted right AFTER revising the list. so look into that. 
        // alternatively we could just have the promoted units take on the same reference number as the unit they're replacing or whatever. 
        // that wouldn't be too hard.  this current solution would still be required though, for when maps transfer over.

        // revise various 'unit counts' which may become redundant soon anyway but whatever
       

        if (unit.isOwnedByPlayerOne == true)
        {
            playerOneCount--;
        }
        else if (unit.isOwnedByPlayerTwo == true)
        {
            playerTwoCount--;
        }

    }




    public void generateMap()
    {
        loadMapFromXml();

        /*
        //TODO: maybe add something that generates a map anyway, if no xml file.
        if (currentCampaignMap == 1)
        {
            loadMapFromXml("versus1.xml");

        }
        else
            if (currentCampaignMap == 2)
        {
            GameObject YouWin;
            YouWin = (GameObject)Instantiate(WinScreenPrefab, new Vector3(0, 7, 0), Quaternion.Euler(new Vector3(90, 0, 0)));

            //PrefabHolder.instance.WinScreen);
            Destroy(YouWin, 2);
            loadMapFromXml("map2.xml");
        }
        else
            if (currentCampaignMap == 3)
        {
            GameObject YouWin;
            YouWin = (GameObject)Instantiate(WinScreenPrefab, new Vector3(0, 7, 0), Quaternion.Euler(new Vector3(90, 0, 0)));

            //PrefabHolder.instance.WinScreen);
            Destroy(YouWin, 2);
            loadMapFromXml("map3.xml");
        }
        else
            if (currentCampaignMap == 4)
        {
            GameObject YouWin;
            YouWin = (GameObject)Instantiate(WinScreenPrefab, new Vector3(0, 7, 0), Quaternion.Euler(new Vector3(90, 0, 0)));

            //PrefabHolder.instance.WinScreen);
            Destroy(YouWin, 2);
            loadMapFromXml("map4.xml");
        }
        else
            if (currentCampaignMap == 5)
        {
            GameObject YouWin;
            YouWin = (GameObject)Instantiate(WinScreenPrefab, new Vector3(0, 7, 0), Quaternion.Euler(new Vector3(90, 0, 0)));

            //PrefabHolder.instance.WinScreen);
            Destroy(YouWin, 2);
            loadMapFromXml("map5.xml");
        }
        else
            if (currentCampaignMap == 6)
        {
            GameObject YouWin;
            YouWin = (GameObject)Instantiate(WinScreenPrefab, new Vector3(0, 7, 0), Quaternion.Euler(new Vector3(90, 0, 0)));

            //PrefabHolder.instance.WinScreen);
            Destroy(YouWin, 2);
            loadMapFromXml("map6.xml");
        }
        */

        /* map = new List<List<Tile>>();
         for (int i = 0; i < mapSize; i++)
         {
             List<Tile> row = new List<Tile>();
             for (int j = 0; j < mapSize; j++)
             {
                 Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize / 2),0,-j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                 tile.gridPosition = new Vector2(i, j);
                 row.Add(tile);
             }
             map.Add(row);
         }
         */
    }

   // void loadMapFromXml(string s)
   void loadMapFromXml()
    {
        NWMapXmlContainer container = NWMapSaveLoad.Load("map.xml");

        mapSize = container.size;

        // initially remove all children
        for (int i = 0; i < mapTransform.transform.childCount; i++)
        {
            Destroy(mapTransform.GetChild(i).gameObject);
        }

        // also destroy all units

        for (int i = 0; i < units.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(units[i]);
            // }
        }
        // for some reason that leaves one unit, so for now, just, destroy it by destroying all units AGAIN:
        for (int i = 0; i < units.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(units[i]);
            // }
        }
        for (int i = 0; i < units.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(units[i]);
            // }
        }
        for (int i = 0; i < units.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(units[i]);
            // }
        }
        for (int i = 0; i < units.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(units[i]);
            // }
        }
        // don't ask me why i need ot murder them five fucking times

        // don't let this carry over between matches.
        // this is probably not necessary
        currentAIUnitIndex = 0;

        
        map = new List<List<NWTile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<NWTile> row = new List<NWTile>();
            for (int j = 0; j < mapSize; j++)
            {
                NWTile tile = ((GameObject)Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<NWTile>();
                tile.transform.parent = mapTransform;
                tile.gridPosition = new Vector2(i, j);
                tile.setType((TileType)container.tiles.Where(x => x.locX == i && x.locY == j).First().id);
                row.Add(tile);

                // add HQs on HQ tiles.  this is a misnomer atm, change cityplayerone to hq etc
                if (tile.type == TileType.HQP1)
                {
                    NWHQ playerHQ;
                    playerHQ = ((GameObject)Instantiate(HQPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3()))).GetComponent<NWHQ>();
                    playerHQ.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerHQ.playerName = "Juicenjam";

                    playerHQ.isOwnedByPlayerOne = true;

                    units.Add(playerHQ);
                    playerOneCount++;
                }
                else if (tile.type == TileType.HQP2)
                {
                    NWHQ playerTwoHQ;
                    playerTwoHQ = ((GameObject)Instantiate(P2HQPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3()))).GetComponent<NWHQ>();
                    playerTwoHQ.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerTwoHQ.playerName = "Juicenjam";

                    playerTwoHQ.isOwnedByPlayerTwo = true;

                    units.Add(playerTwoHQ);
                    playerTwoCount++;
                }
                else if (tile.type == TileType.FleaP1)
                {
                  
                    NWFlea playerFlea;
                    playerFlea = ((GameObject)Instantiate(FleaPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWFlea>();
                    playerFlea.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerFlea.playerName = "Juicenjam";

                    playerFlea.isOwnedByPlayerOne = true;

                    // ADD TO LIST

                    units.Add(playerFlea);
                    playerOneCount++;
                }
                else if (tile.type == TileType.FleaP2)
                {
                    NWFlea playerFlea;
                    playerFlea = ((GameObject)Instantiate(P2FleaPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWFlea>();
                    playerFlea.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerFlea.playerName = "Juicenjam";

                    playerFlea.isOwnedByPlayerTwo = true;

                    units.Add(playerFlea);
                    playerTwoCount++;
                }
                else if (tile.type == TileType.SpiderP1)
                {
                    NWSpider playerSpider;
                    playerSpider = ((GameObject)Instantiate(SpiderPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWSpider>();
                    playerSpider.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerSpider.playerName = "Juicenjam";

                    playerSpider.isOwnedByPlayerOne = true;

                    units.Add(playerSpider);
                    playerOneCount++;
                }
                else if (tile.type == TileType.SpiderP2)
                {
                    NWSpider playerSpider;
                    playerSpider = ((GameObject)Instantiate(P2SpiderPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWSpider>();
                    playerSpider.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerSpider.playerName = "Juicenjam";

                    playerSpider.isOwnedByPlayerTwo = true;

                    units.Add(playerSpider);
                    playerTwoCount++;
                }
                else if (tile.type == TileType.WitchP1)
                {
                    NWWitch playerWitch;
                    playerWitch = ((GameObject)Instantiate(WitchPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWWitch>();
                    playerWitch.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerWitch.playerName = "Juicenjam";

                    playerWitch.isOwnedByPlayerOne = true;

                    units.Add(playerWitch);
                    playerOneCount++;
                }
                else if (tile.type == TileType.WitchP2)
                {
                    NWWitch playerWitch;
                    playerWitch = ((GameObject)Instantiate(P2WitchPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<NWWitch>();
                    playerWitch.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerWitch.playerName = "Juicenjam";

                    playerWitch.isOwnedByPlayerTwo = true;

                    units.Add(playerWitch);
                    playerTwoCount++;
                }
            }
            map.Add(row);
        }
    }

    // use this to autogenerate stuff on particular single player maps
    void generatePlayers()
    {

        /*Flea playerFlea;
         playerFlea = ((GameObject)Instantiate(FleaPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), 0.55f, -0 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Flea>();
         playerFlea.gridPosition = new Vector2(0, 0);
         playerFlea.playerName = "Juicenjam";

         playerFlea.isOwnedByPlayerOne = true;

         players.Add(playerFlea);
         playerOneCount++;

         Witch playerWitch;
         playerWitch = ((GameObject)Instantiate(WitchPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), 0.55f, -(mapSize - 2) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Witch>();
         playerWitch.gridPosition = new Vector2(mapSize - 1, mapSize - 2);
         playerWitch.playerName = "CalforniaRazin";

         playerWitch.isOwnedByPlayerOne = true;

         players.Add(playerWitch);
         playerOneCount++;


         Flea playerTwoFlea = ((GameObject)Instantiate(PlayerTwoFleaPrefab, new Vector3((mapSize - 4) - Mathf.Floor(mapSize / 2), 0.55f, -(mapSize - 4) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Flea>();
         playerTwoFlea.gridPosition = new Vector2(mapSize - 4, mapSize - 4);
         playerTwoFlea.playerName = "death";

         playerTwoFlea.isOwnedByPlayerTwo = true;

         players.Add(playerTwoFlea);
         playerTwoCount++;


         Witch playerTwoWitch = ((GameObject)Instantiate(PlayerTwoWitchPrefab, new Vector3((mapSize - 2) - Mathf.Floor(mapSize / 2), 0.55f, -(mapSize - 2) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Witch>();
         playerTwoWitch.gridPosition = new Vector2(mapSize - 2, mapSize - 2);
         playerTwoWitch.playerName = "filth";

         playerTwoWitch.isOwnedByPlayerTwo = true;

         players.Add(playerTwoWitch);
         playerTwoCount++;
         */



        // YOU WANT THIS DONT YOU
        /*
         AIPlayerFix aiFlea = ((GameObject)Instantiate(AIFleaPrefab, new Vector3((mapSize - 2) - Mathf.Floor(mapSize / 2), 0.55f, -(mapSize - 2) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayerFix>();
         aiFlea.gridPosition = new Vector2(mapSize - 2, mapSize - 2);
         aiFlea.playerName = "compydomp";

        
        aiFlea.isOwnedByPlayerTwo = true;
        aiFlea.isOwnedByAI = true;

         players.Add(aiFlea);
         aiPlayers.Add(aiFlea);
        playerTwoCount++;
        numberOfActiveAIUnits++;


        aiFlea = ((GameObject)Instantiate(AIFleaPrefab, new Vector3((mapSize - 3) - Mathf.Floor(mapSize / 2), 0.55f, -(mapSize - 3) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayerFix>();
        aiFlea.gridPosition = new Vector2(mapSize - 3, mapSize - 3);
        aiFlea.playerName = "compydomp";

        aiFlea.isOwnedByPlayerTwo = true;
        aiFlea.isOwnedByAI = true;

        players.Add(aiFlea);
        aiPlayers.Add(aiFlea);
        playerTwoCount++;
        numberOfActiveAIUnits++;

        /*

         aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(7 - Mathf.Floor(mapSize / 2), 1.5f, -7 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
         aiplayer.gridPosition = new Vector2(7, 7);
         aiplayer.playerName = "roboblast";
         player.chestArmor = Armor.FromKey(ArmorKey.IronPlate);
         player.handWeapons.Add(Weapon.FromKey(WeaponKey.WarHammer));

         players.Add(aiplayer);


         aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(1 - Mathf.Floor(mapSize / 2), 1.5f, -7 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
         aiplayer.gridPosition = new Vector2(1, 7);
         aiplayer.playerName = "mexican";
         player.chestArmor = Armor.FromKey(ArmorKey.IronPlate);
         player.handWeapons.Add(Weapon.FromKey(WeaponKey.LongSword));

         players.Add(aiplayer);


         aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(2 - Mathf.Floor(mapSize / 2), 1.5f, -3 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
         aiplayer.gridPosition = new Vector2(2, 3);
         aiplayer.playerName = "bombuterp";
         player.chestArmor = Armor.FromKey(ArmorKey.IronPlate);
         player.handWeapons.Add(Weapon.FromKey(WeaponKey.LongBow));

         players.Add(aiplayer);*/

    }


    public void BuildUnitsGUI()
    {
        Rect rect = new Rect(10, Screen.height - 80, 100, 20);
        if (GUI.Button(rect, "Flea"))
        {
            // if it's player one's turn, and they can afford it, spawn a player one infantry.
            if (playerOneTurn && fundsArmyOne >= NWFlea.cost)
            {
                NWFlea newFlea;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newFlea = ((GameObject)Instantiate(FleaPrefab, new Vector3(NWTile.myTile.transform.position.x, 0.55f, NWTile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<NWFlea>();
                newFlea.gridPosition = new Vector2(NWTile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -NWTile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newFlea.playerName = "Juicenjam";
                newFlea.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newFlea.isOwnedByPlayerOne = true;

                units.Add(newFlea);
                playerOneCount++;
                fundsArmyOne -= NWFlea.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
            // if player two, spawn player two infantry
            else if (playerTwoTurn && fundsArmyTwo >= NWFlea.cost)
            {
                NWFlea newFlea;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newFlea = ((GameObject)Instantiate(P2FleaPrefab, new Vector3(NWTile.myTile.transform.position.x, 0.55f, NWTile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<NWFlea>();
                newFlea.gridPosition = new Vector2(NWTile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -NWTile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newFlea.playerName = "Juicenjam";
                newFlea.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newFlea.isOwnedByPlayerTwo = true;

                units.Add(newFlea);
                playerTwoCount++;
                fundsArmyTwo -= NWFlea.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
        }


        //mech
        rect = new Rect(10, Screen.height - 110, 100, 20);
        if (GUI.Button(rect, "Spider"))
        {
            // if it's player one's turn, and they can afford it, spawn a player one infantry.
            if (playerOneTurn && fundsArmyOne >= NWSpider.cost)
            {
                NWSpider newSpider;

                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newSpider = ((GameObject)Instantiate(SpiderPrefab, new Vector3(NWTile.myTile.transform.position.x, 0.55f, NWTile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<NWSpider>();
                newSpider.gridPosition = new Vector2(NWTile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -NWTile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newSpider.playerName = "Juicenjam";
                newSpider.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newSpider.isOwnedByPlayerOne = true;

                units.Add(newSpider);
                playerOneCount++;
                fundsArmyOne -= NWSpider.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
            // if player two, spawn for player two
            else if (playerTwoTurn && fundsArmyTwo >= NWSpider.cost)
            {
                NWSpider newSpider;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newSpider = ((GameObject)Instantiate(P2SpiderPrefab, new Vector3(NWTile.myTile.transform.position.x, 0.55f, NWTile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<NWSpider>();
                newSpider.gridPosition = new Vector2(NWTile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -NWTile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newSpider.playerName = "Juicenjam";
                newSpider.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newSpider.isOwnedByPlayerTwo = true;

                units.Add(newSpider);
                playerTwoCount++;
                fundsArmyTwo -= NWSpider.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
        }

        //bike
        rect = new Rect(10, Screen.height - 140, 100, 20);
        if (GUI.Button(rect, "Witch"))
        {
            // if it's player one's turn, and they can afford it, spawn a player one infantry.
            if (playerOneTurn && fundsArmyOne >= NWWitch.cost)
            {
                NWWitch newWitch;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newWitch = ((GameObject)Instantiate(WitchPrefab, new Vector3(NWTile.myTile.transform.position.x, 0.55f, NWTile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<NWWitch>();
                newWitch.gridPosition = new Vector2(NWTile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -NWTile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newWitch.playerName = "Juicenjam";
                newWitch.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newWitch.isOwnedByPlayerOne = true;

                units.Add(newWitch);
                playerOneCount++;
                fundsArmyOne -= NWWitch.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
            // if player two, spawn player two infantry
            else if (playerTwoTurn && fundsArmyTwo >= NWWitch.cost)
            {
                NWWitch newWitch;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newWitch = ((GameObject)Instantiate(P2WitchPrefab, new Vector3(NWTile.myTile.transform.position.x, 0.55f, NWTile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<NWWitch>();
                newWitch.gridPosition = new Vector2(NWTile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -NWTile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newWitch.playerName = "Juicenjam";
                newWitch.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newWitch.isOwnedByPlayerTwo = true;

                units.Add(newWitch);
                playerTwoCount++;
                fundsArmyTwo -= NWWitch.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }


        }
    }
}
