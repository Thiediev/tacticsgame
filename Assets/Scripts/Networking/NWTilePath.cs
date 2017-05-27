using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NWTilePath
{
    public List<NWTile> listOfTiles = new List<NWTile>();
    public NWTile lastTile;

    public int costOfPath = 0;

    public NWTilePath()
    {

    }

    public NWTilePath(NWTilePath tp)
    {
        listOfTiles = tp.listOfTiles.ToList();
        costOfPath = tp.costOfPath;
        lastTile = tp.lastTile;
        // im not sure what this last tile deal is.  figure that out later i guess
    }



    public void addTile(NWTile t)
    {
        costOfPath += t.movementCost;
        listOfTiles.Add(t);
        lastTile = t;
    }

    public void addStaticTile(NWTile t)
    {
        costOfPath += 1;
        listOfTiles.Add(t);
        lastTile = t;
    }
}
