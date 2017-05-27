using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class BattleUI : MonoBehaviour
{
    public void AttackButton()
    {
        // if unit is owned by player whose turn it is, can attack
        if ((GameManager.instance.playerOneTurn && GameManager.myUnit.isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.myUnit.isOwnedByPlayerTwo))
        {
            if (GameManager.myUnit.attacking == false && GameManager.myUnit.waiting == false)
            {
                if ((GameManager.myUnit.isFleaUpB && GameManager.myUnit.fleaActionPoints <= 0) != true)
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
            // temporarily ignore raycasting for living enemy units so you can target them
            if (GameManager.instance.playerOneTurn)
            {
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
            GameManager.instance.ReactivateAButton();
        }
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
