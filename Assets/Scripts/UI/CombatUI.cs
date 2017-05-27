using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class CombatUI : MonoBehaviour
{
    
    public void BuildFleaButton()
    {
        if (GameManager.instance.playerOneTurn && GameManager.instance.fundsArmyOne >= Flea.cost)
        {
            Flea newFlea;
            //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
            //newInfantry.gridPosition = new Vector2(0, 0);


            // i dont know how the gridPosition math works out on the y axis but whatever it works.
            newFlea = ((GameObject)Instantiate(GameManager.instance.FleaPrefab, new Vector3(Tile.buildTile.transform.position.x, 0.55f, Tile.buildTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Flea>();
            newFlea.gridPosition = new Vector2(Tile.buildTile.transform.position.x + Mathf.Floor(GameManager.instance.mapSize / 2), -Tile.buildTile.transform.position.z + Mathf.Floor(GameManager.instance.mapSize / 2));
            newFlea.playerName = "Juicenjam";
            newFlea.waiting = true;
            // need to figure out how to make this update properly next turn so you can move it.

            newFlea.isOwnedByPlayerOne = true;

            GameManager.instance.players.Add(newFlea);
            GameManager.playerOneCount++;
            GameManager.instance.fundsArmyOne -= Flea.cost;

            //trying to get rid of button after usage, add it, after calling this method, to tile
            //TilebuttonPressed = false;
        }
        // if player two, spawn player two infantry
        else if (GameManager.instance.playerTwoTurn && GameManager.instance.fundsArmyTwo >= Flea.cost)
        {
            Flea newFlea;
            //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
            //newInfantry.gridPosition = new Vector2(0, 0);


            // i dont know how the gridPosition math works out on the y axis but whatever it works.
            newFlea = ((GameObject)Instantiate(GameManager.instance.P2FleaPrefab, new Vector3(Tile.buildTile.transform.position.x, 0.55f, Tile.buildTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Flea>();
            newFlea.gridPosition = new Vector2(Tile.buildTile.transform.position.x + Mathf.Floor(GameManager.instance.mapSize / 2), -Tile.buildTile.transform.position.z + Mathf.Floor(GameManager.instance.mapSize / 2));
            newFlea.playerName = "Juicenjam";
            newFlea.waiting = true;
            // need to figure out how to make this update properly next turn so you can move it.

            newFlea.isOwnedByPlayerTwo = true;

            GameManager.instance.players.Add(newFlea);
            GameManager.playerTwoCount++;
            GameManager.instance.fundsArmyTwo -= Flea.cost;

            //trying to get rid of button after usage, add it, after calling this method, to tile
            //TilebuttonPressed = false;
        }

        GameManager.instance.BuildUnitCanvas.transform.position = new Vector3(-20, 0, 0);

    }

    public void BuildWitchButton()
    {
        if (GameManager.instance.playerOneTurn && GameManager.instance.fundsArmyOne >= Witch.cost)
        {
            Witch newWitch;
            //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
            //newInfantry.gridPosition = new Vector2(0, 0);


            // i dont know how the gridPosition math works out on the y axis but whatever it works.
            newWitch = ((GameObject)Instantiate(GameManager.instance.WitchPrefab, new Vector3(Tile.buildTile.transform.position.x, 0.55f, Tile.buildTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Witch>();
            newWitch.gridPosition = new Vector2(Tile.buildTile.transform.position.x + Mathf.Floor(GameManager.instance.mapSize / 2), -Tile.buildTile.transform.position.z + Mathf.Floor(GameManager.instance.mapSize / 2));
            newWitch.playerName = "Juicenjam";
            newWitch.waiting = true;
            // need to figure out how to make this update properly next turn so you can move it.

            newWitch.isOwnedByPlayerOne = true;

            GameManager.instance.players.Add(newWitch);
            GameManager.playerOneCount++;
            GameManager.instance.fundsArmyOne -= Witch.cost;

            //trying to get rid of button after usage, add it, after calling this method, to tile
            //TilebuttonPressed = false;
        }
        // if player two, spawn player two infantry
        else if (GameManager.instance.playerTwoTurn && GameManager.instance.fundsArmyTwo >= Witch.cost)
        {
            Witch newWitch;
            //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
            //newInfantry.gridPosition = new Vector2(0, 0);


            // i dont know how the gridPosition math works out on the y axis but whatever it works.
            newWitch = ((GameObject)Instantiate(GameManager.instance.P2WitchPrefab, new Vector3(Tile.buildTile.transform.position.x, 0.55f, Tile.buildTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Witch>();
            newWitch.gridPosition = new Vector2(Tile.buildTile.transform.position.x + Mathf.Floor(GameManager.instance.mapSize / 2), -Tile.buildTile.transform.position.z + Mathf.Floor(GameManager.instance.mapSize / 2));
            newWitch.playerName = "Juicenjam";
            newWitch.waiting = true;
            // need to figure out how to make this update properly next turn so you can move it.

            newWitch.isOwnedByPlayerTwo = true;

            GameManager.instance.players.Add(newWitch);
            GameManager.playerTwoCount++;
            GameManager.instance.fundsArmyTwo -= Witch.cost;

            //trying to get rid of button after usage, add it, after calling this method, to tile
            //TilebuttonPressed = false;
        }
        GameManager.instance.BuildUnitCanvas.transform.position = new Vector3(-20, 0, 0);

    }

    public void BuildSpiderButton()
    {
        if (GameManager.instance.playerOneTurn && GameManager.instance.fundsArmyOne >= Spider.cost)
        {
            Spider newSpider;

            // i dont know how the gridPosition math works out on the y axis but whatever it works.
            newSpider = ((GameObject)Instantiate(GameManager.instance.SpiderPrefab, new Vector3(Tile.buildTile.transform.position.x, 0.55f, Tile.buildTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Spider>();
            newSpider.gridPosition = new Vector2(Tile.buildTile.transform.position.x + Mathf.Floor(GameManager.instance.mapSize / 2), -Tile.buildTile.transform.position.z + Mathf.Floor(GameManager.instance.mapSize / 2));
            newSpider.playerName = "Juicenjam";
            newSpider.waiting = true;
            // need to figure out how to make this update properly next turn so you can move it.

            newSpider.isOwnedByPlayerOne = true;

            GameManager.instance.players.Add(newSpider);
            GameManager.playerOneCount++;
            GameManager.instance.fundsArmyOne -= Spider.cost;

            //trying to get rid of button after usage, add it, after calling this method, to tile
            //TilebuttonPressed = false;
        }
        // if player two, spawn for player two
        else if (GameManager.instance.playerTwoTurn && GameManager.instance.fundsArmyTwo >= Spider.cost)
        {
            Spider newSpider;
            //newInfantry = ((GameObject)Instantiate(GameManager.instance.InfantryPrefab, new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize / 2), 1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Infantry>();
            //newInfantry.gridPosition = new Vector2(0, 0);


            // i dont know how the gridPosition math works out on the y axis but whatever it works.
            newSpider = ((GameObject)Instantiate(GameManager.instance.P2SpiderPrefab, new Vector3(Tile.buildTile.transform.position.x, 0.55f, Tile.buildTile.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)))).GetComponent<Spider>();
            newSpider.gridPosition = new Vector2(Tile.buildTile.transform.position.x + Mathf.Floor(GameManager.instance.mapSize / 2), -Tile.buildTile.transform.position.z + Mathf.Floor(GameManager.instance.mapSize / 2));
            newSpider.playerName = "Juicenjam";
            newSpider.waiting = true;
            // need to figure out how to make this update properly next turn so you can move it.

            newSpider.isOwnedByPlayerTwo = true;

            GameManager.instance.players.Add(newSpider);
            GameManager.playerTwoCount++;
            GameManager.instance.fundsArmyTwo -= Spider.cost;

            //trying to get rid of button after usage, add it, after calling this method, to tile
            //TilebuttonPressed = false;
        }
        GameManager.instance.BuildUnitCanvas.transform.position = new Vector3(-20, 0, 0);

    }


    public void AttackButton()
    {
        // if unit is owned by player whose turn it is, can attack
        if ((GameManager.instance.playerOneTurn && GameManager.myUnit.isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.myUnit.isOwnedByPlayerTwo))
        {
            if (GameManager.myUnit.attacking == false && GameManager.myUnit.waiting == false)
            {
                GameManager.instance.ButtonCanvas.transform.position = new Vector3(-20,0,0);

                // Flea B Upgrade has special rules since it has two actions per turn.
                if ((
                    //GameManager.myUnit.isFleaUpB && 
                    GameManager.myUnit.fleaActionPoints <= 0) != true)
                {
                    GameManager.instance.removeTileHighlights();
                    GameManager.myUnit.moving = false;
                    GameManager.myUnit.attacking = true;
                    GameManager.myUnit.waiting = false;
                    GameManager.instance.highlightTilesAt(GameManager.myUnit.gridPosition, Color.red, GameManager.myUnit.attackRange);
                }
            }
            else
            {

                GameManager.myUnit.moving = false;
                GameManager.myUnit.attacking = false;
                GameManager.instance.removeTileHighlights();
            }
            GameManager.instance.ReactivateAButton();

            // temporarily ignore raycasting for living enemy units so you can target them
            if (GameManager.instance.playerOneTurn)
            {

                // make sure to add this to player two
                // healers can target allies
                if (GameManager.myUnit.isHealer)
                {
                    foreach (Player u in GameManager.instance.players)
                    {
                        if (u != null)
                        {
                            u.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                        }
                    }
                }


                foreach (Player u in GameManager.instance.players)
                {
                    if (u.isOwnedByPlayerOne == false && u != null)
                    {
                        u.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                }
            }

            if (GameManager.instance.playerTwoTurn)
            {
                foreach (Player u in GameManager.instance.players)
                {
                    if (u.isOwnedByPlayerTwo == false && u != null)
                    {
                        u.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                }
            }
        }
    }

    public void WaitButton()
    {
        // if unit is owned by player whose turn it is, can wait
        if ((GameManager.instance.playerOneTurn && GameManager.myUnit.isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.myUnit.isOwnedByPlayerTwo))
        {
            GameManager.instance.removeTileHighlights();
            // actionPoints = 2;
            GameManager.myUnit.moving = false;
            GameManager.myUnit.attacking = false;
            GameManager.myUnit.waiting = true;
            //GameManager.instance.NextTurn();
            GameManager.instance.ButtonCanvas.transform.position = new Vector3(-20, 0, 0);
            GameManager.instance.ReactivateAButton();


        }
    }

    public void EndTurnButton()
    {
        GameManager.instance.removeTileHighlights();
        
        //this is here to get rid of the buttoncanvas when you end the turn. but in future the end turn button itself should just be deactivated
        // while you're in the process of moving a unit. 
        GameManager.instance.ButtonCanvas.transform.position = new Vector3(-20, 0, 0);

        //currently this affects all units, not just the ones you own
        foreach (Player p in GameManager.instance.players)
        {
            if (p != null)
            {
                if (p.isFleaUpB)
                {
                    p.fleaActionPoints = 2;
                    p.fleaMovementPoints = 2;
                }

                p.waiting = false;
                p.moving = false;
                p.attacking = false;

                // when turn ends, layermask goes back to normal so next player can click units

                p.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            // no selected unit
            GameManager.myUnit = null;
        }
        // make it so all units stop waiting
        GameManager.instance.NextTurn();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //get rid of this
        /*
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("Campaign");
        }
    */
    }

    void OnMouseEnter()
    {

    }

    void OnMouseExit()
    {

    }

    void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            print("something");
        }
    }
}
