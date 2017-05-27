using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



public class Tile : MonoBehaviour {

    bool readyToBuild = true;
   

    public GameObject visual;

    public static Tile myTile;
    public static Tile buildTile;

    GameObject PREFAB;

    public TileType type = TileType.Normal;

    public Vector2 gridPosition = Vector2.zero;

    bool buttonPressed = false;

    public int movementCost = 1;
    public bool impassable = false;
    public bool canBuildLandUnits = false;
    public bool canBuildAirUnits = false;
    public bool canBuildSeaUnits = false;
    public bool addsIncomePlayerOne = false;
    public bool addsIncomePlayerTwo = false;
    public int baseCapturePoints;
    public int capturePoints;
    public bool neutralProperty;
    public bool endsGame;

    public List<Tile> neighbors = new List<Tile>();


    // Use this for initialization
    void Start() {
        if (Application.loadedLevelName == "Campaign")
        {
            generateNeighbors();
            // will only happen gamescene but may want this in mapmaking and whatnot to change tile visuals based on neighbors (clustered forests etc)
        }
    }

  

   
    void generateNeighbors()
    {
        neighbors = new List<Tile>();

        // this stuff will be wrong if you don't use a square grid

        //up
        if (gridPosition.y > 0)
        {
            Vector2 n = new Vector2(gridPosition.x, gridPosition.y - 1);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }



        //down
        if (gridPosition.y < GameManager.instance.map.Count - 1)
        {
            Vector2 n = new Vector2(gridPosition.x, gridPosition.y + 1);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }



        //left
        if (gridPosition.x > 0)
        {
            Vector2 n = new Vector2(gridPosition.x - 1, gridPosition.y);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }


        //right
        if (gridPosition.x < GameManager.instance.map.Count - 1)
        {
            Vector2 n = new Vector2(gridPosition.x + 1, gridPosition.y);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }

    }

