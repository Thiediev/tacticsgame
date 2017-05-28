using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapCreatorManager : MonoBehaviour
{
    public static MapCreatorManager instance;

    public int mapSize;
    public List<List<Tile>> map = new List<List<Tile>>();
    Transform mapTransform;

    public TileType paletteSelection = TileType.Normal;

    void Awake()
    {
        instance = this;
        mapTransform = transform.FindChild("Map");
        generateBlankMap(mapSize);
    }

    void Update()
    {

    }

    void generateBlankMap(int mSize)
    {
        mapSize = mSize;

        // initially remove all children
        for (int i = 0; i < mapTransform.transform.childCount; i++)
        {
            Destroy(mapTransform.GetChild(i).gameObject);
        }

        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.transform.parent = mapTransform;
                tile.gridPosition = new Vector2(i, j);
                tile.setType(TileType.Normal);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    void loadMapFromXml(string s)
    {
        MapXmlContainer container = MapSaveLoad.Load(s);
        mapSize = container.size;

        // initially remove all children
        for (int i = 0; i < mapTransform.childCount; i++)
        {
            Destroy(mapTransform.GetChild(i).gameObject);
        }


        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.transform.parent = mapTransform;
                tile.gridPosition = new Vector2(i, j);
                tile.setType((TileType)container.tiles.Where(x => x.locX == i && x.locY == j).First().id);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    void saveMapToXml(string s)
    {
        MapSaveLoad.Save(MapSaveLoad.CreateMapContainer(map), s);
    }

    public void UIGenerateBlankMap(int mSize)
    {
        mapSize = mSize;

        // initially remove all children
        for (int i = 0; i < mapTransform.transform.childCount; i++)
        {
            Destroy(mapTransform.GetChild(i).gameObject);
        }

        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.transform.parent = mapTransform;
                tile.gridPosition = new Vector2(i, j);
                tile.setType(TileType.Normal);
                row.Add(tile);
            }
            map.Add(row);
        }
    }
    public void UILoadMap(string s)
    {
        MapXmlContainer container = MapSaveLoad.Load(s);
        mapSize = container.size;

        // initially remove all children
        for (int i = 0; i < mapTransform.childCount; i++)
        {
            Destroy(mapTransform.GetChild(i).gameObject);
        }

        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.transform.parent = mapTransform;
                tile.gridPosition = new Vector2(i, j);
                tile.setType((TileType)container.tiles.Where(x => x.locX == i && x.locY == j).First().id);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    public void UISaveMap(string s)
    {
                MapSaveLoad.Save(MapSaveLoad.CreateMapContainer(map), s);
           }

    void OnGUI()
    {

    }

    // MAP UI
    // Save/Load
    public void LoadCampaign1()
    {
       loadMapFromXml("map.xml");
    }

    public void SaveCampaign1()
    {
       saveMapToXml("map.xml");
    }

    public void LoadVersus1()
    {

    }

    public void SaveVersus1()
    {

    }

    public void ClearMap()
    {
        generateBlankMap(mapSize);
    }


    //Units
    // player one
    public void SelectP1Flea()
    {
        paletteSelection = TileType.FleaP1;
    }
    public void SelectP1FleaUpA()
    {

    }
    public void SelectP1FleaUpB()
    {

    }
    public void SelectP1Spider()
    {
        paletteSelection = TileType.SpiderP1;

    }
    public void SelectP1SpiderUpA()
    {

    }
    public void SelectP1SpiderUpB()
    {

    }
    public void SelectP1Witch()
    {
        paletteSelection = TileType.WitchP1;

    }
    public void SelectP1WitchUpA()
    {

    }
    public void SelectP1WitchUpB()
    {

    }

    // player two
    public void SelectP2Flea()
    {
        paletteSelection = TileType.FleaP2;
    }
    public void SelectP2FleaUpA()
    {

    }
    public void SelectP2FleaUpB()
    {

    }
    public void SelectP2Spider()
    {
        paletteSelection = TileType.SpiderP2;

    }
    public void SelectP2SpiderUpA()
    {

    }
    public void SelectP2SpiderUpB()
    {

    }
    public void SelectP2Witch()
    {
        paletteSelection = TileType.WitchP2;
    }
    public void SelectP2WitchUpA()
    {

    }
    public void SelectP2WitchUpB()
    {

    }

    // AI-controlled
    public void SelectAIFlea()
    {
        paletteSelection = TileType.FleaAI;
    }
    public void SelectAIFleaUpA()
    {

    }
    public void SelectAIFleaUpB()
    {

    }
    public void SelectAISpider()
    {
        paletteSelection = TileType.SpiderAI;
    }
    public void SelectAISpiderUpA()
    {

    }
    public void SelectAISpiderUpB()
    {

    }
    public void SelectAIWitch()
    {
        paletteSelection = TileType.WitchAI;
    }
    public void SelectAIWitchUpA()
    {

    }
    public void SelectAIWitchUpB()
    {

    }

    public void TileSelectNormal()
    {

    }

    public void TileSelectP1Base()
    {
       paletteSelection = TileType.BasePlayerOne;
    }

    public void TileSelectP2Base()
    {
      paletteSelection = TileType.BasePlayerTwo;
    }

    public void TileSelectWall()
    {
        paletteSelection = TileType.Impassable;
    }
}
