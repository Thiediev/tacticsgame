using UnityEngine;
using System.Collections;

public enum TileType {
    // base tiles
    Normal,
    Difficult,
    VeryDifficult,
    Impassable,

    // Days of Ruin
    //
    Road,
    Plains,
    Forest,
    Mountain,
    River,
    Sea,
    Shoal,
    // DoR no units
    CityNeutral,
    CityPlayerOne,
    CityPlayerTwo, 
    // DoR build land units 
    BaseNeutral,
    BasePlayerOne,
    BasePlayerTwo,
    // DoR build air units
    AirportNeutral,
    AirportPlayerOne,
    AirportPlayerTwo,
    // DoR build sea units
    PortNeutral,
    PortPlayerOne,
    PortPlayerTwo,
  // there's some other stuff like meteors or something
    HQPlayerOne,
    HQPlayerTwo,


        // tiles for predeployed units
        FleaP1,
        WitchP1,
        SpiderP1,
        HQP1,

        FleaP2,
        WitchP2,
        SpiderP2,
        HQP2,

        FleaAI,
        WitchAI,
        SpiderAI
        	
}
