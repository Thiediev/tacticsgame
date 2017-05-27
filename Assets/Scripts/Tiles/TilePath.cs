using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TilePath
{
    public List<Tile> listOfTiles = new List<Tile>();
    public Tile lastTile;

    public int costOfPath = 0;

    public TilePath()
    {

    }

    public TilePath(TilePath tp)
    {
        listOfTiles = tp.listOfTiles.ToList();
        costOfPath = tp.costOfPath;
        lastTile = tp.lastTile;
        // im not sure what this last tile deal is.  figure that out later i guess
    }



    public void addTile(Tile t)
    {
        costOfPath += t.movementCost;
        listOfTiles.Add(t);
        lastTile = t;
    }

    public void addStaticTile(Tile t)
    {
        costOfPath += 1;
        listOfTiles.Add(t);
        lastTile = t;
    }
}
