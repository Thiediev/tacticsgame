using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPDisplayAI : MonoBehaviour
{

    public static HPDisplayAI instance;

    private int HPForDisplay;
    public AIPlayerFix thisAIUnit = instance.transform.parent.transform.GetComponent<AIPlayerFix>();

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

    public void UpdateHPIndicator()
    {
        if (thisAIUnit.HP < 10 && thisAIUnit.HP > 0)
        {
            if (thisAIUnit.HP == 1)
                txtRef.text = "1";
            else if (thisAIUnit.HP == 2)
                txtRef.text = "2";
            else if (thisAIUnit.HP == 3)
                txtRef.text = "3";
            else if (thisAIUnit.HP == 4)
                txtRef.text = "4";
            else if (thisAIUnit.HP == 5)
                txtRef.text = "5";
            else if (thisAIUnit.HP == 6)
                txtRef.text = "6";
            else if (thisAIUnit.HP == 7)
                txtRef.text = "7";
            else if (thisAIUnit.HP == 8)
                txtRef.text = "8";
            else if (thisAIUnit.HP == 9)
                txtRef.text = "9";
        }
    }
}
