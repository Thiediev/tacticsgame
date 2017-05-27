using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


public class NWHPDisplay : MonoBehaviour
{ 

    public static NWHPDisplay instance;

    private int HPForDisplay;
    public NWUnit thisUnit = instance.transform.parent.transform.GetComponent<NWUnit>();


    [SerializeField]
    private Text txtRef;


    private void Awake()
    {
        txtRef = GetComponent<Text>();//or provide from somewhere else (e.g. if you want via find GameObject.Find("CountText").GetComponent<Text>();)
        //unitRef = GetComponent<Unit>();
    }
    //then where you need:
    private void Start()
    {
        HPForDisplay = 10;
        txtRef.text = "";
    }

    private void Update()
    {
        UpdateHPIndicator();
    }

    public void UpdateHPForDisplay(int i)
    {
        instance.HPForDisplay = i;
    }

    // if 
    public void UpdateHPIndicator()
    {

        // Switch it
        if (thisUnit.HP < 10 && thisUnit.HP > 0)
        {
            if (thisUnit.HP == 1)
                txtRef.text = "1";
            else if (thisUnit.HP == 2)
                txtRef.text = "2";
            else if (thisUnit.HP == 3)
                txtRef.text = "3";
            else if (thisUnit.HP == 4)
                txtRef.text = "4";
            else if (thisUnit.HP == 5)
                txtRef.text = "5";
            else if (thisUnit.HP == 6)
                txtRef.text = "6";
            else if (thisUnit.HP == 7)
                txtRef.text = "7";
            else if (thisUnit.HP == 8)
                txtRef.text = "8";
            else if (thisUnit.HP == 9)
                txtRef.text = "9";
        }
    }
    /*
    if (HPForDisplay < 10)
    {
        txtRef.text = "8";
    }    */
}