  /*  void generateUnits()
    {
        if (this.TileType == HQPlayerOne)
        HQ playerHQ;
        playerHQ = ((GameObject)Instantiate(GameManager.instance.HQPrefab, new Vector3(this.transform.position.x, 0.55f, this.transform.position.z), Quaternion.Euler(new Vector3()))).GetComponent<HQ>();
        playerHQ.gridPosition = new Vector2(this.transform.position.x + Mathf.Floor(GameManager.instance.mapSize / 2), -this.transform.position.z + Mathf.Floor(GameManager.instance.mapSize / 2));
        playerHQ.playerName = "Juicenjam";

        playerHQ.isOwnedByPlayerOne = true;

        GameManager.instance.players.Add(playerHQ);
        GameManager.playerOneCount++;
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("mouse 0"))
        {
            buttonPressed = true;
            //Debug.Log("xyz position is " + myTile.transform.position.x + ", " + myTile.transform.position.y + ", " + myTile.transform.position.z);
        }

    }

    void OnMouseEnter()
    {
        if (Application.loadedLevelName == "Map Creator" && Input.GetMouseButton(0))
        {
            setType(MapCreatorManager.instance.paletteSelection);
        }

        /*
            if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving)
        {
            Debug.Log(gameObject.name + " ok");
            transform.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking)
        {
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
        */
    }



    void OnMouseExit()
    {
        // transform.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }


    // right click to cancel unit stuff
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (GameManager.myUnit.moving == true)
            {
                GameManager.myUnit.moving = false;
                GameManager.instance.removeTileHighlights();


            }
            if (GameManager.myUnit.attacking == true)
            {
                GameManager.myUnit.attacking = false;
                GameManager.instance.removeTileHighlights();

            }
            GameManager.instance.ButtonCanvas.transform.position = new Vector3(-20, 0, 0);
            GameManager.instance.BuildUnitCanvas.transform.position = new Vector3(-20, 0, 0);

        }
    }


    void OnMouseUp()
    {
        if (GameManager.instance.myUnitIsBeingUsed == true)
        {
            GameManager.instance.ButtonCanvas.transform.position = new Vector3(transform.position.x + 6, transform.position.y + 4, transform.position.z + 7);
            GameManager.instance.myUnitIsBeingUsed = false;
            GameManager.instance.myUnitMustWaitOrAttack = true;
            GameManager.instance.DeactivateAButton();

        }

    }


    void OnMouseDown()
    {
      

        myTile = this;
        // change that game scnee to whatver the acutal name of the game scene is.  this applies to other situatios where i did this, make sur eyou get it right
        if (Application.loadedLevelName == "Campaign")
        {





            //Player.instance.moving
            //GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving

            if (GameManager.myUnit != null)
            {
                if (GameManager.myUnit.moving && !(GameManager.myUnit is HQ))
                {

                    // can call something like 'confirm path' method here, in the future
                    GameManager.instance.moveCurrentPlayer(this);
                    // now occurring onMouseUp
                    //GameManager.instance.ButtonCanvas.transform.position = new Vector3(transform.position.x + 3.5f, transform.position.y, transform.position.z + 6);

                }
                //else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking)
                else if (GameManager.myUnit.attacking && !(GameManager.myUnit is HQ))
                {
                    // trying to first call ConfirmTarget method
                    GameManager.instance.confirmTarget(this);
                    // may need to move attackWithCurrentPlayer to confirmTarget or something?
                    GameManager.instance.attackWithCurrentPlayer(this);

                }
            }
            

                if (this.canBuildLandUnits && ((this.addsIncomePlayerOne && GameManager.instance.playerOneTurn) || (addsIncomePlayerTwo && GameManager.instance.playerTwoTurn)))
            {
                buildTile = myTile;
                GameManager.instance.BuildUnitCanvas.transform.position = new Vector3(transform.position.x + 3.5f, transform.position.y, transform.position.z + 6);

                // spawn buttons???
            }
            /* else
             {
                 // if the tile is a base, and is owned by the player whose turn it is, you can build units from it
                 if (this.canBuildLandUnits && ((this.addsIncomePlayerOne && GameManager.instance.playerOneTurn) || (addsIncomePlayerTwo && GameManager.instance.playerTwoTurn)))
                 {
                     // need some sort of gui thing to pick a unit

                     OnGUI();
                     // then, it'll instantiate the unit just like in gamemanager's auto unit generation.
                 }

                 /* this is test code that allows you to click tiles to highlight them and make them impassable
                 impassable = impassable ? false : true;
                 if (impassable)
                 {
                     visual.transform.GetComponent<Renderer>().materials[0].color = new Color(0.5f, 0.5f, 0.0f);
                 }
                 else
                 {
                     visual.transform.GetComponent<Renderer>().materials[0].color = Color.white;
                 }

             }*/
        }
        

        else if (Application.loadedLevelName == "Map Creator")
        {
            setType(MapCreatorManager.instance.paletteSelection);
        }
            }





    public void OnGUI()
    {
        if (Application.loadedLevelName == "Campaign" || Application.loadedLevelName == "NetworkVersus")
        {
            

            if (buttonPressed && myTile != null)
            {
                // if the tile clicked is a base and it's owned by the player whose turn it is, you can build a unit.
                // if there's a unit on the base, you can't build from it
                if (myTile.canBuildLandUnits && ((myTile.addsIncomePlayerOne && GameManager.instance.playerOneTurn) || (myTile.addsIncomePlayerTwo && GameManager.instance.playerTwoTurn)))
                {
                    foreach (Player p in GameManager.instance.players)
                    {
                        if (p.gridPosition == myTile.gridPosition)
                        {
                            readyToBuild = false;
                        }
                        else
                        {
                            readyToBuild = true;
                        }
                    }
                    if (readyToBuild)
                    {

                        GameManager.instance.BuildUnitsGUI();
                    }


                    /*rect = new Rect(10 + (100 + 10) * 1, Screen.height - 80, 100, 60);
                    if (GUI.Button(rect, "Antiair"))
                    {
                        paletteSelection = TileType.CityPlayerOne;
                    }*/

                    /* rect = new Rect(10 + (100 + 10) * 2, Screen.height - 80, 100, 60);
                     if (GUI.Button(rect, "Alphabalpha"))
                     {
                         paletteSelection = TileType.CityPlayerTwo;
                     }

                     rect = new Rect(10 + (100 + 10) * 3, Screen.height - 80, 100, 60);
                     if (GUI.Button(rect, "BaseP1"))
                     {
                         paletteSelection = TileType.BasePlayerOne;
                     }
                     */
                }                
            }
        }
    }


    
    
    public void setType(TileType t)
    {
        // tile information stored here
        type = t;
        switch (t)
        {
            case TileType.CityPlayerOne:
                movementCost = 1;
                impassable = false;
                addsIncomePlayerOne = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                PREFAB = PrefabHolder.instance.TILE_CITY_PLAYER_ONE_PREFAB;
                break;
            case TileType.CityPlayerTwo:
                movementCost = 1;
                impassable = false;
                addsIncomePlayerTwo = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                PREFAB = PrefabHolder.instance.TILE_CITY_PLAYER_TWO_PREFAB;
                break;
            case TileType.BasePlayerOne:
                movementCost = 1;
                impassable = false;
                addsIncomePlayerOne = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                canBuildLandUnits = true;
                PREFAB = PrefabHolder.instance.TILE_BASE_PLAYER_ONE_PREFAB;
                break;
            case TileType.BasePlayerTwo:
                movementCost = 1;
                impassable = false;
                addsIncomePlayerTwo = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                canBuildLandUnits = true;
                PREFAB = PrefabHolder.instance.TILE_BASE_PLAYER_TWO_PREFAB;
                break;
            case TileType.CityNeutral:
                movementCost = 1;
                impassable = false;
                neutralProperty = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                PREFAB = PrefabHolder.instance.TILE_CITY_NEUTRAL_PREFAB;
                break;
            case TileType.BaseNeutral:
                movementCost = 1;
                impassable = false;
                neutralProperty = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                canBuildLandUnits = true;
                PREFAB = PrefabHolder.instance.TILE_BASE_NEUTRAL_PREFAB;
                break;
            case TileType.AirportNeutral:
                movementCost = 1;
                impassable = false;
                neutralProperty = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                canBuildAirUnits = true;
                PREFAB = PrefabHolder.instance.TILE_AIRPORT_NEUTRAL_PREFAB;
                break;
            case TileType.PortNeutral:
                movementCost = 1;
                impassable = false;
                neutralProperty = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                canBuildSeaUnits = true;
                PREFAB = PrefabHolder.instance.TILE_PORT_NEUTRAL_PREFAB;
                break;
            case TileType.HQPlayerOne:
                movementCost = 1;
                impassable = false;
                addsIncomePlayerOne = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                endsGame = true;
                PREFAB = PrefabHolder.instance.TILE_HQ_PLAYER_ONE_PREFAB;





                break;
            case TileType.HQPlayerTwo:
                movementCost = 1;
                impassable = false;
                addsIncomePlayerTwo = true;
                baseCapturePoints = 20;
                capturePoints = 20;
                endsGame = true;
                PREFAB = PrefabHolder.instance.TILE_HQ_PLAYER_TWO_PREFAB;
                break;



            case TileType.Normal:
                movementCost = 1;
                impassable = false;
                PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                break;
            case TileType.Difficult:
                movementCost = 2;
                impassable = false;
                PREFAB = PrefabHolder.instance.TILE_DIFFICULT_PREFAB;
                break;
            case TileType.VeryDifficult:
                movementCost = 4;
                impassable = false;
                PREFAB = PrefabHolder.instance.TILE_VERY_DIFFICULT_PREFAB;
                break;
            case TileType.Impassable:
                movementCost = 9999;
                impassable = true;
                PREFAB = PrefabHolder.instance.TILE_IMPASSABLE_PREFAB;
                break;


            // Unit Spawn Tiles

            //SceneManager. == "Campaign")


            case TileType.FleaP1:
                movementCost = 1;
                impassable = false;
                if (Application.loadedLevelName == "Map Creator")
                {
                    PREFAB = PrefabHolder.instance.TILE_P1FLEA_PREFAB;
                }
                else
                {
                    PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                }
                break;
            case TileType.FleaAI:
                movementCost = 1;
                impassable = false;
                if (Application.loadedLevelName == "Map Creator")
                {
                    PREFAB = PrefabHolder.instance.TILE_AIFLEA_PREFAB;
                }
                else
                {
                    PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                }
                break;
            case TileType.SpiderP1:
                movementCost = 1;
                impassable = false;
                if (Application.loadedLevelName == "Map Creator")
                {
                    PREFAB = PrefabHolder.instance.TILE_P1SPIDER_PREFAB;
                }
                else
                {
                    PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                }
                break;
            case TileType.SpiderAI:
                movementCost = 1;
                impassable = false;
                if (Application.loadedLevelName == "Map Creator")
                {
                    PREFAB = PrefabHolder.instance.TILE_AISPIDER_PREFAB;
                }
                else
                {
                    PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                }
                break;
            case TileType.WitchP1:
                movementCost = 1;
                impassable = false;
                if (Application.loadedLevelName == "Map Creator")
                {
                    PREFAB = PrefabHolder.instance.TILE_P1WITCH_PREFAB;
                }
                else
                {
                    PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                }
                break;
            case TileType.WitchAI:
                movementCost = 1;
                impassable = false;
                if (Application.loadedLevelName == "Map Creator")
                {
                    PREFAB = PrefabHolder.instance.TILE_AIWITCH_PREFAB;
                }
                else
                {
                    PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                }
                break;
        


            default:
                movementCost = 1;
                impassable = false;
                PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                break;
        }

        generateVisuals();
          
    }
    public void generateVisuals()
    {
        GameObject container = transform.FindChild("Visuals").gameObject;
        // initially remove all children
        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
        GameObject newVisual = (GameObject)Instantiate(PREFAB, transform.position, Quaternion.Euler(new Vector3(0, 180, 0)));
        newVisual.transform.parent = container.transform;

        visual = newVisual;
    }
}
