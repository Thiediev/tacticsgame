using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NWTileHighlight
{



    public NWTileHighlight()
    {

    }

    public static List<NWTile> FindHighlight(NWTile originTile, int movementPoints)
    {
        return FindHighlight(originTile, movementPoints, new Vector2[0], false);
    }
    public static List<NWTile> FindHighlight(NWTile originTile, int movementPoints, bool staticRange)
    {
        return FindHighlight(originTile, movementPoints, new Vector2[0], staticRange);
    }

    public static List<NWTile> FindHighlight(NWTile originTile, int movementPoints, Vector2[] occupied)
    {
        return FindHighlight(originTile, movementPoints, occupied, false);
    }

    public static List<NWTile> FindHighlight(NWTile originTile, int movementPoints, Vector2[] occupied, bool staticRange)
    {
        List<NWTile> closed = new List<NWTile>();
        List<NWTilePath> open = new List<NWTilePath>();


        NWTilePath originPath = new NWTilePath();
        if (staticRange)
        {
            originPath.addStaticTile(originTile);


        }
        else
        {
            originPath.addTile(originTile);
        }

        open.Add(originPath);

        while (open.Count > 0)
        {
            NWTilePath current = open[0];
            open.Remove(open[0]);

            if (closed.Contains(current.lastTile))
            {
                continue;
            }
            // there may be some stuff here inconsistent with how i was told to do it so check on that later if you have issues
            if (current.costOfPath > movementPoints + 1)
            {
                continue;
            }

            closed.Add(current.lastTile);

            foreach (NWTile t in current.lastTile.neighbors)
            {
                if (

                    //t.impassable || 
                    occupied.Contains(t.gridPosition))
                {
                    continue;
                }
                NWTilePath newTilePath = new NWTilePath(current);
                if (staticRange)
                {
                    newTilePath.addStaticTile(t);


                }
                else
                {
                    newTilePath.addTile(t);
                }
                open.Add(newTilePath);
            }

        }
        closed.Remove(originTile);
        // closed.Remove(alliedUnitTiles), which is something we'll have to define and add to the method that determines that stuff,
        // just like it determines that we can't move past occupied tiles
        // we'll have to hope this doesn't obliterate pathing.  
        // if it does i'm not sure what to do yet
        closed.Distinct();
        return closed;

    }
}

