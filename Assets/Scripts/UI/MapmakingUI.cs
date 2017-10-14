using UnityEngine;
using System.Collections;

public class MapmakingUI : MonoBehaviour {
    

    
    // Load
    public void LoadCampaign1()
    {
        MapCreatorManager.instance.UILoadMap("map.xml");
    }
    public void LoadCampaign2()
    {
        MapCreatorManager.instance.UILoadMap("map2.xml");
    }
    public void LoadCampaign3()
    {
        MapCreatorManager.instance.UILoadMap("map3.xml");
    }
    public void LoadCampaign4()
    {
        MapCreatorManager.instance.UILoadMap("map4.xml");
    }
    public void LoadCampaign5()
    {
        MapCreatorManager.instance.UILoadMap("map5.xml");
    }
    public void LoadCampaign6()
    {
        MapCreatorManager.instance.UILoadMap("map6.xml");
    }
    public void LoadCampaign7()
    {
        MapCreatorManager.instance.UILoadMap("map7.xml");
    }
    public void LoadCampaign8()
    {
        MapCreatorManager.instance.UILoadMap("map8.xml");
    }
    public void LoadCampaign9()
    {
        MapCreatorManager.instance.UILoadMap("map9.xml");
    }
    public void LoadCampaign10()
    {
        MapCreatorManager.instance.UILoadMap("map10.xml");
    }
    public void LoadCampaign11()
    {
        MapCreatorManager.instance.UILoadMap("map11.xml");
    }
    public void LoadCampaign12()
    {
        MapCreatorManager.instance.UILoadMap("map12.xml");
    }
    public void LoadCampaign13()
    {
        MapCreatorManager.instance.UILoadMap("map13.xml");
    }
    public void LoadCampaign14()
    {
        MapCreatorManager.instance.UILoadMap("map14.xml");
    }
    public void LoadCampaign15()
    {
        MapCreatorManager.instance.UILoadMap("map15.xml");
    }
    public void LoadCampaign16()
    {
        MapCreatorManager.instance.UILoadMap("map16.xml");
    }

    //Save
    public void SaveCampaign1()
    {
        MapCreatorManager.instance.UISaveMap("map.xml");
    }
    public void SaveCampaign2()
    {
        MapCreatorManager.instance.UISaveMap("map2.xml");
    }
    public void SaveCampaign3()
    {
        MapCreatorManager.instance.UISaveMap("map3.xml");
    }
    public void SaveCampaign4()
    {
        MapCreatorManager.instance.UISaveMap("map4.xml");
    }
    public void SaveCampaign5()
    {
        MapCreatorManager.instance.UISaveMap("map5.xml");
    }
    public void SaveCampaign6()
    {
        MapCreatorManager.instance.UISaveMap("map6.xml");
    }
    public void SaveCampaign7()
    {
        MapCreatorManager.instance.UISaveMap("map7.xml");
    }
    public void SaveCampaign8()
    {
        MapCreatorManager.instance.UISaveMap("map8.xml");
    }
    public void SaveCampaign9()
    {
        MapCreatorManager.instance.UISaveMap("map9.xml");
    }
    public void SaveCampaign10()
    {
        MapCreatorManager.instance.UISaveMap("map10.xml");
    }
    public void SaveCampaign11()
    {
        MapCreatorManager.instance.UISaveMap("map11.xml");
    }
    public void SaveCampaign12()
    {
        MapCreatorManager.instance.UISaveMap("map12.xml");
    }
    public void SaveCampaign13()
    {
        MapCreatorManager.instance.UISaveMap("map13.xml");
    }
    public void SaveCampaign14()
    {
        MapCreatorManager.instance.UISaveMap("map14.xml");
    }
    public void SaveCampaign15()
    {
        MapCreatorManager.instance.UISaveMap("map15.xml");
    }
    public void SaveCampaign16()
    {
        MapCreatorManager.instance.UISaveMap("map16.xml");
    }

    public void LoadVersus1()
    {
        MapCreatorManager.instance.UISaveMap("versus1.xml");
    }
    public void LoadVersus2()
    {
        MapCreatorManager.instance.UISaveMap("versus2.xml");
    }
    public void LoadVersus3()
    {
        MapCreatorManager.instance.UISaveMap("versus3.xml");
    }

    public void SaveVersus1()
    {
        MapCreatorManager.instance.UISaveMap("versus1.xml");
    }
    public void SaveVersus2()
    {
        MapCreatorManager.instance.UISaveMap("versus2.xml");
    }
    public void SaveVersus3()
    {
        MapCreatorManager.instance.UISaveMap("versus3.xml");
    }

    public void ClearMap()
    {
        // have something where it prompts you to input the map size here
        MapCreatorManager.instance.UIGenerateBlankMap(MapCreatorManager.instance.mapSize);
    }


    //Units
    // player one
    public void SelectP1Flea()
    {
        MapCreatorManager.instance.paletteSelection = TileType.FleaP1;
    }
    public void SelectP1FleaUpA()
    {

    }
    public void SelectP1FleaUpB()
    {

    }
    public void SelectP1Spider()
    {
        MapCreatorManager.instance.paletteSelection = TileType.SpiderP1;
    }
    public void SelectP1SpiderUpA()
    {

    }
    public void SelectP1SpiderUpB()
    {

    }
    public void SelectP1Witch()
    {
        MapCreatorManager.instance.paletteSelection = TileType.WitchP1;
    }
    public void SelectP1WitchUpA()
    {

    }
    public void SelectP1WitchUpB()
    {

    }
public void SelectP1HQ()
    {
        MapCreatorManager.instance.paletteSelection = TileType.HQP1;
    }

    // player two
    public void SelectP2Flea()
    {
        MapCreatorManager.instance.paletteSelection = TileType.FleaP2;
    }
    public void SelectP2FleaUpA()
    {

    }
    public void SelectP2FleaUpB()
    {

    }
    public void SelectP2Spider()
    {
        MapCreatorManager.instance.paletteSelection = TileType.SpiderP2;
    }
    public void SelectP2SpiderUpA()
    {

    }
    public void SelectP2SpiderUpB()
    {

    }
    public void SelectP2Witch()
    {
        MapCreatorManager.instance.paletteSelection = TileType.WitchP2;
    }
    public void SelectP2WitchUpA()
    {

    }
    public void SelectP2WitchUpB()
    {

    }
    public void SelectP2HQ()
    {
        MapCreatorManager.instance.paletteSelection = TileType.HQP2;
    }

    // AI-controlled
    public void SelectAIFlea()
    {
        MapCreatorManager.instance.paletteSelection = TileType.FleaAI;
    }
    public void SelectAIFleaUpA()
    {

    }
    public void SelectAIFleaUpB()
    {

    }
    public void SelectAISpider()
    {
        MapCreatorManager.instance.paletteSelection = TileType.SpiderAI;
    }
    public void SelectAISpiderUpA()
    {

    }
    public void SelectAISpiderUpB()
    {

    }
    public void SelectAIWitch()
    {
        MapCreatorManager.instance.paletteSelection = TileType.WitchAI;
    }
    public void SelectAIWitchUpA()
    {

    }
    public void SelectAIWitchUpB()
    {

    }
    
    //Tiles
    public void TileSelectNormal()
    {

    }

    public void TileSelectP1Base()
    {
        MapCreatorManager.instance.paletteSelection = TileType.BasePlayerOne;
    }

    public void TileSelectP2Base()
    {
        MapCreatorManager.instance.paletteSelection = TileType.BasePlayerTwo;
    }

    public void TileSelectWall()
    {
        MapCreatorManager.instance.paletteSelection = TileType.Impassable;
    }
}