using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPDisplayAI : MonoBehaviour
{

    public static HPDisplayAI instance;

    private int HPForDisplay;
    public AIPlayerFix thisAIUnit = instance.transform.parent.transform.GetComponent<AIPlayerFix>();

    [SerializeField]
    private Text HPTxtRef;

    private void Awake()
    {
        HPTxtRef = GetComponent<Text>();//or provide from somewhere else (e.g. if you want via find GameObject.Find("CountText").GetComponent<Text>();)
        //unitRef = GetComponent<Unit>();
    }
    //then where you need:
    private void Start()
    {
        HPForDisplay = 10;
        HPTxtRef.text = "";
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

        HPForDisplay = thisAIUnit.HP;
        if (thisAIUnit.HP < 10)
            HPTxtRef.text = HPForDisplay.ToString();
    }
}
