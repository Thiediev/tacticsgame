using UnityEngine;
using System.Collections;

public class PrefabHolder : MonoBehaviour {
    public static PrefabHolder instance;

    public GameObject BASE_TILE_PREFAB;

    public GameObject TILE_NORMAL_PREFAB;
    public GameObject TILE_DIFFICULT_PREFAB;
    public GameObject TILE_VERY_DIFFICULT_PREFAB;
    public GameObject TILE_IMPASSABLE_PREFAB;

    public GameObject HIGHLIGHT_TILE;
    public GameObject AI_HIGHLIGHT;

    // DoR tiles
    public GameObject TILE_ROAD_PREFAB;
    public GameObject TILE_PLAINS_PREFAB;
    public GameObject TILE_FOREST_PREFAB;
    public GameObject TILE_MOUNTAIN_PREFAB;
    public GameObject TILE_RIVER_PREFAB;
    public GameObject TILE_SEA_PREFAB;
    public GameObject TILE_REEF_PREFAB;
    public GameObject TILE_SHOAL_PREFAB;
    public GameObject TILE_CITY_NEUTRAL_PREFAB;
    public GameObject TILE_CITY_PLAYER_ONE_PREFAB;
    public GameObject TILE_CITY_PLAYER_TWO_PREFAB;
    public GameObject TILE_BASE_NEUTRAL_PREFAB;
    public GameObject TILE_BASE_PLAYER_ONE_PREFAB;
    public GameObject TILE_BASE_PLAYER_TWO_PREFAB;
    public GameObject TILE_AIRPORT_NEUTRAL_PREFAB;
    public GameObject TILE_AIRPORT_PLAYER_ONE_PREFAB;
    public GameObject TILE_AIRPORT_PLAYER_TWO_PREFAB;
    public GameObject TILE_PORT_NEUTRAL_PREFAB;
    public GameObject TILE_PORT_PLAYER_ONE_PREFAB;
    public GameObject TILE_PORT_PLAYER_TWO_PREFAB;
    public GameObject TILE_HQ_PLAYER_ONE_PREFAB;
    public GameObject TILE_HQ_PLAYER_TWO_PREFAB;

    // predeployed unit tiles
    public GameObject TILE_P1FLEA_PREFAB;
    public GameObject TILE_P2FLEA_PREFAB;
    public GameObject TILE_AIFLEA_PREFAB;

    public GameObject TILE_P1SPIDER_PREFAB;
    public GameObject TILE_P2SPIDER_PREFAB;
    public GameObject TILE_AISPIDER_PREFAB;

    public GameObject TILE_P1WITCH_PREFAB;
    public GameObject TILE_P2WITCH_PREFAB;
    public GameObject TILE_AIWITCH_PREFAB;

    //menus
   /* // Menus
    public GameObject MapmakerCanvas;
    public GameObject UnitsCanvas;
    public GameObject TerrainCanvas;
    public GameObject SaveLoadCanvas;
    */

    void Awake()
    {
        instance = this;
    }
}
