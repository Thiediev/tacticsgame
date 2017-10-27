using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIPlayerFix : Player
{
    // Set Unit Movement
    // Update the unit's position 
    // Set Unit Actions (attack etc)
    // Select Next Unit
    // End Turn when all units are finished

    public bool MovementHasBeenSet = false;
    public bool AttackHasCompleted = false; // could be replaced with Wait
    public bool MovementHasBeenCompleted = false;

    void Start()
    {
        GameManager.instance.numberOfActiveAIUnits = GameManager.instance.aiPlayers.Count;
    }

    public override void Update()
    {
        // for now we're assuming if it's player two's turn and there are AI units on the map, then it's the AI's turn
        if (GameManager.instance.playerTwoTurn)
        {
            //human player cannot act while AI player acts
            GameManager.instance.DeactivateAButton();
            GameManager.instance.MainUICanvas.transform.position = new Vector3(-20, 0, 0);


            //pause between moving each unit
            if (GameManager.instance.resumeIn3Seconds == true)
            {
                if (GameManager.instance.startedPausing == true)
                {
                    GameManager.instance.ResumeIn3Seconds();
                }
            } else
            if (GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex] != null && GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].HP > 0)
            {
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].TurnUpdate();
            }
            else if (GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex] == null || GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].HP <= 0)
            {
                // +1 to offset the -1 in SelectNextUnit
                GameManager.instance.numberOfActiveAIUnits = GameManager.playerAICount + 1;
                SelectNextUnit();
            }
        }
        base.Update();
    }

    public override void TurnUpdate()
    {
        // make sure active AI units don't include any dead units
        if (GameManager.instance.aiPlayerTurn == false)
        {
            GameManager.instance.numberOfActiveAIUnits = GameManager.playerAICount;
            GameManager.instance.aiPlayerTurn = true;
        }

        if (MovementHasBeenSet == false)
        {
            if (GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex] == null)
            {
                SelectNextUnit();
            }
            if (GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].fleaMovementPoints > 0)
            {
                SetMovementOrders();
            }
        }
       
        if (positionQueue.Count > 0)
        {
            UpdateUnitPosition();
        }
        if (positionQueue.Count == 0)
        {
            GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting = true;
            MovementHasBeenCompleted = true;
        }

        if (MovementHasBeenCompleted == true)
        {
            if (GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].fleaActionPoints > 0)
            {
                AttackIfPossible();
            }
            // if there are more units to choose
            if (GameManager.instance.numberOfActiveAIUnits > 0 && GameManager.instance.unitIsDead == false)
            {
                SelectNextUnit();
            }
            else if (GameManager.instance.unitIsDead == true)
            {
                GameManager.instance.unitIsDead = false;
            }
            // if not
            if (GameManager.instance.numberOfActiveAIUnits == 0)
            {
                //human player regains control
                GameManager.instance.ReactivateAButton();
                GameManager.instance.MainUICanvas.transform.position = new Vector3(0, 0, 0);

                EndTurn();
            }
        }
        base.TurnUpdate();
    }

    public void SetMovementOrders()
    {
        List<Tile> movementTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint + 100000);

        //heal first i think?
        if (this.isHealer)
        { 
            if ( !moving && movementTilesInRange.Where(x => GameManager.instance.players.Where(y => y.isOwnedByAI == true && y.HP > 0 && y.HP < 9 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
        {
            var opponentsInRange = movementTilesInRange.Select(x => GameManager.instance.players.Where(y => y.isOwnedByAI == true && y.HP > 0 && y.HP < 9 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition && y.HP > 0).First() : null).ToList();
            Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).OrderBy(x => x != null ? TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000).First();

            GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = true;
            GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].attacking = false;
            GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false, true);
                
            List<Tile> path = TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y], GameManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());

                if (path == null)
                {
                    GameManager.instance.removeTileHighlights();
                   
                    GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = false;
                }
                else
           if (path.Count() > 1)
                {
                    List<Tile> actualMovement = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint, GameManager.instance.players.Where(x => x.gridPosition != gridPosition).Select(x => x.gridPosition).ToArray());
                    path.Reverse();
                    if (path.Where(x => actualMovement.Contains(x)).Count() > 0)
                    {
                        GameManager.instance.moveCurrentPlayer(path.Where(x => actualMovement.Contains(x)).First());

                    }
                }
            }
        }

        // after healing (or if no healing)
        else
            if (!moving && movementTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayerFix) && y.isOwnedByAI != true && y.inStorage != true && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
        {
            var opponentsInRange = movementTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayerFix) && y.isOwnedByAI != true && y.inStorage != true && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition && y.HP > 0).First() : null).ToList();
            Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).OrderBy(x => x != null ? TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000).First();

            GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = true;
            GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].attacking = false;
            GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false, true);

            List<Tile> path = TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y], GameManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());

            if (path == null)
            {
                GameManager.instance.removeTileHighlights();
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = false;
            }
            else
            if (path.Count() > 1)
            {
                List<Tile> actualMovement = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint, GameManager.instance.players.Where(x => x.gridPosition != gridPosition).Select(x => x.gridPosition).ToArray());
                path.Reverse();
                if (path.Where(x => actualMovement.Contains(x)).Count() > 0)
                {
                    GameManager.instance.moveCurrentPlayer(path.Where(x => actualMovement.Contains(x)).First());
                }
            }
            MovementHasBeenSet = true;
            // i think we want this in updateunitposition GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].waiting = true;
        }
    }

    public void UpdateUnitPosition()
    {
        transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
        {
            transform.position = positionQueue[0];
            positionQueue.RemoveAt(0);
        }
    }

    public void AttackIfPossible()
    {
        // this is an attempt on a workaround of the problem where units that die on counter fuck up the unit index
        GameManager.instance.diedOnCounter = false;
       
        List<Tile> attackTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], attackRange, true);

        // trying to make healing work for AI
        if (this.isHealer)
        {
            if (attackTilesInRange.Where(x => GameManager.instance.players.Where(y => y.isOwnedByAI == true && y.HP > 0 && y.HP < 9 && 
            y != null && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = attackTilesInRange.Select(x => GameManager.instance.players.Where(y => y.HP > 0 && y.HP < 9 && y != this && y.isOwnedByAI == true && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).First();

                GameManager.instance.removeTileHighlights();

                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = false;
                GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].attacking = true;
                GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
                GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
            }
        }
        // after healing
        else
            if (attackTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayerFix) && y.inStorage != true && y.isOwnedByAI != true && y.HP > 0 && y != null && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
        {
            var opponentsInRange = attackTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayerFix) && y.inStorage != true && y.HP > 0 && y != this && y.isOwnedByAI != true && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
            Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).First();

            GameManager.instance.removeTileHighlights();

            GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].moving = false;
            GameManager.instance.aiPlayers[GameManager.instance.currentAIUnitIndex].attacking = true;
            GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
            GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
            
            if (GameManager.instance.diedOnCounter == true)
            {
                GameManager.instance.diedOnCounter = false;
                GameManager.instance.unitIsDead = true;
                SelectNextUnit();
            }
        }
        AttackHasCompleted = true;
    }

    public void SelectNextUnit()
    {
        // select next unit if there are any left
        if (GameManager.instance.currentAIUnitIndex + 1 < GameManager.instance.aiPlayers.Count) 
        {
            MovementHasBeenSet = false;
            MovementHasBeenCompleted = false;
            GameManager.instance.numberOfActiveAIUnits--;
            GameManager.instance.currentAIUnitIndex++;
            GameManager.instance.resumeIn3Seconds = true;
            GameManager.instance.startedPausing = true;
        }
        else
        {
            MovementHasBeenSet = false;
            MovementHasBeenCompleted = false;
            GameManager.instance.numberOfActiveAIUnits--;
            GameManager.instance.currentAIUnitIndex = 0;
            //GameManager.instance.aiPlayerTurn = false;
        }
    }

    public void EndTurn()
    {
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

        GameManager.instance.aiPlayerTurn = false;
        MovementHasBeenSet = false;
        AttackHasCompleted = false;
        GameManager.instance.numberOfActiveAIUnits = GameManager.playerAICount;

        GameManager.instance.NextTurn();
    }

    // later i'd like to have this so the gui doesnt exist on ai player turn
    public override void TurnOnGUI()
    {
        base.TurnOnGUI();
    }
}