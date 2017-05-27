using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIPlayer : Player {

    public int MustUpdatePath = 0;

    // later, will want to make it a bit more advanced; for example, units that can attack should go first, fleas go last, spiders go first, etc
        void Awake()
    {
        
    }

    void Start() {
        GameManager.instance.numberOfActiveAIUnits = GameManager.instance.aiPlayers.Count;
    }

    public override void Update()
    {
        // for now, if it's player two's turn we assume it's AI's turn
        if (GameManager.instance.playerTwoTurn)
        {
            if (GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].HP > 0 && GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting != true
                )
            {
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].TurnUpdate();
            }
            
           /* else
            {
                //wait two seconds or something
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting = false;
                GameManager.instance.currentAIUnitIndex++;

                //            GameManager.instance.NextTurn();
            }*/
        }
        base.Update();
    }

    public void SetUnitActions()
    {
            // priority queue
            // attack if in range and with lowest hp
            List<Tile> attackTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], attackRange, true);
            // List<Tile> movementToAttackTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint + attackRange);
            List<Tile> movementTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint + 100000);

            if (attackTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.isOwnedByAI != true && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = attackTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.isOwnedByAI != true && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).First();

                GameManager.instance.removeTileHighlights();
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = false;
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].attacking = true;
                GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
                GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting = true;
            }

            // move toward nearest attack range of opponent
            /* else if (movementToAttackTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
                {
                var opponentsInRange = movementToAttackTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).OrderBy(x => x != null ? TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000).First();

                GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint);

                List<Tile> path = TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
                if (path.Count > 1)
                {
                    GameManager.instance.moveCurrentPlayer(path[(int)Mathf.Max(0, path.Count - 1 - attackRange)]);
                }

            }
         */
            // move toward nearest opponent

            else if (!moving && movementTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.isOwnedByAI != true && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = movementTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.isOwnedByAI != true && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition && y.HP > 0).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).OrderBy(x => x != null ? TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000).First();

                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = true;
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].attacking = false;
                GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false, true);

                List<Tile> path = TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y], GameManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());
                if (path.Count() > 1)
                {
                    List<Tile> actualMovement = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint, GameManager.instance.players.Where(x => x.gridPosition != gridPosition).Select(x => x.gridPosition).ToArray());
                    path.Reverse();
                    if (path.Where(x => actualMovement.Contains(x)).Count() > 0)
                    {
                        GameManager.instance.moveCurrentPlayer(path.Where(x => actualMovement.Contains(x)).First());
                    }
                }
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting = true;
            }

            // first, attack if enemy is in immediate range of being attacked.
            // second, move towards nearest enemy if that is not the case.
            // third, after that move, attack if enemy is now in immediate range of being attacked.
            // this is the third thing:
            if (attackTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.isOwnedByAI != true && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = attackTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.isOwnedByAI != true && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).First();

                GameManager.instance.removeTileHighlights();
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = false;
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].attacking = true;
                GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
                GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting = true;
            }

            // end turn if nothing else
            else
            {
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = false;
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].attacking = false;
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting = true;

                GameManager.instance.numberOfActiveAIUnits -= 1;
            }
      
        // On the first turn, check one more time (for each unit) to make sure units move after their first actions are calculated
        // i know this should be a bool but whatever i dont care
        // TODO: make it so this ONLY happens on the first turn.  
        // right now it's doing the whole shebang, so when the first turn double update is happening, it's actually cycling through
        // the entire process and generating a new gridposition.  should not be doing that just yet. or something.
        if (MustUpdatePath < 1)
        {
            GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting = false;
            GameManager.instance.numberOfActiveAIUnits += 1;

            MustUpdatePath++;
        }
        else if (MustUpdatePath == 1)
        {
            MustUpdatePath = 0;
            EndTurn();
        }/*
        else
        {
            MustUpdatePath = false;

            waiting = false;

            GameManager.instance.numberOfActiveAIUnits = GameManager.instance.aiPlayers.Count;

            GameManager.instance.currentAIUnitIndex = 0;

        }*/
        }

    public void UpdateUnitPosition()
    {
        transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
        {
            transform.position = positionQueue[0];
            positionQueue.RemoveAt(0);
            if (positionQueue.Count == 0)
            {
                inactive = true;
                //EndTurn();
            }
            // GameManager.instance.NextTurn();  
            // (probably best to have something like this for a chesslike but for an AWlike the current code makes more sense)
        }
    }
                
    public override void TurnUpdate()
    {
        GameManager.instance.aiPlayerTurn = true;

        if (positionQueue.Count > 0)
        {
            UpdateUnitPosition();
        }

        // TODO: Turn this into aiPlayerTurn
        else if (GameManager.instance.playerTwoTurn)
        { 
            SetUnitActions();
            if (positionQueue.Count == 0)
            {
                EndTurn();
            }
        }
        base.TurnUpdate();
    }

    public void SelectNextUnit()
    {

    }

    public void EndTurn()
    {
        // august 18, this is not accomplishing what i'm trying to make it accomplish
       // while (positionQueue.Count > 0)
       // {
       //     UpdateUnitPosition();
       // }

        if (GameManager.instance.numberOfActiveAIUnits == 0)
        {
            //if all of the units have been used, go to next turn.
            // i think this causes problems here
            //GameManager.instance.numberOfActiveAIUnits = GameManager.instance.aiPlayers.Count;
            //foreach (Player p in GameManager.instance.aiPlayers)
            //{
            //    p.waiting = false;
            // }

            // play some sort of animation here or something
            GameManager.instance.aiPlayerTurn = false;
            GameManager.instance.numberOfActiveAIUnits = GameManager.instance.aiPlayers.Count;

            GameManager.instance.NextTurn();
        }

        else if (GameManager.instance.numberOfActiveAIUnits > 0)
        {
            // select next unit if there are any left
             if (GameManager.instance.currentAIUnitIndex + 1 < GameManager.instance.aiPlayers.Count)
            {
                GameManager.instance.currentAIUnitIndex++;
            }
            else
            {
                GameManager.instance.currentAIUnitIndex = 0;
                GameManager.instance.aiPlayerTurn = false;
            }
        }
    }
   
    // later i'd like to have this so the gui doesnt exist on ai player turn
    public override void TurnOnGUI()
    {
        int buttonHeight = 30;
        int buttonWidth = 70;

        Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);
        if (GUI.Button(buttonRect, "Attack"))
        {
            // if unit is owned by player whose turn it is, can attack
            if ((GameManager.instance.playerOneTurn && GameManager.myUnit.isOwnedByPlayerOne) || (GameManager.instance.playerTwoTurn && GameManager.myUnit.isOwnedByPlayerTwo))
            {
                if (GameManager.myUnit.attacking == false && GameManager.myUnit.waiting == false)
                {
                    GameManager.instance.removeTileHighlights();
                    GameManager.myUnit.moving = false;
                    GameManager.myUnit.attacking = true;
                    GameManager.myUnit.waiting = false;
                    //GameManager.myUnit.capturing = false;
                    GameManager.instance.highlightTilesAt(GameManager.myUnit.gridPosition, Color.red, GameManager.myUnit.attackRange);

                }
                else
                {

                    GameManager.myUnit.moving = false;
                    GameManager.myUnit.attacking = false;
                   // GameManager.myUnit.capturing = false;
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
        // attack button

        // wait button
        buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);

        if (GUI.Button(buttonRect, "Wait"))
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
            }
        }

        //end turn button (right now just skips to next unit)
        buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);
        if (GUI.Button(buttonRect, "End Turn"))
        {
            GameManager.instance.removeTileHighlights();

            //currently this affects all units, not just the ones you own
            foreach (Player p in GameManager.instance.players)
            {
                if (p != null)
                {
                    p.waiting = false;
                 //   p.actionPoints = 1;
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
        base.TurnOnGUI();
    }
}
