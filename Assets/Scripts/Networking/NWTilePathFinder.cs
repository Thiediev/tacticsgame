using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NWTilePathFinder : MonoBehaviour
{

    public static List<NWTile> FindPath(NWTile originTile, NWTile destinationTile)
    {
        return FindPath(originTile, destinationTile, new Vector2[0]);
    }

    public static List<NWTile> FindPath(NWTile originTile, NWTile destinationTile, Vector2[] occupied)
    {
        List<NWTile> closed = new List<NWTile>();
        List<NWTilePath> open = new List<NWTilePath>();


        NWTilePath originPath = new NWTilePath();
        originPath.addTile(originTile);

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

            if (current.lastTile == destinationTile)
            {
                current.listOfTiles.Remove(originTile);
                return current.listOfTiles;
            }

            closed.Add(current.lastTile);

            foreach (NWTile t in current.lastTile.neighbors)
            {
                if (t.impassable || occupied.Contains(t.gridPosition))
                {
                    continue;
                }
                NWTilePath newTilePath = new NWTilePath(current);
                newTilePath.addTile(t);
                open.Add(newTilePath);
            }
        }
        return null;
    }
}
