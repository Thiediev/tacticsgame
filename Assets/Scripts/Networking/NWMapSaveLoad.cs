using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class NWTileXml
{
    [XmlAttribute("id")]
    public int id;

    [XmlAttribute("locX")]
    public int locX;
    
    [XmlAttribute("locY")]
    public int locY;

}

[XmlRoot("MapCollection")]
public class NWMapXmlContainer
{
    [XmlAttribute("size")]
    public int size;

    [XmlArray("Tiles")]
    [XmlArrayItem("Tile")]
    public List<NWTileXml> tiles = new List<NWTileXml>();
}

public class NWMapSaveLoad
{
    public static NWMapXmlContainer CreateMapContainer(List<List<NWTile>> map)
    {


        List<NWTileXml> tiles = new List<NWTileXml>();
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map.Count; j++)
            {
                tiles.Add(NWMapSaveLoad.CreateTileXml(map[i][j]));
            }
        }

        return new NWMapXmlContainer()
        {
            size = map.Count,
            tiles = tiles
        };
    }

    public static NWTileXml CreateTileXml(NWTile tile)
    {
        return new NWTileXml()
        {
            id = (int)tile.type,
            locX = (int)tile.gridPosition.x,
            locY = (int)tile.gridPosition.y
        };

    }

    public static void Save(NWMapXmlContainer mapContainer, string filename)
    {
        var serializer = new XmlSerializer(typeof(MapXmlContainer));
        using (var stream = new FileStream(filename, FileMode.Create))
        {
            serializer.Serialize(stream, mapContainer);
        }
    }

    public static NWMapXmlContainer Load(string filename)
    {
        var serializer = new XmlSerializer(typeof(NWMapXmlContainer));
        using (var stream = new FileStream(filename, FileMode.Open))
        {
            return serializer.Deserialize(stream) as NWMapXmlContainer;
        }
    }


}
