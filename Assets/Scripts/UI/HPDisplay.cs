using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class HPDisplay : MonoBehaviour
{

    public static HPDisplay instance;

    private int HPForDisplay;
    public Unit thisUnit = instance.transform.parent.transform.GetComponent<Unit>();

    [SerializeField]
    private Text HPTxtRef;

    private void Awake()
    {
        HPTxtRef = GetComponent<Text>();
    }

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
        HPForDisplay = thisUnit.HP;
        if (thisUnit.HP < 10)
        HPTxtRef.text = HPForDisplay.ToString();
    }
}