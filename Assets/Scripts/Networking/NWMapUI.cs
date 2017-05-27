using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NWMapUI : NetworkBehaviour
{
    // various info such as turn counter, player funds, power meter (if there is any), etc


    public static NWMapUI instance;

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
        if (NWGameManager.instance.turnCount == 1)
            turnCountText.text = "Turn 1";
        else if (NWGameManager.instance.turnCount == 2)
            turnCountText.text = "Turn 2";
    }



    // if 
    public void UpdateTurnCounter()
    {

        // Switch it
        if (NWGameManager.instance.turnCount == 1)
            turnCountText.text = "Turn 1";
        else if (NWGameManager.instance.turnCount == 2)
            turnCountText.text = "Turn 2";
    }
    /*
    if (HPForDisplay < 10)
    {
        txtRef.text = "8";
    }    */
}
