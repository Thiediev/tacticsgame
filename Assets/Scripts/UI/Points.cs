using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{

    public static Points instance;
    private Text pointsTxtRef;

    [SerializeField]
    private Text pointsText;

    private void Awake()
    {
        pointsTxtRef = GetComponent<Text>();
    }
    //then where you need:
    private void Start()
    {
        pointsText.text = "0";
    }

    private void Update()
    {
        UpdatePoints();

    }

    public void UpdatePoints()
    {
        //fundoRundo = GameManager.instance.fundsArmyOne;
        pointsText.text = GameManager.instance.totalPoints.ToString();
    }
}
