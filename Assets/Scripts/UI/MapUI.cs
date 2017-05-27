using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MapUI : MonoBehaviour {
    // various info such as turn counter, player funds, power meter (if there is any), etc


    public static MapUI instance;

    private Text turnTxtRef;



    [SerializeField]
    private Text turnCountText;



    private void Awake()
    {
        turnTxtRef = GetComponent<Text>();//or provide from somewhere else (e.g. if you want via find GameObject.Find("CountText").GetComponent<Text>();)

        //unitRef = GetComponent<Unit>();
    }
    //then where you need:
    private void Start()
    {
        turnCountText.text = "Turn 1";
    }
    
    private void Update()
    {
        // Switch it
        UpdateTurnCounter();
        /*
        if (GameManager.instance.turnCount == 1)
            turnCountText.text = "Turn 1";
        else if (GameManager.instance.turnCount == 2)
            turnCountText.text = "Turn 2";
            */
    }

  

    // if 
    public void UpdateTurnCounter()
    {

        // Switch it
        if (GameManager.instance.turnCount == 1)
            turnCountText.text = "Turn 1";
        else if (GameManager.instance.turnCount == 2)
            turnCountText.text = "Turn 2";
        else if (GameManager.instance.turnCount == 3)
            turnCountText.text = "Turn 3";
        else if (GameManager.instance.turnCount == 4)
            turnCountText.text = "Turn 4";
        else if (GameManager.instance.turnCount == 5)
            turnCountText.text = "Turn 5";
        else if (GameManager.instance.turnCount == 6)
            turnCountText.text = "Turn 6";
        else if (GameManager.instance.turnCount == 7)
            turnCountText.text = "Turn 7";
        else if (GameManager.instance.turnCount == 8)
            turnCountText.text = "Turn 8";
        else if (GameManager.instance.turnCount == 9)
            turnCountText.text = "Turn 9";
        else if (GameManager.instance.turnCount == 10)
            turnCountText.text = "Turn 10";
        else if (GameManager.instance.turnCount == 11)
            turnCountText.text = "Turn 11";
        else if (GameManager.instance.turnCount == 12)
            turnCountText.text = "Turn 12";
        else if (GameManager.instance.turnCount == 13)
            turnCountText.text = "Turn 13";
        else if (GameManager.instance.turnCount == 14)
            turnCountText.text = "Turn 14";
        else if (GameManager.instance.turnCount == 15)
            turnCountText.text = "Turn 15";
        else if (GameManager.instance.turnCount == 16)
            turnCountText.text = "Turn 16";
        else if (GameManager.instance.turnCount == 17)
            turnCountText.text = "Turn 17";
        else if (GameManager.instance.turnCount == 18)
            turnCountText.text = "Turn 18";
        else if (GameManager.instance.turnCount == 19)
            turnCountText.text = "Turn 19";
        else if (GameManager.instance.turnCount == 20)
            turnCountText.text = "Turn 20";
        else if (GameManager.instance.turnCount == 21)
            turnCountText.text = "Turn 21";
        else if (GameManager.instance.turnCount == 22)
            turnCountText.text = "Turn 22";
        else if (GameManager.instance.turnCount == 23)
            turnCountText.text = "Turn 23";
        else if (GameManager.instance.turnCount == 24)
            turnCountText.text = "Turn 24";
        else if (GameManager.instance.turnCount == 25)
            turnCountText.text = "Turn 25";
        else if (GameManager.instance.turnCount == 26)
            turnCountText.text = "Turn 26";
        else if (GameManager.instance.turnCount == 27)
            turnCountText.text = "Turn 27";
        else if (GameManager.instance.turnCount == 28)
            turnCountText.text = "Turn 28";
        else if (GameManager.instance.turnCount == 29)
            turnCountText.text = "Turn 29";
        else if (GameManager.instance.turnCount == 30)
            turnCountText.text = "Turn 30";
        else if (GameManager.instance.turnCount == 31)
            turnCountText.text = "Turn 31";
        else if (GameManager.instance.turnCount == 32)
            turnCountText.text = "Turn 32";
        else if (GameManager.instance.turnCount == 33)
            turnCountText.text = "Turn 33";
        else if (GameManager.instance.turnCount == 34)
            turnCountText.text = "Turn 34";
        else if (GameManager.instance.turnCount == 35)
            turnCountText.text = "Turn 35";
        else if (GameManager.instance.turnCount == 36)
            turnCountText.text = "Turn 36";
        else if (GameManager.instance.turnCount == 37)
            turnCountText.text = "Turn 37";
        else if (GameManager.instance.turnCount == 38)
            turnCountText.text = "Turn 38";
        else if (GameManager.instance.turnCount == 39)
            turnCountText.text = "Turn 39";
        else if (GameManager.instance.turnCount == 40)
            turnCountText.text = "Turn 40";
        else if (GameManager.instance.turnCount == 41)
            turnCountText.text = "Turn 41";
        else if (GameManager.instance.turnCount == 42)
            turnCountText.text = "Turn 42";
        else if (GameManager.instance.turnCount == 43)
            turnCountText.text = "Turn 43";
        else if (GameManager.instance.turnCount == 44)
            turnCountText.text = "Turn 44";
        else if (GameManager.instance.turnCount == 45)
            turnCountText.text = "Turn 45";
        else if (GameManager.instance.turnCount == 46)
            turnCountText.text = "Turn 46";
        else if (GameManager.instance.turnCount == 47)
            turnCountText.text = "Turn 47";
        else if (GameManager.instance.turnCount == 48)
            turnCountText.text = "Turn 48";
        else if (GameManager.instance.turnCount == 49)
            turnCountText.text = "Turn 49";
        else if (GameManager.instance.turnCount == 50)
            turnCountText.text = "Turn 50";
        else if (GameManager.instance.turnCount == 51)
            turnCountText.text = "Turn 51";
        else if (GameManager.instance.turnCount == 52)
            turnCountText.text = "Turn 52";
        else if (GameManager.instance.turnCount == 53)
            turnCountText.text = "Turn 53";
        else if (GameManager.instance.turnCount == 54)
            turnCountText.text = "Turn 54";
        else if (GameManager.instance.turnCount == 55)
            turnCountText.text = "Turn 55";
        else if (GameManager.instance.turnCount == 56)
            turnCountText.text = "Turn 56";
        else if (GameManager.instance.turnCount == 57)
            turnCountText.text = "Turn 57";
        else if (GameManager.instance.turnCount == 58)
            turnCountText.text = "Turn 58";
        else if (GameManager.instance.turnCount == 59)
            turnCountText.text = "Turn 59";
        else if (GameManager.instance.turnCount == 60)
            turnCountText.text = "Turn 60";
        else if (GameManager.instance.turnCount == 61)
            turnCountText.text = "Turn 61";
        else if (GameManager.instance.turnCount == 62)
            turnCountText.text = "Turn 62";
        else if (GameManager.instance.turnCount == 63)
            turnCountText.text = "Turn 63";
        else if (GameManager.instance.turnCount == 64)
            turnCountText.text = "Turn 64";
        else if (GameManager.instance.turnCount == 65)
            turnCountText.text = "Turn 65";
        else if (GameManager.instance.turnCount == 66)
            turnCountText.text = "Turn 66";
    }
    /*
    if (HPForDisplay < 10)
    {
        txtRef.text = "8";
    }    */
}
