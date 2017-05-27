using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public int currentCampaignMap = 1;

    public static UserPlayer myUnit;

    public int turnCount = 1;

    public static GameManager instance;

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

    public CombatUI ButtonCanvas;
    public CombatUI BuildUnitCanvas;


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

    public int mapSize = 7;

    // Control AI unit delay
    public bool resumeIn3Seconds = false;
    public bool startedPausing = false;

    public List<List<Tile>> map = new List<List<Tile>>();
    public List<Player> players = new List<Player>();

    // list of ai players
    public List<Player> aiPlayers = new List<Player>();
    public int currentAIUnitIndex = 0;
    public int numberOfActiveAIUnits = 0;

    public int currentPlayerIndex = 0;
    public int currentArmyIndex = 0;

    // total number of each faction's units
    public static int playerOneCount = 0;
    public static int playerTwoCount = 0;
    public static int playerAICount = 0;

    // keep track of whose turn it is
    public bool playerOneTurn = true;
    public bool playerTwoTurn = false;

    // ehhhhhhhhhhhh
    public bool firstTurn = true;
    public bool diedOnCounter = false;
    public bool unitIsDead = false;
    public bool myUnitIsBeingUsed = false;
    public bool myUnitMustWaitOrAttack = false;

    public bool aiPlayerTurn = false;

    public int fundsArmyOne;
    public int fundsArmyTwo;

    void Awake()
    {
        instance = this;
        mapTransform = transform.FindChild("Map");
    }

    void Start()
    {
        generateMap();
        generatePlayers();

        // first player gets half the funds on the first day to counteract first turn advantage
        fundsArmyOne = (numberOfPropsArmyOne * 1000) / 2;
        fundsArmyTwo = 0;
        gameObject.GetComponent<FundsP1>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (myUnit == null)
        {
           // print("no unit chosen yet");
        }
        else
        if (myUnit.HP > 0)
        {
            myUnit.TurnUpdate();
        }
          
        //TODO: make this work for specific armies
        if (playerOneCount <= 0)
        {
            //print("PLAYER TWO WINS MY DUDE");
                generateMap();
        }
        if (playerTwoCount <= 0)
        {
            print("next level please");
            //
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
            print("next level please");
                        
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
        }
    }

    // AI pauses briefly between moving each unit
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
        List<Tile> highlightedTiles = new List<Tile>();
        if (ignorePlayers)
        {
            highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance, highlightColor == Color.red || highlightColor == Color.cyan || highlightColor == Color.magenta);
        }
        else if (playerOneTurn)
        {
            highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance, players.Where(x => x.gridPosition != originLocation && x.isOwnedByPlayerOne == false && x.isDestroyed == false).Select(x => x.gridPosition).ToArray(), highlightColor == Color.red);
        }
        else if (playerTwoTurn)
        {
            highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance, players.Where(x => x.gridPosition != originLocation && x.isOwnedByPlayerTwo == false && x.isDestroyed == false).Select(x => x.gridPosition).ToArray(), highlightColor == Color.red);
        }

        if (invisibleHighlights == false)
        {
            foreach (Tile t in highlightedTiles)
            {
                t.visual.transform.GetComponent<Renderer>().materials[0].color = highlightColor;
                highlights = (GameObject)Instantiate(PrefabHolder.instance.HIGHLIGHT_TILE, new Vector3((int)t.gridPosition.x - Mathf.Floor(mapSize / 2), 0.55f, -(int)t.gridPosition.y + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3(0, 0, 180)));
                highlightTiles.Add(highlights);
            }
        }
        else
        {
            foreach (Tile t in highlightedTiles)
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

    public void DeactivateAButton()
    {
        //units and tiles are clickable again
        foreach (Player u in GameManager.instance.players)
        {
            u.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        for (int i = 0; i < GameManager.instance.mapSize; i++)
        {
            for (int j = 0; j < GameManager.instance.mapSize; j++)
            {
                GameManager.instance.map[i][j].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
        } 
}
   
    public void ReactivateAButton()
    {
        //units and tiles are clickable again
        foreach (Player u in players)
        {
            u.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                map[i][j].gameObject.layer = LayerMask.NameToLayer("Default");
            }
        } 
}

    void OnGUI()
    {   
        if (players[currentPlayerIndex].HP > 0)
        {
            players[currentPlayerIndex].TurnOnGUI();
        }
    }

    public void NextTurn()
    {
        // ends turn for whichever player is active when they press the end turn button
        if (playerOneTurn)
        {
            playerOneTurn = false;
            playerTwoTurn = true;
            int playerTwoIncome = (numberOfPropsArmyTwo * 1000);
            fundsArmyTwo += playerTwoIncome;

            foreach (Player p in GameManager.instance.players)
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

            foreach (Player p in GameManager.instance.players)
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
    }

    public void moveCurrentPlayer(Tile destTile)
    {
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
            }
            else
            {
                Debug.Log("destination invalid");
            }
        }
    
    public void confirmTarget(Tile destTile)
    {

    }

    public void attackWithCurrentPlayer(Tile destTile)
    {
        ButtonCanvas.transform.position = new Vector3(-20, 0, 0);

        Player attacker;
        if (aiPlayerTurn)
        {
            attacker = aiPlayers[currentAIUnitIndex];
        }
        else
        {
            attacker = myUnit;
        }

        int unitBaseDamage = attacker.damageBase;
        int counterDamage = 0;
        if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassable)
        {
            Player target = null;
            foreach (Player p in players)
            {
                if (p.gridPosition == destTile.gridPosition)
                {
                    target = p;
                }
            }
            float distanceX = Mathf.Abs(attacker.gridPosition.x - target.gridPosition.x);
            float distanceY = Mathf.Abs(attacker.gridPosition.y - target.gridPosition.y);
            float totalDistance = distanceX + distanceY;

            // set base damage for each unit matchup
            if (attacker is Flea || attacker is FleaAI || attacker is FleaPlusA || attacker is FleaPlusAAI || attacker is FleaPlusB || attacker is FleaPlusBAI)
            {
                if (target is Flea || target is FleaAI || target is FleaPlusA || target is FleaPlusAAI || target is FleaPlusB || target is FleaPlusBAI)
                {
                    unitBaseDamage = 5;
                    counterDamage = 4;
                }
                else if (target is Spider || target is SpiderAI)
                {
                    unitBaseDamage = 2;
                    counterDamage = 7;
                }
                else if (target is Witch || target is WitchAI || target is WitchPlusA || target is WitchPlusAAI || target is WitchPlusB || target is WitchPlusBAI)
                {
                    unitBaseDamage = 3;
                    counterDamage = 5;
                }
                else if (target is SpiderPlusA || target is SpiderPlusAAI)
                {
                    unitBaseDamage = 2;
                    counterDamage = 9;
                }
                else if (target is SpiderPlusB || target is SpiderPlusBAI)
                {
                    unitBaseDamage = 0;
                    counterDamage = 7;
                }
                else if (target is HQ)
                {
                    unitBaseDamage = 20;
                    counterDamage = 0;
                }
            }
            else
        if ((attacker is Witch || attacker is WitchAI || attacker is WitchPlusA || attacker is WitchPlusAAI || attacker is WitchPlusB || attacker is WitchPlusBAI)
                && ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerTwo) || (attacker.isOwnedByPlayerOne && target.isOwnedByAI) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByAI)
                || (attacker.isOwnedByAI && target.isOwnedByPlayerOne) || (attacker.isOwnedByAI && target.isOwnedByPlayerTwo)))
            {
                if (target is Flea || target is FleaAI || target is FleaPlusA || target is FleaPlusAAI || target is FleaPlusB || target is FleaPlusBAI)
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
                else if (target is Spider || target is SpiderAI)
                {
                    unitBaseDamage = 5;
                    counterDamage = 5;
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                        counterDamage = 0;
                    }
                }
                else if (target is SpiderPlusA || target is SpiderPlusAAI)
                {
                    unitBaseDamage = 5;
                    counterDamage = 7;
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                        counterDamage = 0;
                    }
                }
                else if (target is SpiderPlusB || target is SpiderPlusBAI)
                {
                    unitBaseDamage = 3;
                    counterDamage = 5;
                    if (totalDistance > 2)
                    {
                        unitBaseDamage--;
                        counterDamage = 0;
                    }
                }
                else if (target is Witch || target is WitchAI || target is WitchPlusB || target is WitchPlusBAI)
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
                else if (target is WitchPlusA || target is WitchPlusAAI)
                {
                    unitBaseDamage = 5;
                    counterDamage = 4;
                    if (totalDistance > 1)
                    {
                        unitBaseDamage--;
                        counterDamage--;
                    }
                }
                else if (target is HQ)
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
                ((attacker is WitchPlusB || attacker is WitchPlusBAI) && ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo) || (attacker.isOwnedByAI && target.isOwnedByAI)))
            {
                unitBaseDamage = -2;
                counterDamage = -1;
            }
            else
                if(attacker is Spider || attacker is SpiderAI)
            { 
                if (target is Flea || target is FleaAI || target is FleaPlusA || target is FleaPlusAAI || target is FleaPlusB || target is FleaPlusBAI)
                {
                    unitBaseDamage = 8;
                    counterDamage = 1;
                }
                else if (target is Spider || target is SpiderAI)
                {
                    unitBaseDamage = 5;
                    counterDamage = 4;
                }
                else if (target is SpiderPlusA || target is SpiderPlusAAI)
                {
                    unitBaseDamage = 5;
                    counterDamage = 6;
                }
                else if (target is SpiderPlusB || target is SpiderPlusBAI)
                {
                    unitBaseDamage = 3;
                    counterDamage = 4;
                }
                else if (target is Witch || target is WitchAI || target is WitchPlusA || target is WitchPlusAAI || target is WitchPlusB || target is WitchPlusBAI)
                {
                    unitBaseDamage = 6;
                    counterDamage = 4;
                }
                else if (target is HQ)
                {
                    unitBaseDamage = 15;
                    counterDamage = 0;
                }
            }
            else if (attacker is SpiderPlusA || attacker is SpiderPlusAAI)
            {
                {
                    if (target is Flea || target is FleaAI || target is FleaPlusA || target is FleaPlusAAI || target is FleaPlusB || target is FleaPlusBAI)
                    {
                        unitBaseDamage = 10;
                        counterDamage = 1;
                    }
                    else if (target is Spider || target is SpiderAI)
                    {
                        unitBaseDamage = 7;
                        counterDamage = 4;
                    }
                    else if (target is SpiderPlusA || target is SpiderPlusAAI)
                    {
                        unitBaseDamage = 7;
                        counterDamage = 6;
                    }
                    else if (target is SpiderPlusB || target is SpiderPlusBAI)
                    {
                        unitBaseDamage = 5;
                        counterDamage = 4;
                    }
                    else if (target is Witch || target is WitchAI || target is WitchPlusA || target is WitchPlusAAI || target is WitchPlusB || target is WitchPlusBAI)
                    {
                        unitBaseDamage = 8;
                        counterDamage = 4;
                    }
                    else if (target is HQ)
                    {
                        unitBaseDamage = 17;
                        counterDamage = 0;
                    }
                }
            }
            else if (attacker is SpiderPlusB || attacker is SpiderPlusBAI)
            {
                {
                    if (target is Flea || target is FleaAI || target is FleaPlusA || target is FleaPlusAAI || target is FleaPlusB || target is FleaPlusBAI)
                    {
                        unitBaseDamage = 8;
                        counterDamage = 0;
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage = 0;
                        }
                    }
                    else if (target is Spider || target is SpiderAI)
                    {
                        unitBaseDamage = 5;
                        counterDamage = 2;
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage = 0;
                        }
                    }
                    else if (target is SpiderPlusA || target is SpiderPlusAAI)
                    {
                        unitBaseDamage = 5;
                        counterDamage = 4;
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage = 0;
                        }
                    }
                    else if (target is SpiderPlusB || target is SpiderPlusBAI)
                    {
                        unitBaseDamage = 3;
                        counterDamage = 2;
                        if (totalDistance > 1)
                        {
                            unitBaseDamage--;
                            counterDamage--;
                        }
                    }
                    else if (target is Witch || target is WitchAI || target is WitchPlusA || target is WitchPlusAAI || target is WitchPlusB || target is WitchPlusBAI)
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
                    else if (target is HQ)
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
               
                // TODO: trying to get base damage stuff happening here
                /* public int getBaseDamageAgainst(Player attacker, Player defender)
                {

                    attacker = players[currentPlayerIndex];
                    defender = p;

                    return baseDamageAgainst;
                }
                //*/

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
                if ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerTwo) || (attacker.isOwnedByPlayerOne && target.isOwnedByAI) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByAI)
                 || (attacker.isOwnedByAI && target.isOwnedByPlayerOne) || (attacker.isOwnedByAI && target.isOwnedByPlayerTwo))
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
                        if ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) != true && (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo) != true && (attacker.isOwnedByAI && target.isOwnedByAI) != true)
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

                        /*
                       else
                        if (target.HP > 0 && target.HP < 10)
                        {
                            HPDisplay.instance.UpdateHPIndicator(target.HP);
                        }*/

                        Debug.Log(attacker.playerName + " hit " + target.playerName + " but it feels good. " + amountOfDamage + " damage.");

                        //promote unit if it killed the enemy
                        if ((playerAICount > 0 && playerOneCount > 0) || (playerOneCount > 0 && playerTwoCount > 0))
                        {

                            // currently this only works for player one
                            // if attacker has a certain amount of HP, promotes to A branch. if not, B branch
                            //Flea
                            if (attacker is Flea && attacker.HP > 5 && attacker.isOwnedByPlayerOne)
                            {
                                FleaPlusA playerFleaUpA;

                                playerFleaUpA = ((GameObject)Instantiate(FleaUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusA>();

                                playerFleaUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerFleaUpA.playerName = "Juicenjam";

                                playerFleaUpA.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerFleaUpA);
                                playerOneCount++;

                                FleaPlusA.Promotion(playerFleaUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (attacker is Flea && attacker.HP <= 5 && attacker.isOwnedByPlayerOne)
                            {
                                FleaPlusB playerFleaUpB;

                                playerFleaUpB = ((GameObject)Instantiate(FleaUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusB>();

                                playerFleaUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerFleaUpB.playerName = "Juicenjam";

                                playerFleaUpB.isOwnedByPlayerOne = true;
                                playerFleaUpB.isFleaUpB = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerFleaUpB);
                                playerOneCount++;
                                playerFleaUpB.waiting = false;


                                FleaPlusB.Promotion(playerFleaUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else
                             if (attacker is Flea && attacker.HP > 5 && attacker.isOwnedByPlayerTwo)
                            {
                                FleaPlusA playerFleaUpA;

                                playerFleaUpA = ((GameObject)Instantiate(P2FleaUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusA>();

                                playerFleaUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerFleaUpA.playerName = "Juicenjam";

                                playerFleaUpA.isOwnedByPlayerTwo = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerFleaUpA);
                                playerTwoCount++;

                                FleaPlusA.Promotion(playerFleaUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (attacker is Flea && attacker.HP <= 5 && attacker.isOwnedByPlayerTwo)
                            {
                                FleaPlusB playerFleaUpB;

                                playerFleaUpB = ((GameObject)Instantiate(P2FleaUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusB>();

                                playerFleaUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerFleaUpB.playerName = "Juicenjam";

                                playerFleaUpB.isOwnedByPlayerTwo = true;
                                playerFleaUpB.isFleaUpB = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerFleaUpB);
                                playerTwoCount++;
                                playerFleaUpB.waiting = false;


                                FleaPlusB.Promotion(playerFleaUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            // AI-controlled Flea
                            if (attacker is FleaAI && attacker.HP > 5)
                            {
                                FleaPlusAAI AIFleaUpA;

                                AIFleaUpA = ((GameObject)Instantiate(AIFleaUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusAAI>();

                                AIFleaUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                AIFleaUpA.playerName = "Juicenjam";

                                AIFleaUpA.isOwnedByAI = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(AIFleaUpA);
                                aiPlayers.Add(AIFleaUpA);
                                numberOfActiveAIUnits++;
                                playerAICount++;
                                playerTwoCount++;


                                FleaPlusAAI.Promotion(AIFleaUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (attacker is FleaAI && attacker.HP <= 5)
                            {
                                FleaPlusBAI AIFleaUpB;

                                AIFleaUpB = ((GameObject)Instantiate(AIFleaUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusBAI>();

                                AIFleaUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                AIFleaUpB.playerName = "Juicenjam";

                                AIFleaUpB.waiting = false;
                                AIFleaUpB.isOwnedByAI = true;
                                AIFleaUpB.isFleaUpB = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(AIFleaUpB);
                                aiPlayers.Add(AIFleaUpB);
                                playerAICount++;
                                numberOfActiveAIUnits++;
                                playerTwoCount++;


                                FleaPlusBAI.Promotion(AIFleaUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }


                            //Spider

                            if (attacker is Spider && attacker.HP > 5 && attacker.isOwnedByPlayerOne)
                            {
                                SpiderPlusA playerSpiderUpA;

                                playerSpiderUpA = ((GameObject)Instantiate(SpiderUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusA>();

                                playerSpiderUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerSpiderUpA.playerName = "Juicenjam";

                                playerSpiderUpA.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerSpiderUpA);
                                playerOneCount++;

                                SpiderPlusA.Promotion(playerSpiderUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (attacker is Spider && attacker.HP <= 5 && attacker.isOwnedByPlayerOne)
                            {
                                SpiderPlusB playerSpiderUpB;

                                playerSpiderUpB = ((GameObject)Instantiate(SpiderUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusB>();

                                playerSpiderUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerSpiderUpB.playerName = "Juicenjam";

                                playerSpiderUpB.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerSpiderUpB);
                                playerOneCount++;

                                SpiderPlusA.Promotion(playerSpiderUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }

                            else
                             if (attacker is Spider && attacker.HP > 5 && attacker.isOwnedByPlayerTwo)
                            {
                                SpiderPlusA playerSpiderUpA;

                                playerSpiderUpA = ((GameObject)Instantiate(P2SpiderUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusA>();

                                playerSpiderUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerSpiderUpA.playerName = "Juicenjam";

                                playerSpiderUpA.isOwnedByPlayerTwo = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerSpiderUpA);
                                playerTwoCount++;

                                SpiderPlusA.Promotion(playerSpiderUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (attacker is Spider && attacker.HP <= 5 && attacker.isOwnedByPlayerTwo)
                            {
                                SpiderPlusB playerSpiderUpB;

                                playerSpiderUpB = ((GameObject)Instantiate(P2SpiderUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusB>();

                                playerSpiderUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerSpiderUpB.playerName = "Juicenjam";

                                playerSpiderUpB.isOwnedByPlayerTwo = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerSpiderUpB);
                                playerTwoCount++;

                                SpiderPlusA.Promotion(playerSpiderUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            // AI controlled spider

                            if (attacker is SpiderAI && attacker.HP > 5)
                            {
                                SpiderPlusAAI AISpiderUpA;

                                AISpiderUpA = ((GameObject)Instantiate(AISpiderUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusAAI>();

                                AISpiderUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                AISpiderUpA.playerName = "Juicenjam";

                                AISpiderUpA.isOwnedByAI = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(AISpiderUpA);
                                aiPlayers.Add(AISpiderUpA);
                                playerAICount++;
                                numberOfActiveAIUnits++;
                                playerTwoCount++;


                                SpiderPlusAAI.Promotion(AISpiderUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else
                            if (attacker is SpiderAI && attacker.HP <= 5)
                            {
                                SpiderPlusBAI AISpiderUpB;

                                AISpiderUpB = ((GameObject)Instantiate(AISpiderUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusBAI>();

                                AISpiderUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                AISpiderUpB.playerName = "Juicenjam";

                                AISpiderUpB.isOwnedByAI = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(AISpiderUpB);
                                aiPlayers.Add(AISpiderUpB);
                                playerAICount++;
                                numberOfActiveAIUnits++;
                                playerTwoCount++;


                                SpiderPlusBAI.Promotion(AISpiderUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }

                            //Witch
                            if (attacker is Witch && attacker.HP > 5 && attacker.isOwnedByPlayerOne)
                            {

                                WitchPlusA playerWitchUpA;

                                playerWitchUpA = ((GameObject)Instantiate(WitchUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusA>();

                                playerWitchUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerWitchUpA.playerName = "Juicenjam";

                                playerWitchUpA.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerWitchUpA);
                                playerOneCount++;

                                WitchPlusA.Promotion(playerWitchUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (attacker is Witch && attacker.HP <= 5 && attacker.isOwnedByPlayerOne)
                            {
                                WitchPlusB playerWitchUpB;

                                playerWitchUpB = ((GameObject)Instantiate(WitchUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusB>();

                                playerWitchUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerWitchUpB.playerName = "Juicenjam";

                                playerWitchUpB.isOwnedByPlayerOne = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerWitchUpB);
                                playerOneCount++;

                                WitchPlusB.Promotion(playerWitchUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else
                          // AI-controlled witch
                          if (attacker is Witch && attacker.HP > 5 && attacker.isOwnedByPlayerTwo)
                            {

                                WitchPlusA playerWitchUpA;

                                playerWitchUpA = ((GameObject)Instantiate(P2WitchUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusA>();

                                playerWitchUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerWitchUpA.playerName = "Juicenjam";

                                playerWitchUpA.isOwnedByPlayerTwo = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerWitchUpA);
                                playerTwoCount++;

                                WitchPlusA.Promotion(playerWitchUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else if (attacker is Witch && attacker.HP <= 5 && attacker.isOwnedByPlayerTwo)
                            {
                                WitchPlusB playerWitchUpB;

                                playerWitchUpB = ((GameObject)Instantiate(P2WitchUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusB>();

                                playerWitchUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                playerWitchUpB.playerName = "Juicenjam";

                                playerWitchUpB.isOwnedByPlayerTwo = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(playerWitchUpB);
                                playerTwoCount++;

                                WitchPlusB.Promotion(playerWitchUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            if (attacker is WitchAI && attacker.HP > 5)
                            {
                                WitchPlusAAI AIWitchUpA;

                                AIWitchUpA = ((GameObject)Instantiate(AIWitchUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusAAI>();

                                AIWitchUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                AIWitchUpA.playerName = "Juicenjam";

                                AIWitchUpA.isOwnedByAI = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(AIWitchUpA);
                                aiPlayers.Add(AIWitchUpA);
                                numberOfActiveAIUnits++;
                                playerAICount++;

                                WitchPlusAAI.Promotion(AIWitchUpA, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
                            else
                            if (attacker is WitchAI && attacker.HP <= 5)
                            {
                                WitchPlusBAI AIWitchUpB;

                                AIWitchUpB = ((GameObject)Instantiate(AIWitchUpBPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusBAI>();

                                AIWitchUpB.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
                                AIWitchUpB.playerName = "Juicenjam";

                                AIWitchUpB.isOwnedByAI = true;

                                //playerWitchUpA.HP = attacker.HP;


                                players.Add(AIWitchUpB);
                                aiPlayers.Add(AIWitchUpB);
                                numberOfActiveAIUnits++;
                                playerAICount++;
                                playerTwoCount++;


                                WitchPlusBAI.Promotion(AIWitchUpB, attacker.HP);

                                KillUnit(attacker);
                                // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                // is there a way to just have the unit transform 
                            }
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
                    if ((counterDamage < 0) && (attacker is WitchPlusB || attacker is WitchPlusBAI) && ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo) || (attacker.isOwnedByAI && target.isOwnedByAI)) != true)

                    {
                        if (counterDamage < 0)
                        {
                            counterDamage = 0;
                        }
                    }
                    if ((counterDamage < 0) && (attacker is WitchPlusB || attacker is WitchPlusBAI) && ((attacker.isOwnedByPlayerOne && target.isOwnedByPlayerOne) || (attacker.isOwnedByPlayerTwo && target.isOwnedByPlayerTwo) || (attacker.isOwnedByAI && target.isOwnedByAI)) == true)
                    {
                        counterDamage = -1;
                    }


                    attacker.HP -= counterDamage;


                // maybe bracket here
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
                                if (target is Flea && target.HP > 5)
                                {
                                    FleaPlusA playerFleaUpA;

                                    playerFleaUpA = ((GameObject)Instantiate(FleaUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusA>();

                                    playerFleaUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    playerFleaUpA.playerName = "Juicenjam";

                                    playerFleaUpA.isOwnedByPlayerOne = true;

                                    players.Add(playerFleaUpA);
                                    playerOneCount++;

                                    FleaPlusA.Promotion(playerFleaUpA, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                else if (target is Flea && target.HP <= 5)
                                {
                                    FleaPlusB playerFleaUpB;

                                    playerFleaUpB = ((GameObject)Instantiate(FleaUpBPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusB>();

                                    playerFleaUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    playerFleaUpB.playerName = "Juicenjam";

                                    playerFleaUpB.isOwnedByPlayerOne = true;

                                    players.Add(playerFleaUpB);
                                    playerOneCount++;

                                    FleaPlusB.Promotion(playerFleaUpB, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                else
                        // AI-controlled Flea
                        if (target is FleaAI && target.HP > 5)
                                {
                                    FleaPlusAAI AIFleaUpA;

                                    AIFleaUpA = ((GameObject)Instantiate(AIFleaUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusAAI>();

                                    AIFleaUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    AIFleaUpA.playerName = "Juicenjam";

                                    AIFleaUpA.isOwnedByAI = true;

                                    players.Add(AIFleaUpA);
                                    aiPlayers.Add(AIFleaUpA);
                                    numberOfActiveAIUnits++;
                                    playerTwoCount++;
                                    playerAICount++;


                                    FleaPlusAAI.Promotion(AIFleaUpA, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                else if (target is FleaAI && target.HP <= 5)
                                {
                                    FleaPlusBAI AIFleaUpB;

                                    AIFleaUpB = ((GameObject)Instantiate(AIFleaUpBPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusBAI>();

                                    AIFleaUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    AIFleaUpB.playerName = "Juicenjam";

                                    AIFleaUpB.isOwnedByAI = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(AIFleaUpB);
                                    aiPlayers.Add(AIFleaUpB);
                                    numberOfActiveAIUnits++;
                                    playerTwoCount++;
                                    playerAICount++;


                                    FleaPlusBAI.Promotion(AIFleaUpB, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }



                                //Spider

                                if (target is Spider && target.HP > 5)
                                {
                                    SpiderPlusA playerSpiderUpA;

                                    playerSpiderUpA = ((GameObject)Instantiate(SpiderUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusA>();

                                    playerSpiderUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    playerSpiderUpA.playerName = "Juicenjam";

                                    playerSpiderUpA.isOwnedByPlayerOne = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(playerSpiderUpA);
                                    playerOneCount++;

                                    SpiderPlusA.Promotion(playerSpiderUpA, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                else if (target is Spider && target.HP <= 5)
                                {
                                    SpiderPlusB playerSpiderUpB;

                                    playerSpiderUpB = ((GameObject)Instantiate(SpiderUpBPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusB>();

                                    playerSpiderUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    playerSpiderUpB.playerName = "Juicenjam";

                                    playerSpiderUpB.isOwnedByPlayerOne = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(playerSpiderUpB);
                                    playerOneCount++;

                                    SpiderPlusA.Promotion(playerSpiderUpB, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                else
                        // AI controlled spider

                        if (target is SpiderAI && target.HP > 5)
                                {
                                    SpiderPlusAAI AISpiderUpA;

                                    AISpiderUpA = ((GameObject)Instantiate(AISpiderUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusAAI>();

                                    AISpiderUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    AISpiderUpA.playerName = "Juicenjam";

                                    AISpiderUpA.isOwnedByAI = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(AISpiderUpA);
                                    aiPlayers.Add(AISpiderUpA);
                                    numberOfActiveAIUnits++;
                                    playerTwoCount++;
                                    playerAICount++;


                                    SpiderPlusAAI.Promotion(AISpiderUpA, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                else
                        if (target is SpiderAI && target.HP <= 5)
                                {
                                    SpiderPlusBAI AISpiderUpB;

                                    AISpiderUpB = ((GameObject)Instantiate(AISpiderUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<SpiderPlusBAI>();

                                    AISpiderUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    AISpiderUpB.playerName = "Juicenjam";

                                    AISpiderUpB.isOwnedByAI = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(AISpiderUpB);
                                    aiPlayers.Add(AISpiderUpB);
                                    numberOfActiveAIUnits++;
                                    playerTwoCount++;
                                    playerAICount++;


                                    SpiderPlusBAI.Promotion(AISpiderUpB, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }

                                //Witch
                                if (target is Witch && target.HP > 5)
                                {

                                    WitchPlusA playerWitchUpA;

                                    playerWitchUpA = ((GameObject)Instantiate(WitchUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusA>();

                                    playerWitchUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    playerWitchUpA.playerName = "Juicenjam";

                                    playerWitchUpA.isOwnedByPlayerOne = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(playerWitchUpA);
                                    playerOneCount++;

                                    WitchPlusA.Promotion(playerWitchUpA, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                else if (target is Witch && target.HP <= 5)
                                {
                                    WitchPlusB playerWitchUpB;

                                    playerWitchUpB = ((GameObject)Instantiate(WitchUpBPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusB>();

                                    playerWitchUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    playerWitchUpB.playerName = "Juicenjam";

                                    playerWitchUpB.isOwnedByPlayerOne = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(playerWitchUpB);
                                    playerOneCount++;

                                    WitchPlusB.Promotion(playerWitchUpB, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                // AI-controlled witch
                                else
                                if (target is WitchAI && target.HP > 5)
                                {
                                    WitchPlusAAI AIWitchUpA;

                                    AIWitchUpA = ((GameObject)Instantiate(AIWitchUpAPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusAAI>();

                                    AIWitchUpA.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    AIWitchUpA.playerName = "Juicenjam";

                                    AIWitchUpA.isOwnedByAI = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(AIWitchUpA);
                                    aiPlayers.Add(AIWitchUpA);
                                    numberOfActiveAIUnits++;
                                    playerTwoCount++;
                                    playerAICount++;


                                    WitchPlusAAI.Promotion(AIWitchUpA, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
                                }
                                else
                                   if (target is WitchAI && target.HP <= 5)
                                {
                                    WitchPlusBAI AIWitchUpB;

                                    AIWitchUpB = ((GameObject)Instantiate(AIWitchUpBPrefab, new Vector3(target.transform.position.x, 0.55f, target.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<WitchPlusBAI>();

                                    AIWitchUpB.gridPosition = new Vector2(target.transform.position.x + Mathf.Floor(mapSize / 2), -target.transform.position.z + Mathf.Floor(mapSize / 2));
                                    AIWitchUpB.playerName = "Juicenjam";

                                    AIWitchUpB.isOwnedByAI = true;

                                    //playerWitchUpA.HP = attacker.HP;


                                    players.Add(AIWitchUpB);
                                    aiPlayers.Add(AIWitchUpB);
                                    numberOfActiveAIUnits++;
                                    playerTwoCount++;
                                    playerAICount++;


                                    WitchPlusBAI.Promotion(AIWitchUpB, target.HP);

                                    KillUnit(target);
                                    // destroy and respawn as the new unit i guess. this will probably cause all sorts of issues with AI though. hm
                                    // is there a way to just have the unit transform 
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
            foreach (Player p in players)
            {
                p.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    

    //destroy units
    public static void KillUnit(Player player)
    {


        player.isDestroyed = true;


            
            Destroy(player.gameObject);

        // revise the list of units

        for (int i = 0; i < instance.players.Count; i++)
        {
            if (instance.players[i] == player)
            {
                instance.players.RemoveAt(i);
            }


        }

        // revise list of AI units too, so that queue doesn't screw up
        for (int j = 0; j < instance.aiPlayers.Count; j++)
        {
            if (instance.aiPlayers[j] == player)
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
        if (player.isOwnedByAI == true)
        {
            playerAICount--;
           // instance.numberOfActiveAIUnits--;
            playerTwoCount--;

        }

        if (player.isOwnedByPlayerOne == true)
        {
            playerOneCount--;
        }
        else if (player.isOwnedByPlayerTwo == true)
        {
            playerTwoCount--;
        }

    }




    void generateMap()
    {
        //TODO: maybe add something that generates a map anyway, if no xml file.
        if (currentCampaignMap == 1)
        {
        loadMapFromXml("map.xml");

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

    void loadMapFromXml(string s)
    {
        MapXmlContainer container = MapSaveLoad.Load(s);


        mapSize = container.size;

        // initially remove all children
        for (int i = 0; i < mapTransform.transform.childCount; i++)
        {
            Destroy(mapTransform.GetChild(i).gameObject);
        }

        // also destroy all units
      
        for (int i = 0; i < players.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(players[i]);
            // }
        }
        // for some reason that leaves one unit, so for now, just, destroy it by destroying all units AGAIN:
        for (int i = 0; i < players.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(players[i]);
            // }
        }
        for (int i = 0; i < players.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(players[i]);
            // }
        }
        for (int i = 0; i < players.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(players[i]);
            // }
        }
        for (int i = 0; i < players.Count; i++)
        {
            //if (players[i] != null)
            // {
            KillUnit(players[i]);
            // }
        }
        // don't ask me why i need ot murder them five fucking times

        // don't let this carry over between matches.
        // this is probably not necessary
        currentAIUnitIndex = 0;


        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.transform.parent = mapTransform;
                tile.gridPosition = new Vector2(i, j);
                tile.setType((TileType)container.tiles.Where(x => x.locX == i && x.locY == j).First().id);
                row.Add(tile);

                // add HQs on HQ tiles.  this is a misnomer atm, change cityplayerone to hq etc
                if (tile.type == TileType.HQP1)
                {
                    HQ playerHQ;
                    playerHQ = ((GameObject)Instantiate(HQPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0,0,180)))).GetComponent<HQ>();
                    playerHQ.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerHQ.playerName = "Juicenjam";

                    playerHQ.isOwnedByPlayerOne = true;

                    players.Add(playerHQ);
                    playerOneCount++;
                }
                else if(tile.type == TileType.HQP2)
                {
                    HQ playerTwoHQ;
                    playerTwoHQ = ((GameObject)Instantiate(P2HQPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0,0,180)))).GetComponent<HQ>();
                    playerTwoHQ.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerTwoHQ.playerName = "Juicenjam";

                    playerTwoHQ.isOwnedByPlayerTwo = true;

                    players.Add(playerTwoHQ);
                    playerTwoCount++;
                }
                else if (tile.type == TileType.FleaP1)
                {
                    Flea playerFlea;
                    playerFlea = ((GameObject)Instantiate(FleaPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0,0,180)))).GetComponent<Flea>();
                    playerFlea.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerFlea.playerName = "Juicenjam";

                    playerFlea.isOwnedByPlayerOne = true;

                    players.Add(playerFlea);
                    playerOneCount++;
                }
                else if (tile.type == TileType.FleaP2)
                {
                    Flea playerFlea;
                    playerFlea = ((GameObject)Instantiate(P2FleaPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<Flea>();
                    playerFlea.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerFlea.playerName = "Juicenjam";

                    playerFlea.isOwnedByPlayerTwo = true;

                    players.Add(playerFlea);
                    playerTwoCount++;
                }
                else if (tile.type == TileType.FleaAI)
                {
                    FleaAI AIFlea;
                    AIFlea = ((GameObject)Instantiate(AIFleaPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0,0,180)))).GetComponent<FleaAI>();
                    AIFlea.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    AIFlea.playerName = "Juicenjam";

                    AIFlea.isOwnedByAI = true;

                    players.Add(AIFlea);
                    aiPlayers.Add(AIFlea);
                    numberOfActiveAIUnits++;
                    //playerTwoCount++;
                    playerAICount++;
                }
                else if (tile.type == TileType.SpiderP1)
                {
                    Spider playerSpider;
                    playerSpider = ((GameObject)Instantiate(SpiderPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0,0,180)))).GetComponent<Spider>();
                    playerSpider.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerSpider.playerName = "Juicenjam";

                    playerSpider.isOwnedByPlayerOne = true;

                    players.Add(playerSpider);
                    playerOneCount++;
                }
                else if (tile.type == TileType.SpiderP2)
                {
                    Spider playerSpider;
                    playerSpider = ((GameObject)Instantiate(P2SpiderPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<Spider>();
                    playerSpider.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerSpider.playerName = "Juicenjam";

                    playerSpider.isOwnedByPlayerTwo = true;

                    players.Add(playerSpider);
                    playerTwoCount++;
                }
                else if (tile.type == TileType.SpiderAI)
                {
                    SpiderAI AISpider;
                    AISpider = ((GameObject)Instantiate(AISpiderPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0,0,180)))).GetComponent<SpiderAI>();
                    AISpider.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    AISpider.playerName = "Juicenjam";

                    AISpider.isOwnedByAI = true;

                    players.Add(AISpider);
                    aiPlayers.Add(AISpider);
                    numberOfActiveAIUnits++;
                    //playerTwoCount++;
                    playerAICount++;
                }
                else if (tile.type == TileType.WitchP1)
                {
                    Witch playerWitch;
                    playerWitch = ((GameObject)Instantiate(WitchPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0,0,180)))).GetComponent<Witch>();
                    playerWitch.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerWitch.playerName = "Juicenjam";

                    playerWitch.isOwnedByPlayerOne = true;

                    players.Add(playerWitch);
                    playerOneCount++;
                }
                else if (tile.type == TileType.WitchP2)
                {
                    Witch playerWitch;
                    playerWitch = ((GameObject)Instantiate(P2WitchPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<Witch>();
                    playerWitch.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    playerWitch.playerName = "Juicenjam";

                    playerWitch.isOwnedByPlayerTwo = true;

                    players.Add(playerWitch);
                    playerTwoCount++;
                }
                else if (tile.type == TileType.WitchAI)
                {
                    WitchAI AIWitch;
                    AIWitch = ((GameObject)Instantiate(AIWitchPrefab, new Vector3(tile.transform.position.x, 0.55f, tile.transform.position.z), Quaternion.Euler(new Vector3(0,0,180)))).GetComponent<WitchAI>();
                    AIWitch.gridPosition = new Vector2(tile.transform.position.x + Mathf.Floor(mapSize / 2), -tile.transform.position.z + Mathf.Floor(mapSize / 2));
                    AIWitch.playerName = "Juicenjam";

                    AIWitch.isOwnedByAI = true;

                    players.Add(AIWitch);
                    aiPlayers.Add(AIWitch);
                    numberOfActiveAIUnits++;
                    //playerTwoCount++;
                    playerAICount++;
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
            if (playerOneTurn && fundsArmyOne >= Flea.cost)
            {
                Flea newFlea;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);

               
                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newFlea = ((GameObject)Instantiate(FleaPrefab, new Vector3(Tile.myTile.transform.position.x, 0.55f, Tile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Flea>();
                newFlea.gridPosition = new Vector2(Tile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -Tile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newFlea.playerName = "Juicenjam";
                newFlea.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newFlea.isOwnedByPlayerOne = true;

                players.Add(newFlea);
                playerOneCount++;
                fundsArmyOne -= Flea.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
            // if player two, spawn player two infantry
            else if (playerTwoTurn && fundsArmyTwo >= Flea.cost)
            {
                Flea newFlea;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newFlea = ((GameObject)Instantiate(P2FleaPrefab, new Vector3(Tile.myTile.transform.position.x, 0.55f, Tile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Flea>();
                newFlea.gridPosition = new Vector2(Tile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -Tile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newFlea.playerName = "Juicenjam";
                newFlea.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newFlea.isOwnedByPlayerTwo = true;

                players.Add(newFlea);
                playerTwoCount++;
                fundsArmyTwo -= Flea.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
        }


        //mech
        rect = new Rect(10, Screen.height -110, 100, 20);
        if (GUI.Button(rect, "Spider"))
        {
            // if it's player one's turn, and they can afford it, spawn a player one infantry.
            if (playerOneTurn && fundsArmyOne >= Spider.cost)
            {
                Spider newSpider;
               
                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newSpider = ((GameObject)Instantiate(SpiderPrefab, new Vector3(Tile.myTile.transform.position.x, 0.55f, Tile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Spider>();
                newSpider.gridPosition = new Vector2(Tile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -Tile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newSpider.playerName = "Juicenjam";
                newSpider.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newSpider.isOwnedByPlayerOne = true;

                players.Add(newSpider);
                playerOneCount++;
                fundsArmyOne -= Spider.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
            // if player two, spawn for player two
            else if (playerTwoTurn && fundsArmyTwo >= Spider.cost)
            {
                Spider newSpider;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newSpider = ((GameObject)Instantiate(P2SpiderPrefab, new Vector3(Tile.myTile.transform.position.x, 0.55f, Tile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Spider>();
                newSpider.gridPosition = new Vector2(Tile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -Tile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newSpider.playerName = "Juicenjam";
                newSpider.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newSpider.isOwnedByPlayerTwo = true;

                players.Add(newSpider);
                playerTwoCount++;
                fundsArmyTwo -= Spider.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
        }

        //bike
        rect = new Rect(10, Screen.height - 140, 100, 20);
        if (GUI.Button(rect, "Witch"))
        {
            // if it's player one's turn, and they can afford it, spawn a player one infantry.
            if (playerOneTurn && fundsArmyOne >= Witch.cost)
            {
                Witch newWitch;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newWitch = ((GameObject)Instantiate(WitchPrefab, new Vector3(Tile.myTile.transform.position.x, 0.55f, Tile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Witch>();
                newWitch.gridPosition = new Vector2(Tile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -Tile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newWitch.playerName = "Juicenjam";
                newWitch.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newWitch.isOwnedByPlayerOne = true;

                players.Add(newWitch);
                playerOneCount++;
                fundsArmyOne -= Witch.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
            // if player two, spawn player two infantry
            else if (playerTwoTurn && fundsArmyTwo >= Witch.cost)
            {
                Witch newWitch;
                //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
                //newInfantry.gridPosition = new Vector2(0, 0);


                // i dont know how the gridPosition math works out on the y axis but whatever it works.
                newWitch = ((GameObject)Instantiate(P2WitchPrefab, new Vector3(Tile.myTile.transform.position.x, 0.55f, Tile.myTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Witch>();
                newWitch.gridPosition = new Vector2(Tile.myTile.transform.position.x + Mathf.Floor(mapSize / 2), -Tile.myTile.transform.position.z + Mathf.Floor(mapSize / 2));
                newWitch.playerName = "Juicenjam";
                newWitch.waiting = true;
                // need to figure out how to make this update properly next turn so you can move it.

                newWitch.isOwnedByPlayerTwo = true;

                players.Add(newWitch);
                playerTwoCount++;
                fundsArmyTwo -= Witch.cost;

                //trying to get rid of button after usage, add it, after calling this method, to tile
                //TilebuttonPressed = false;
            }
      

        }
    }
}
