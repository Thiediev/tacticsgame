using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsDisplay : MonoBehaviour {

    public static PointsDisplay instance;

    public int PointsForDisplay;
    public Unit thisUnit = instance.transform.parent.transform.GetComponent<Unit>();

    [SerializeField]
    private Text pointTxtRef;

    private void Awake()
    {
        pointTxtRef = GetComponent<Text>();//or provide from somewhere else (e.g. if you want via find GameObject.Find("CountText").GetComponent<Text>();)
        //unitRef = GetComponent<Unit>();
    }

    private void Start()
    {
        PointsForDisplay = 10;
        pointTxtRef.text = "";
    }

    private void Update()
    {
        // If it's the win screen, display the amount of points earned per unit
        if (GameManager.instance.WinScreenOn == true)
            UpdatePointsDisplay();

        // If it's not the win screen, display nothing
        if (GameManager.instance.WinScreenOn == false)
            ErasePointsDisplay();
    }

    public void ErasePointsDisplay()
    {
        pointTxtRef.text = "";
    }

    public void UpdatePointsDisplay()
    {
        PointsForDisplay = thisUnit.UnitPoints;
        pointTxtRef.text = PointsForDisplay.ToString();
    }
}