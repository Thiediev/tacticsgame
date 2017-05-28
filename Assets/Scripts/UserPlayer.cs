using UnityEngine;
using System.Collections;

public class UserPlayer : Player
{
    public AudioClip RightClickCancel;
    public AudioClip SelectUnit;

    public void Start()
    {

    }

    public override void Update()
    {
        if (GameManager.myUnit == this)
        {
            if ((GameManager.instance.playerOneTurn && GameManager.myUnit.isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.myUnit.isOwnedByPlayerTwo))
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

    public override void TurnUpdate()
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
                    fleaMovementPoints--;
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

    // right click to cancel
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
            GetComponent<AudioSource>().PlayOneShot(RightClickCancel);
            GameManager.instance.ButtonCanvas.transform.position = new Vector3(-20, 0, 0);
            GameManager.myUnit = null;
        }
    }
    // choose a unit. or not
    void OnMouseDown()
    {
        // If the unit is already selected, clicking the unit again will open up the attack/wait menu without moving the unit
        if (this.waiting)
        { }
        else
        if (this == GameManager.myUnit && !(GameManager.myUnit is HQ))
        {
            GameManager.instance.myUnitIsBeingUsed = false;
            GameManager.instance.removeTileHighlights();
            GameManager.myUnit.moving = false;
            GameManager.instance.ButtonCanvas.transform.position = new Vector3(transform.position.x + 6, transform.position.y + 4, transform.position.z + 7);
            GameManager.instance.DeactivateAButton();
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(SelectUnit);

            GameManager.instance.myUnitIsBeingUsed = true;
            //active unit becomes the one selected i hope
            GameManager.myUnit = this;

            //GameManager.instance.ButtonCanvas.transform.position = new Vector3(transform.position.x + 3.5f, transform.position.y, transform.position.z + 6);

            // if the unit is owned by the player whose turn it is, can move unit
            if ((GameManager.instance.playerOneTurn && GameManager.myUnit.isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.myUnit.isOwnedByPlayerTwo))
            {
                if (fleaMovementPoints > 0)
                {
                    if (!moving && !waiting)
                    {
                        foreach (Player p in GameManager.instance.players)
                            p.moving = false;
 
                        // get rid of attack/wait buttons
                        GameManager.instance.ButtonCanvas.transform.position = new Vector3(-100, 0, 0);
                     
                        GameManager.instance.removeTileHighlights();
                        moving = true;
                        attacking = false;
                        waiting = false;
                        GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false, false);
                    }
                    else
                    {
                        moving = false;
                        attacking = false;
                        GameManager.instance.removeTileHighlights();
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
                     }*/
            }
        }
    }
}